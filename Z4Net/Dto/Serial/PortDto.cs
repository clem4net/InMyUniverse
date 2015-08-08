using System.Collections.Generic;
using System.IO.Ports;
using System.Runtime.Serialization;

namespace Z4Net.Dto.Serial
{
    /// <summary>
    /// COM port.
    /// </summary>
    [DataContract]
    public class PortDto
    {

        /// <summary>
        /// Buffer of bytes not processed.
        /// </summary>
        public Queue<byte> ReadBuffer { get; set; } = new Queue<byte>();

        /// <summary>
        /// Message received.
        /// </summary>
        public MessageDto ReceiveMessage { get; set; } = new MessageDto();

        /// <summary>
        /// True if port is open.
        /// </summary>
        [DataMember]
        public bool IsOpen { get; set; }

        /// <summary>
        /// Port name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Raw serial port.
        /// </summary>
        public SerialPort RawPort { get; set; }

    }
}
