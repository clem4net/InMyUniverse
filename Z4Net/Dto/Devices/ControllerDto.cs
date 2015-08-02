﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Z4Net.Dto.Serial;

namespace Z4Net.Dto.Devices
{
    /// <summary>
    /// Controler description.
    /// </summary>
    [DataContract]
    public class ControlerDto : DeviceDto
    {

        /// <summary>
        /// API version.
        /// </summary>
        [XmlAttribute("apiVersion")]
        public decimal ApiVersion { get; set; }

        /// <summary>
        /// True if controler of the port is initialized.
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public bool IsReady { get; set; }

        /// <summary>
        /// Manufacturer identifier.
        /// </summary>
        [XmlAttribute("manufacterId")]
        public string ManufacturerIdentifier { get; set; }

        /// <summary>
        /// List of nodes Z nodes linked to the controler.
        /// </summary>
        [DataMember]
        public List<DeviceDto> Nodes { get; set; } = new List<DeviceDto>();

        /// <summary>
        /// Port of the controler.
        /// </summary>
        [DataMember]
        public PortDto Port { get; set; }

        /// <summary>
        /// Product identifier.
        /// </summary>
        [XmlAttribute("productId")]
        public string ProductIdentifier { get; set; }

        /// <summary>
        /// Product type.
        /// </summary>
        [XmlAttribute("productType")]
        public string ProductType { get; set; }

        /// <summary>
        /// Controler API capabilities.
        /// </summary>
        [XmlAttribute("apiCapabilities", DataType = "hexBinary")]
        public List<byte> ApiCapabilities { get; set; }

        /// <summary>
        /// Z version.
        /// </summary>
        [XmlAttribute("zVersion")]
        public string ZVersion { get; set; }

    }
}
