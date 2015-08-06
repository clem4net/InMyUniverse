using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Z4Net.Business.Messaging;
using Z4Net.Dto.Devices;
using Z4Net.Dto.Messaging;
using Z4Net.Dto.Serial;

namespace Z4Net.Business.Devices
{

    /// <summary>
    /// Binary switch mangament.
    /// </summary>
    internal class SwitchBinaryBusiness : IDevice
    {

        #region Private data

        /// <summary>
        /// Acknowledgment wait event.
        /// </summary>
        private static readonly EventWaitHandle WaitAcknowledgment = new EventWaitHandle(false, EventResetMode.AutoReset);

        /// <summary>
        /// Wait response event.
        /// </summary>
        private static readonly EventWaitHandle WaitResponseEvent = new EventWaitHandle(false, EventResetMode.AutoReset);

        /// <summary>
        /// Wait report event.
        /// </summary>
        private static readonly EventWaitHandle WaitReportEvent = new EventWaitHandle(false, EventResetMode.AutoReset);

        #endregion

        #region IDevice methods

        /// <summary>
        /// Get switch value.
        /// </summary>
        /// <param name="controller">Concerned controller.</param>
        /// <param name="node">Concerned node.</param>
        /// <returns>Value of node.</returns>
        public bool Get(ControllerDto controller, DeviceDto node)
        {
            // send message
            node.Value = null;
            if (MessageQueueBusiness.Send(controller, CreateCommandMessage(node, new List<byte> {(byte) SwitchBinaryAction.Get})))
            {
                // wait for response from controller
                WaitAcknowledgment.WaitOne(DeviceConstants.WaitEventTimeout);
                WaitResponseEvent.WaitOne(DeviceConstants.WaitEventTimeout);
                WaitReportEvent.WaitOne(DeviceConstants.WaitEventTimeout);
            }

            return node.Value != null;
        }

        /// <summary>
        /// Set ON a switch.
        /// </summary>
        /// <param name="controller">Concerned controller.</param>
        /// <param name="node">Concerned node.</param>
        /// <param name="value">"0xFF" to set on, "0x00" to set off.</param>
        /// <returns>Updated node.</returns>
        public bool Set(ControllerDto controller, DeviceDto node, List<byte> value)
        {
            // send message
            if (MessageQueueBusiness.Send(controller, CreateCommandMessage(node, new List<byte> {(byte) SwitchBinaryAction.Set, value.FirstOrDefault()})))
            {
                // wait for response from controller
                WaitAcknowledgment.WaitOne(DeviceConstants.WaitEventTimeout);
                WaitResponseEvent.WaitOne(DeviceConstants.WaitEventTimeout);
                Get(controller, node);
            }

            // get node value
            return node.Value == BitConverter.ToString(value.ToArray());
        }

        /// <summary>
        /// Acknowlegment received.
        /// </summary>
        public void AcknowlegmentReceived(MessageHeader ack)
        {
            WaitAcknowledgment.Set();
        }

        /// <summary>
        /// A request is received.
        /// </summary>
        /// <param name="receivedMessage">Received request.</param>
        /// <returns>Process state.</returns>
        public void RequestRecevied(MessageDto receivedMessage)
        {
            // update state of switch
            if (receivedMessage.Content.Count >= 5)
            {
                // report action
                var switchAction = (SwitchBinaryAction)receivedMessage.Content[4];
                if (switchAction == SwitchBinaryAction.Report)
                {
                    // Update node value
                    var valueLength = receivedMessage.Content[2] - 2;
                    var rawValue = receivedMessage.Content.Skip(receivedMessage.Content.Count - valueLength).Take(valueLength).ToList();
                    receivedMessage.Node.Value = BitConverter.ToString(rawValue.ToArray());

                    // release event
                    WaitReportEvent.Set();
                }
            }
        }

        /// <summary>
        /// Receive a response message from binary switch.
        /// </summary>
        /// <param name="receivedMessage">Received message.</param>
        public void ResponseReceived(MessageDto receivedMessage)
        {
            // if message received is good, wait event is released.
            if (receivedMessage.Content.Count == 1 && receivedMessage.Content.First() == 0x01)
            {
                WaitResponseEvent.Set();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Create a command message.
        /// </summary>
        /// <param name="node">Concerned node.</param>
        /// <param name="content">Content to send.</param>
        /// <returns>Message.</returns>
        private static MessageDto CreateCommandMessage(DeviceDto node, List<byte> content)
        {
            var result = new MessageDto
            {
                Command = MessageCommand.SendData,
                Content = new List<byte> { (byte)CommandClass.SwitchBinaryAction },
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
