using System;
using System.Collections.Generic;
using Z4Net.Dto.Devices;

namespace Z4Net.Dto.Messaging
{
    /// <summary>
    /// Generic message.
    /// </summary>
    public abstract class AMessageDto
    {

        /// <summary>
        /// Message command.
        /// </summary>
        public MessageCommand Command { get; set; }

        /// <summary>
        /// Raw message received.
        /// </summary>
        public List<byte> Content { get; set; } = new List<byte>();

        /// <summary>
        /// Creation date.
        /// </summary>
        public DateTime Date { get; } = DateTime.Now;

        /// <summary>
        /// True if message is valid.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Node concerned by the message.
        /// </summary>
        public DeviceDto Node { get; set; }

        /// <summary>
        /// Message type.
        /// </summary>
        public MessageType Type { get; set; }

        /// <summary>
        /// Node identifier concerned. 0 if no node is concerned.
        /// </summary>
        public int ZIdentifier { get; set; }

    }
}
