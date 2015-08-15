using System.Collections.Generic;
using Z4Net.Dto.Serial;

namespace Z4Net.Dto.Devices
{
    /// <summary>
    /// Controller description.
    /// </summary>
    public class ControllerDto : DeviceDto
    {

        /// <summary>
        /// API version.
        /// </summary>
        public decimal ApiVersion { get; set; }

        /// <summary>
        /// True if controller of the port is initialized.
        /// </summary>
        public bool IsReady { get; set; }

        /// <summary>
        /// List of nodes Z nodes linked to the controller.
        /// </summary>
        public List<DeviceDto> Nodes { get; set; } = new List<DeviceDto>();

        /// <summary>
        /// Port of the controller.
        /// </summary>
        public PortDto Port { get; set; }

        /// <summary>
        /// Product type.
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// Controller API capabilities.
        /// </summary>
        public List<byte> ApiCapabilities { get; set; }

        /// <summary>
        /// Z version.
        /// </summary>
        public string ZVersion { get; set; }

    }
}
