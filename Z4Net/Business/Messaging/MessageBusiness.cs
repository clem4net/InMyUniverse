using System.Collections.Generic;
using System.Linq;
using Z4Net.Business.Devices;
using Z4Net.Dto.Devices;
using Z4Net.Dto.Messaging;
using Z4Net.Dto.Serial;

namespace Z4Net.Business.Messaging
{
    /// <summary>
    /// Message business.
    /// </summary>
    internal static class MessageBusiness
    {

        #region Internal methods

        /// <summary>
        /// Create message for serial port.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <returns>Message frame.</returns>
        internal static SerialMessageDto ConvertToSerial(MessageDto message)
        {
            var content = CreateFrame(message);
            var result = new SerialMessageDto
            {
                Content = content,
                IsComplete = true,
                Size = content.Count
            };

            return result;
        }

        /// <summary>
        /// Convert a serial mesage to business message.
        /// </summary>
        /// <param name="message">Serial message.</param>
        /// <returns>Business message.</returns>
        internal static MessageDto ConvertToMessage(SerialMessageDto message)
        {
            var result = new MessageDto();

            if (message.Header == MessageHeader.StartOfFrame)
            {
                result.IsValid = ValidMessage(message.Content);
                if (result.IsValid && message.Content.Count > 2)
                {
                    result.Type = (MessageType)message.Content[2];
                    if (message.Content.Count > 4)
                    {
                        result.Command = (MessageCommand)message.Content[3];
                        result.Content = message.Content.Skip(4).Take(message.Content.Count - 5).ToList();
                    }
                }
            }
            else
            {
                result.Content = message.Content;
            }

            return result;
        }

        /// <summary>
        /// Process a response message.
        /// </summary>
        /// <param name="sendMessage">Message send.</param>
        /// <param name="receivedMessage">Message received.</param>
        internal static void ProcessResponse(MessageDto sendMessage, MessageDto receivedMessage)
        {
            receivedMessage.Node = sendMessage.Node;
            receivedMessage.ZIdentifier = sendMessage.ZIdentifier;
            DevicesBusiness.ResponseReceived(sendMessage, receivedMessage);
        }

        /// <summary>
        /// Process request message.
        /// </summary>
        /// <param name="receivedMessage">Message received.</param>
        internal static void ProcessRequest(MessageDto receivedMessage)
        {
            var cmdClass = CommandClass.None;

            if (receivedMessage.Command == MessageCommand.NodeValueChanged)
            {
                receivedMessage.Node = new DeviceDto { ZIdentifier = receivedMessage.Content[1] };
                cmdClass = (CommandClass)receivedMessage.Content[3];
            }

            DevicesBusiness.RequestReceived(receivedMessage, cmdClass);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Create raw frame.
        /// </summary>
        /// <param name="message">Original message.</param>
        /// <returns>Message.</returns>
        private static List<byte> CreateFrame(MessageDto message)
        {
            var result = new List<byte>
            {
                (byte) MessageHeader.StartOfFrame,
                0,
                (byte) message.Type,
                (byte) message.Command
            };

            if (message.ZIdentifier != 0)
            {
                result.Add(message.ZIdentifier);
            }

            result.AddRange(message.Content);
            result.Add((byte) (TransmitOptions.Acknowlegment | TransmitOptions.Explore | TransmitOptions.NoRoute));
            result[1] = (byte) (result.Count - 1);
            result.Add(GenerateChecksum(result));

            return result;
        }

        /// <summary>
        /// Generate a Z checksum.
        /// </summary>
        /// <param name="data">Message data.</param>
        /// <returns>Checksum byte.</returns>
        private static byte GenerateChecksum(List<byte> data)
        {
            var result = data[1];
            for (var i = 2; i < data.Count; i++)
            {
                result ^= data[i];
            }
            result = (byte)(~result);
            return result;
        }

        /// <summary>
        /// Valid message data.
        /// </summary>
        /// <param name="data">Message data.</param>
        /// <returns>True if message is valid.</returns>
        private static bool ValidMessage(List<byte> data)
        {
            var result = false;

            if (data[1] == data.Count - 2)
            {
                result = ValidChecksum(data);
            }

            return result;
        }

        /// <summary>
        /// Valid the message checksum.
        /// </summary>
        /// <param name="data">Complete message.</param>
        /// <returns>Checksum valid result.s</returns>
        private static bool ValidChecksum(List<byte> data)
        {
            var checksum = data[1];
            for (var i = 2; i < data.Count - 1; i++)
            {
                checksum ^= data[i];
            }
            checksum = (byte)(~checksum);

            return checksum == data[data.Count - 1];
        }

        #endregion

    }
}
