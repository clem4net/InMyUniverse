using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Z4Net.Dto.Devices
{
    /// <summary>
    /// Node description.
    /// </summary>
    [DataContract]
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
        [DataMember]
        public DeviceClassGeneric DeviceClassGeneric { get; set; } 

        /// <summary>
        /// Value of the node.
        /// </summary>
        [XmlAttribute("currentValue")]
        [DataMember]
        public string Value { get; set; }

        /// <summary>
        /// Z-Wave identifier of the node.
        /// </summary>
        [XmlAttribute("zId")]
        [DataMember]
        public int ZIdentifier { get; set; }

    }
}
