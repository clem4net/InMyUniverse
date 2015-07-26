using System.Collections.Generic;

namespace Z4Net.Dto.Serial
{
    /// <summary>
    /// Serial message.
    /// </summary>
    public class SerialMessageDto
    {

        /// <summary>
        /// Initialize object.
        /// </summary>
        public SerialMessageDto()
        {
            Content = new List<byte>();
        }

        /// <summary>
        /// Current message received.
        /// </summary>
        public List<byte> Content { get; set; }

        /// <summary>
        /// Message header.
        /// </summary>
        public MessageHeader Header { get; set; }

        /// <summary>
        /// Current message size.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// True if message is complete.
        /// </summary>
        public bool IsComplete { get; set; }

    }
}
