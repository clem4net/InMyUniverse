using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Technical;
using Z4Net.Dto.Devices;

namespace Z4Net.Dto.Services
{
    /// <summary>
    /// Node, represents a business device.
    /// </summary>
    [Table("Devices")]
    [DataContract]
    public class NodeDto
    {

        /// <summary>
        /// Controller home identifier.
        /// </summary>
        [MaxLength(20), Column("HOMEID")]
        [DataMember]
        public string HomeIdentifier { get; set; }

        /// <summary>
        /// Node unique identifier.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), Column("ID")]
        [DataMember]
        public int Identifier { get; set; }

        /// <summary>
        /// Device type.
        /// </summary>
        [Column("TYPE")]
        [DataMember]
        public DeviceClassGeneric Type { get; set; }

        /// <summary>
        /// Raw value.
        /// </summary>
        [NotMapped]
        [DataMember]
        public List<byte> Value
        {
            get { return ValueProxy.ToHexList(); }
            set { ValueProxy = value != null ? value.ToHexString() : string.Empty; }
        }

        /// <summary>
        /// Node value.
        /// </summary>
        [MaxLength(100), Column("VALUE")]
        [DataMember]
        public string ValueProxy { get; set; }

        /// <summary>
        /// Device Z identifier.
        /// </summary>
        [Column("ZID")]
        [DataMember]
        public int ZIdentifier { get; set; }

    }
}
