using System.Collections.Generic;
using System.Threading;
using Z4Net.Business.Messaging;
using Z4Net.Dto.Devices;
using Z4Net.Dto.Messaging;

namespace Z4Net.Business.Devices
{
    /// <summary>
    /// Constructor business.
    /// </summary>
    internal class ConstructorBusiness : IDevice
    {

        #region Private data

        /// <summary>
        /// Acknowledgment wait event.
        /// </summary>
        private static readonly EventWaitHandle WaitAcknowledgment = new EventWaitHandle(false, EventResetMode.AutoReset);

        /// <summary>
        /// Wait event from node.
        /// </summary>
        private static readonly EventWaitHandle WaitEvent = new EventWaitHandle(false, EventResetMode.AutoReset);

        /// <summary>
        /// Wait event from node.
        /// </summary>
        private static readonly EventWaitHandle WaitReport = new EventWaitHandle(false, EventResetMode.AutoReset);

        #endregion

        #region Device methods

        /// <summary>
        /// Get the value of the device.
        /// </summary>
        /// <param name="controller">Concerned controller.</param>
        /// <param name="device">Concerned device.</param>
        /// <returns>True if device value is completed.</returns>
        public bool Get(ControllerDto controller, DeviceDto device)
        {
            // send message
            if (MessageProcessBusiness.Send(controller, CreateCommandMessage(controller, device.ZIdentifier)))
            {
                // wait for ack and response
                WaitAcknowledgment.WaitOne(DeviceConstants.WaitEventTimeout);
                WaitEvent.WaitOne(DeviceConstants.WaitEventTimeout);
                WaitReport.WaitOne(DeviceConstants.WaitEventTimeout);
            }

            return !string.IsNullOrEmpty(device.ConstructorIdentifier) && !string.IsNullOrEmpty(device.ProductIdentifier);
        }

        /// <summary>
        /// No possible update of constructor.
        /// </summary>
        /// <param name="controller">Concerned controller.</param>
        /// <param name="device">Concerned device.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>True if value is setted.</returns>
        public bool Set(ControllerDto controller, DeviceDto device, List<byte> value)
        {
            return true;
        }

        /// <summary>
        /// Acknowlegment received.
        /// </summary>
        /// <param name="message">Received message.</param>
        public void AcknowlegmentReceived(MessageFromDto message)
        {
            WaitAcknowledgment.Set();
        }

        /// <summary>
        /// A response a received from node.
        /// </summary>
        /// <param name="resposne">Response message.</param>
        public void ResponseReceived(MessageFromDto resposne)
        {
            WaitEvent.Set();
        }

        /// <summary>
        /// No request received.
        /// </summary>
        /// <param name="request">Received message.</param>
        public void RequestRecevied(MessageFromDto request)
        {
            if (request.Content.Count > 6 && request.Content[0] == (byte)ConstructorAction.Report)
            {
                request.Node.ConstructorIdentifier = string.Concat(request.Content[1].ToString("X2"), request.Content[2].ToString("0:X2"));
                // content 3 and 4 are product type
                request.Node.ProductIdentifier = string.Concat(request.Content[5].ToString("X2"), request.Content[6].ToString("0:X2"));
            }

            WaitReport.Set();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Create a command message.
        /// </summary>
        /// <param name="controller">Contextual node of the message.</param>
        /// <param name="zId">Node identifier to send in message. 0x00 if no node is concerned.</param>
        /// <returns>Message.</returns>
        private static MessageToDto CreateCommandMessage(DeviceDto controller, int zId)
        {
            var result = new MessageToDto
            {
                Command = MessageCommand.SendData,
                IsValid = true,
                Node = controller,
                ZIdentifier = (byte)zId,
                Type = MessageType.Request,
                Content = new List<byte> { 0x02, (byte)RequestCommandClass.Constructor, (byte)ConstructorAction.Get },

                IsConstructor = true
            };

            return result;
        }

        #endregion

    }
}
