﻿using System.Collections.Generic;
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
        public Queue<byte> CurrentBuffer { get; set; } = new Queue<byte>();

        /// <summary>
        /// Current received message.
        /// </summary>
        public SerialMessageDto SerialMessage { get; set; } = new SerialMessageDto();

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
