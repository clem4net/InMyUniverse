using System.Collections.Generic;
using System.Threading.Tasks;
using Z4Net.Business.Serial;
using Z4Net.Dto.Devices;
using Z4Net.Dto.Messaging;
using Z4Net.Dto.Serial;

namespace Z4Net.Business.Messaging
{
    /// <summary>
    /// Message business.
    /// </summary>
    internal static class MessageQueueBusiness
    {

        #region Private properties

        /// <summary>
        /// Message queue.
        /// </summary>
        private static MessageQueueDto Queue { get; set; }

        #endregion

        #region Internal methods

        /// <summary>
        /// Connect to a port.
        /// </summary>
        /// <param name="port">Port to connect.</param>
        /// <returns>Connected port.</returns>
        internal static PortDto Connect(PortDto port)
        {
            // close existing port
            Close();

            // connect port
            port = PortBusiness.Connect(port);

            // initialize queue
            if (Queue == null) Queue = new MessageQueueDto {Port = port, State = QueueState.Stop};

            // start listener
            if (Queue.TaskLaunch == false)
            {
                Queue.TaskContinue = true;
                Queue.TaskReceive = new Task(() => ReceiveTask(Queue));
                Queue.TaskReceive.Start();
                Queue.TaskLaunch = true;
            }

            return port;
        }

        /// <summary>
        /// List ports accessible.
        /// </summary>
        /// <returns>Port list.</returns>
        internal static List<PortDto> ListPorts()
        {
            return PortBusiness.List();
        }

        /// <summary>
        /// Send a message.
        /// </summary>
        /// <param name="controller">Concerned controller.</param>
        /// <param name="message">Message to send.</param>
        internal static void Send(ControllerDto controller, MessageDto message)
        {
            lock (Queue)
            {
                Queue.Messages.Enqueue(message);
                if (Queue.State == QueueState.Stop) Task.Run(() => SendNextMessage(Queue));
            }
        }

        /// <summary>
        /// Close messaging.
        /// </summary>
        internal static void Close()
        {
            if (Queue != null)
            {
                Queue.TaskContinue = false;
                Queue.TaskLaunch = false;
            }
            PortBusiness.Close();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Send messages from queue for a port.
        /// </summary>
        /// <param name="queue">Queue to process.</param>
        private static void SendNextMessage(MessageQueueDto queue)
        {
            // start queue
            lock(queue) queue.State = QueueState.Process;

            while (queue.State != QueueState.Stop)
            {
                var wait = false;
                lock (queue)
                {
                    if (queue.Messages.Count != 0 && queue.State == QueueState.Wait)
                    {
                        wait = true;   
                    }
                    else if (queue.Messages.Count != 0)
                    {
                        // get message to send
                        var message = queue.Messages.Dequeue();
                        queue.ProcessingMessage = message;

                        // send message
                        queue.Port.SerialMessage = MessageBusiness.ConvertToSerial(message);
                        PortBusiness.Send(queue.Port);

                        // wait for message response
                        queue.State = QueueState.Wait;
                        wait = true;
                    }
                    else
                    {
                        queue.State = QueueState.Stop;
                    }
                }

                if (wait) Task.Delay(MessageConstants.WaitSendTask);
            }
        }

        /// <summary>
        /// Send an acknowledgment.
        /// </summary>
        /// <param name="queue">Message queue.</param>
        /// <param name="receivedMessage">Received message.</param>
        private static void SendAcknowledgment(MessageQueueDto queue, MessageDto receivedMessage)
        {
            queue.Port.SerialMessage = new SerialMessageDto
            {
                Content = new List<byte>
                {
                    receivedMessage.IsValid ? (byte) MessageHeader.Acknowledgment : (byte) MessageHeader.NotAcknowledgment
                }
            };
            PortBusiness.Send(queue.Port);
        }

        /// <summary>
        /// Called when a message is received.
        /// </summary>
        /// <param name="queue">Concerned queue.</param>
        private static async void ReceiveTask(MessageQueueDto queue)
        {
            while (queue.TaskContinue)
            {
                // get serial message
                var serialMessage = PortBusiness.Receive(queue.Port);

                // SOF message
                if (serialMessage.Header == MessageHeader.StartOfFrame)
                {
                    // get business message & send ACK
                    var message = MessageBusiness.ConvertToMessage(serialMessage);
                    SendAcknowledgment(queue, message);

                    // process
                    if (message.IsValid)
                    {
                        if (message.Type == MessageType.Request)
                        {
                            MessageBusiness.ProcessRequest(message);
                        }
                        else
                        {
                            MessageBusiness.ProcessResponse(queue.ProcessingMessage, message);
                            lock (Queue) Queue.State = Queue.State == QueueState.Stop ? QueueState.Stop : QueueState.Process;
                        }
                    }
                }

                // wait if no message is got
                if (serialMessage.Size == 0) await Task.Delay(MessageConstants.WaitSendTask);
            }
        }

        #endregion

    }
}
