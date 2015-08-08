using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Z4Net.Business.Devices;
using Z4Net.Business.Serial;
using Z4Net.Dto.Devices;
using Z4Net.Dto.Messaging;
using Z4Net.Dto.Serial;
using Z4Net.DtoFactory.Messaging;

namespace Z4Net.Business.Messaging
{
    /// <summary>
    /// Message business.
    /// </summary>
    internal static class MessageProcessBusiness
    {

        #region Private properties

        /// <summary>
        /// Message queue.
        /// </summary>
        private static MessageProcessDto Queue { get; set; }

        #endregion

        #region Internal methods

        /// <summary>
        /// Close messaging.
        /// </summary>
        /// <param name="port">Optional port to close. If null, close all ports.</param>
        internal static void Close(PortDto port = null)
        {
            if (Queue != null)
            {
                Queue.TaskContinue = false;
                Queue.TaskReceive = null;
            }
            PortBusiness.Close(port);
        }

        /// <summary>
        /// Connect to a port.
        /// </summary>
        /// <param name="controller">Controller to connect.</param>
        /// <returns>Connected controller.</returns>
        internal static ControllerDto Connect(ControllerDto controller)
        {
            // close existing port
            Close();

            // connect port
            controller.Port = PortBusiness.Connect(controller.Port);

            // initialize queue
            Queue = new MessageProcessDto { Controller = controller };

            // start listener
            if (Queue.TaskReceive == null)
            {
                Queue.TaskContinue = true;
                Queue.TaskReceive = new Task(() => ReceiveTask(Queue));
                Queue.TaskReceive.Start();
            }

            return controller;
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
        internal static bool Send(ControllerDto controller, MessageToDto message)
        {
            // get serial message
            MessageDto serialMessage;
            using (var ctx = new MessageProcessDtoFactory())
            {
                serialMessage = ctx.Convert(message);
            }

            // wait for sending turn & request process
            Queue.WaitMessageSending.WaitOne(DeviceConstants.WaitEventTimeout);
            Queue.WaitMessageReception.WaitOne(DeviceConstants.WaitEventTimeout);

            // set processed message
            Queue.MessageTo = message;

            // send message
            var result = PortBusiness.Send(Queue.Controller.Port, serialMessage);

            Queue.WaitMessageReception.Set();

            // wait round time trip
            Thread.Sleep(MessageConstants.WaitForRoundTimeTrip);
            return result;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Called when a message is received.
        /// </summary>
        /// <param name="messageProcess">Concerned queue.</param>
        private static async void ReceiveTask(MessageProcessDto messageProcess)
        {
            while (messageProcess.TaskContinue)
            {
                // get reception lock
                messageProcess.WaitMessageReception.WaitOne(DeviceConstants.WaitEventTimeout);

                // get message
                var serialMessage = PortBusiness.Receive(messageProcess.Controller.Port);

                // process
                if (serialMessage.IsComplete)
                {
                    // get message
                    using (var ctx = new MessageProcessDtoFactory())
                    {
                        messageProcess.MessageFrom = ctx.Convert(serialMessage);
                    }

                    // send ACK
                    if (messageProcess.MessageFrom.Header == MessageHeader.StartOfFrame) SendAcknowledgment(messageProcess);

                    // process received message
                    if(messageProcess.MessageFrom.IsValid) ProcessFrame(messageProcess);
                }

                // round trip time
                if (serialMessage.Content.Count == 0) await Task.Delay(MessageConstants.WaitReceiveTask);


                // release reception & request lock
                messageProcess.WaitMessageReception.Set();
            }
        }

        /// <summary>
        /// Process a start of frame (SOF).
        /// </summary>
        /// <param name="messageProcess">Message processing.</param>
        private static void ProcessFrame(MessageProcessDto messageProcess)
        {
            // special request
            if (messageProcess.MessageFrom.Type == MessageType.Request && messageProcess.MessageFrom.Content.Count > 1)
            {
                messageProcess.MessageFrom.Node = messageProcess.Controller.Nodes.FirstOrDefault(x => x.ZIdentifier == messageProcess.MessageFrom.ZIdentifier);
            }
            // complete received message if it's a response (SOF and ACK)
            else if (messageProcess.MessageTo != null)
            {
                messageProcess.MessageFrom.Node = messageProcess.MessageTo.Node;
                messageProcess.MessageFrom.ZIdentifier = messageProcess.MessageTo.ZIdentifier;
            }

            // Data frame (SOF)
            if (messageProcess.MessageFrom.Header == MessageHeader.StartOfFrame && messageProcess.MessageFrom.Node != null)
            {
                // process response
                if (messageProcess.MessageTo != null && messageProcess.MessageFrom.Type == MessageType.Response &&
                    messageProcess.MessageTo.Command == messageProcess.MessageFrom.Command)
                {
                    DevicesBusiness.ResponseReceived(messageProcess);
                    messageProcess.WaitMessageSending.Set(); // release message sending lock
                }
                // process request
                else if (messageProcess.MessageFrom.Type == MessageType.Request && 
                         messageProcess.MessageFrom.RequestCommand != RequestCommandClass.None)
                {
                    DevicesBusiness.RequestReceived(messageProcess.MessageFrom);
                }
            }
            // ACK
            else
            {
                DevicesBusiness.AcknowledgmentReceived(messageProcess);
            }
        }

        /// <summary>
        /// Send an acknowledgment.
        /// </summary>
        /// <param name="messageProcess">Message processing to use.</param>
        private static void SendAcknowledgment(MessageProcessDto messageProcess)
        {
            Thread.Sleep(MessageConstants.WaitForRoundTimeTrip);
            var message = new MessageDto
            {
                Content = new List<byte>
                {
                    messageProcess.MessageFrom.IsValid ? (byte) MessageHeader.Acknowledgment : (byte) MessageHeader.NotAcknowledgment
                }
            };
            PortBusiness.Send(messageProcess.Controller.Port, message);
        }

        #endregion

    }
}
