﻿using System.Collections.Generic;
using System.Threading;
using Z4Net.Business.Messaging;
using Z4Net.Dto.Devices;
using Z4Net.Dto.Messaging;

namespace Z4Net.Business.Devices
{
    /// <summary>
    /// Node configuration.
    /// </summary>
    public class ConfigurationBusiness : IDevice
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

        #endregion

        #region IDevice methods

        /// <summary>
        /// Get the value of the device.
        /// </summary>
        /// <param name="controller">Concerned controller.</param>
        /// <param name="device">Concerned device.</param>
        /// <returns>True if device value is completed.</returns>
        public bool Get(ControllerDto controller, DeviceDto device)
        {
            return false;
        }

        /// <summary>
        /// Set the device value.
        /// </summary>
        /// <param name="controller">Concerned controller.</param>
        /// <param name="device">Concerned device.</param>
        /// <param name="value">Value to set. First byte is parameter identifier, next is parameter value.</param>
        /// <returns>True if value is setted.</returns>
        public bool Set(ControllerDto controller, DeviceDto device, List<byte> value)
        {
            return true;
        }

        /// <summary>
        /// Acknowlegment received.
        /// </summary>
        /// <param name="receivedMessage">Recevied message.</param>
        public void AcknowlegmentReceived(MessageFromDto receivedMessage)
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
        /// A request is recevied from node.
        /// </summary>
        /// <param name="request">Received message.</param>
        public void RequestRecevied(MessageFromDto request)
        {
        }

        /// <summary>
        /// Configure a parameter.
        /// </summary>
        /// <param name="controller">Controller used to send message.</param>
        /// <param name="node">Concerned message.</param>
        /// <param name="parameter">Parameter identifier.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>True if configuration is OK.</returns>
        internal static bool Set(ControllerDto controller, DeviceDto node, byte parameter, List<byte> value)
        {
            var content = new List<byte> { (byte)ConfigurationAction.Set, parameter, (byte)value.Count };
            content.AddRange(value);

            // send message
            if (MessageProcessBusiness.Send(controller, CreateCommandMessage(node, content)))
            {
                // wait for ack and response
                WaitAcknowledgment.WaitOne(DeviceConstants.WaitEventTimeout);
                WaitEvent.WaitOne(DeviceConstants.WaitEventTimeout);
            }

            return true;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Create a command message.
        /// </summary>
        /// <param name="node">Concerned node.</param>
        /// <param name="content">Content to send.</param>
        /// <returns>Message.</returns>
        private static MessageToDto CreateCommandMessage(DeviceDto node, List<byte> content)
        {
            var result = new MessageToDto
            {
                Command = MessageCommand.SendData,
                Content = new List<byte> { (byte)RequestCommandClass.Configuration },
                IsConfiguration = true,
                IsValid = true,
                Node = node,
                Type = MessageType.Request,
                ZIdentifier = node.ZIdentifier
            };

            // fill content
            result.Content.AddRange(content);
            result.Content.Insert(0, (byte)result.Content.Count);

            return result;
        }

        #endregion

    }
}
