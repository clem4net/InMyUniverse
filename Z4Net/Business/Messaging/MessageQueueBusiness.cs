using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Z4Net.Business.Devices;
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

        /// <summary>
        /// Event used to wait the end of the message process.
        /// </summary>
        private static readonly EventWaitHandle WaitMessageProcessing = new EventWaitHandle(true, EventResetMode.AutoReset);

        #endregion

        #region Internal methods

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
            Queue = new MessageQueueDto {Port = port };

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
        internal static bool Send(ControllerDto controller, MessageDto message)
        {
            // get serial message
            var serialMessage = MessageBusiness.ConvertToSerial(message);

            // wait for turn
            WaitMessageProcessing.WaitOne(DeviceConstants.WaitEventTimeout);

            // set processed message
            Queue.ProcessingMessage = message;

            // send message
            return PortBusiness.Send(Queue.Port, serialMessage);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Called when a message is received.
        /// </summary>
        /// <param name="queue">Concerned queue.</param>
        private static async void ReceiveTask(MessageQueueDto queue)
        {
            while (queue.TaskContinue)
            {
                // get message
                var serialMessage = PortBusiness.Receive(queue.Port);

                // process
                if (serialMessage.IsComplete)
                {
                    // get message and send ack
                    var message = MessageBusiness.ConvertToMessage(serialMessage);
                    if (message.IsDataFrame) SendAcknowledgment(queue.Port, message);

                    // process received message
                    if(message.IsValid) ProcessFrame(queue.ProcessingMessage, message);
                }

                // round trip time
                await Task.Delay(MessageConstants.WaitSendTask);
            }
        }

        /// <summary>
        /// Process a start of frame (SOF).
        /// </summary>
        /// <param name="requestMessage">Processed message.</param>
        /// <param name="receivedMessage">Received message.</param>
        private static void ProcessFrame(MessageDto requestMessage, MessageDto receivedMessage)
        {
            // complete received message
            if (requestMessage != null)
            {
                receivedMessage.Node = requestMessage.Node;
                receivedMessage.ZIdentifier = requestMessage.ZIdentifier;
            }

            // Data frame (SOF)
            if (receivedMessage.IsDataFrame)
            {
                // process response
                if (receivedMessage.Type == MessageType.Response && requestMessage != null && requestMessage.Command == receivedMessage.Command)
                {
                    DevicesBusiness.ResponseReceived(receivedMessage);
                    WaitMessageProcessing.Set(); // release message lock
                }
                // process request
                else if (receivedMessage.Type == MessageType.Request && receivedMessage.Command == MessageCommand.NodeValueChanged && requestMessage != null)
                {
                    receivedMessage.Node = requestMessage.Node;
                    receivedMessage.ZIdentifier = receivedMessage.Content[1];
                    DevicesBusiness.RequestReceived(receivedMessage, (CommandClass)receivedMessage.Content[3]);
                }
            }
            // ACK
            else
            {
                DevicesBusiness.AcknowledgmentReceived(receivedMessage);
            }
        }


        /// <summary>
        /// Send an acknowledgment.
        /// </summary>
        /// <param name="port">Port to use.</param>
        /// <param name="receivedMessage">Received message.</param>
        private static void SendAcknowledgment(PortDto port, MessageDto receivedMessage)
        {
            var message = new SerialMessageDto
            {
                Content = new List<byte>
                {
                    receivedMessage.IsValid ? (byte) MessageHeader.Acknowledgment : (byte) MessageHeader.NotAcknowledgment
                }
            };
            PortBusiness.Send(port, message);
        }

        #endregion

    }
}
