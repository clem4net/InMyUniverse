using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Z4Net.Dto.Devices
{
    /// <summary>
    /// Node description.
    /// </summary>
    public class DeviceDto
    {

        /// <summary>
        /// Home identifier of the controller of the node.
        /// </summary>
        [XmlAttribute("controllerHomeId")]
        public string HomeIdentifier { get; set; }

        /// <summary>
        /// Device type.
        /// </summary>
        [XmlAttribute("baseClass")]
        public DeviceClass DeviceClass { get; set; }

        /// <summary>
        /// Device class of the node.
        /// </summary>
        [XmlAttribute("genericClass")]
        public DeviceClassGeneric DeviceClassGeneric { get; set; } 

        /// <summary>
        /// Value of the node.
        /// </summary>
        [XmlAttribute("currentValue")]
        public List<byte> Value { get; set; }

        /// <summary>
        /// Z-Wave identifier of the node.
        /// </summary>
        [XmlAttribute("zId")]
        public int ZIdentifier { get; set; }

    }
}
