using System.Collections.Generic;
using System.IO.Ports;

namespace Z4Net.Dto.Serial
{
    /// <summary>
    /// COM port.
    /// </summary>
    public class PortDto
    {

        /// <summary>
        /// Initiliaze.
        /// </summary>
        public PortDto()
        {
            CurrentBuffer = new Queue<byte>();
            SerialMessage = new SerialMessageDto();
        }

        /// <summary>
        /// Buffer of bytes not processed.
        /// </summary>
        public Queue<byte> CurrentBuffer { get; set; }

        /// <summary>
        /// Current received message.
        /// </summary>
        public SerialMessageDto SerialMessage { get; set; }

        /// <summary>
        /// True if port is open.
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Port name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Raw serial port.
        /// </summary>
        public SerialPort RawPort { get; set; }

    }
}
