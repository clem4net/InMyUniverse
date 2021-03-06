﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using Technical;
using Z4Net.Dto.Serial;

namespace Z4Net.DtoFactory.Serial
{
    /// <summary>
    /// Serial port communication helper.
    /// </summary>
    internal class PortDtoFactory : IDisposable
    {

        #region Internal methods

        /// <summary>
        /// Close a port.
        /// </summary>
        /// <param name="port">Port to close.</param>
        /// <returns>Closed port.</returns>
        public PortDto Close(PortDto port)
        {
            try
            {
                port.RawPort?.Close();
                port.IsOpen = false;
            }
            catch
            {
                port.IsOpen = false;
            }

            return port;
        }

        /// <summary>
        /// Connect a port.
        /// </summary>
        /// <param name="port">Port to connect.</param>
        /// <returns>Connected port.</returns>
        public PortDto Connect(PortDto port)
        {
            try
            {
                port.RawPort = new SerialPort(port.Name)
                {
                    BaudRate = 115200,
                    ReceivedBytesThreshold = 1 // API flush when 1 octet is in send buffer
                };
                port.RawPort.Open();
                port.IsOpen = port.RawPort.IsOpen;
            }
            catch
            {
                port.IsOpen = false;
            }

            return port;
        }

        /// <summary>
        /// Dispose the class.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Detect connected ports.
        /// </summary>
        /// <returns>Port list.</returns>
        public List<PortDto> List()
        {
            List<PortDto> result;

            try
            {
                result = SerialPort.GetPortNames().Select(x => new PortDto { Name = x.ToUpperInvariant() }).ToList();
            }
            catch
            {
                result = new List<PortDto>();
            }

            return result;
        }

        /// <summary>
        /// Try to get next data.
        /// </summary>
        /// <param name="port">Port to use.</param>
        /// <returns>Completed message.</returns>
        public MessageDto Receive(PortDto port)
        {
            ReadBytes(port.RawPort).ForEach(port.ReadBuffer.Enqueue);

            // reinitialize message
            if (port.ReceiveMessage.IsComplete)
            {
                port.ReceiveMessage = new MessageDto();
            }

            // complete message
            if (port.ReadBuffer.Count != 0)
            {
                port.ReceiveMessage = BuildMessage(port);
            }

            return port.ReceiveMessage;
        }

        /// <summary>
        /// Send a message.
        /// </summary>
        /// <param name="port">Port to use.</param>
        /// <param name="message">Message to send.</param>
        /// <returns>Send result.</returns>
        public bool Send(PortDto port, MessageDto message)
        {
            bool result;

            try
            {
                port.RawPort.Write(message.Content.ToArray(), 0, message.Content.Count);
                TraceHelper.TraceFrame(message.Content, true);
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Get bytes received.
        /// </summary>
        /// <param name="port">Serial port.</param>
        /// <returns>Received bytes.</returns>
        private List<byte> ReadBytes(SerialPort port)
        {
            byte[] buffer;

            if (port != null && port.IsOpen)
            {
                buffer = new byte[port.BytesToRead];

                if (buffer.Length > 0)
                {
                    try
                    {
                        port.Read(buffer, 0, buffer.Length);
                    }
                    catch
                    {
                        buffer = new byte[0];
                    }
                }
            }
            else
            {
                buffer = new byte[0];
            }

            return buffer.ToList();
        }

        /// <summary>
        /// Build the received message.
        /// </summary>
        /// <param name="port">Port to use.</param>
        /// <returns>Recevied message.</returns>
        private MessageDto BuildMessage(PortDto port)
        {
            var buffer = port.ReadBuffer;
            var message = port.ReceiveMessage;

            // new message
            if (message.Content.Count == 0)
            {
                message.Content.Add(buffer.Dequeue());
            }

            // create a frame
            message.Header = (MessageHeader)message.Content[0];
            if (message.Header == MessageHeader.StartOfFrame)
            {
                // message size
                if (message.Size == 0 && buffer.Count > 0)
                {
                    message.Content.Add(buffer.Dequeue());
                    message.Size = message.Content[1] + 2; // frame size + header byte + size byte
                }

                // message data
                while (message.Content.Count != message.Size && buffer.Count != 0)
                {
                    message.Content.Add(buffer.Dequeue());
                }
                message.IsComplete = message.Content.Count == message.Size;
            }
            // create acknowledgment
            else if (message.Header == MessageHeader.Acknowledgment || message.Header == MessageHeader.NotAcknowledgment)
            {
                message.IsComplete = true;
            }
            if (message.IsComplete) TraceHelper.TraceFrame(message.Content, false);

            return message;
        }

        #endregion

    }
}
