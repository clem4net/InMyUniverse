using System.Collections.Generic;

namespace Z4Net.Dto.Devices
{
    /// <summary>
    /// Node description.
    /// </summary>
    public class DeviceDto
    {

        /// <summary>
        /// Constructor identifier.
        /// </summary>
        public string ConstructorIdentifier { get; set; }

        /// <summary>
        /// Device type.
        /// </summary>
        public DeviceClass DeviceClass { get; set; }

        /// <summary>
        /// Device class of the node.
        /// </summary>
        public DeviceClassGeneric DeviceClassGeneric { get; set; }

        /// <summary>
        /// Home identifier of the controller of the node.
        /// </summary>
        public string HomeIdentifier { get; set; }

        /// <summary>
        /// Product identifier.
        /// </summary>
        public string ProductIdentifier { get; set; }

        /// <summary>
        /// Value of the node.
        /// </summary>
        public List<byte> Value { get; set; }

        /// <summary>
        /// Z-Wave identifier of the node.
        /// </summary>
        public int ZIdentifier { get; set; }

    }
}
