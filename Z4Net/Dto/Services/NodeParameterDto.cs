using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Technical;
using Z4Net.Dto.Services.Definitions;

namespace Z4Net.Dto.Services
{
    /// <summary>
    /// Node parameter.
    /// </summary>
    [Table("NodesParameters")]
    [DataContract]
    public class NodeParameterDto
    {

        /// <summary>
        /// Parameter identifier.
        /// </summary>
        [Column("PRM_ID")]
        public int DefinitionIdentifier { get; set; }

        /// <summary>
        /// Parameter definition.
        /// </summary>
        [NotMapped]
        public ProductParameterDto Definition { get; set; }

        /// <summary>
        /// Parameter identifier.
        /// </summary>
        [Key, Column("ID"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public int Identifier { get; set; }

        /// <summary>
        /// Node identifier.
        /// </summary>
        [Column("NODE_ID")]
        [DataMember]
        public int NodeIdentifier { get; set; }

        /// <summary>
        /// Parameter node.
        /// </summary>
        [ForeignKey("NodeIdentifier")]
        public NodeDto Node { get; set; }

        /// <summary>
        /// Last parameter update.
        /// </summary>
        [Column("MODIFICATION")]
        [DataMember]
        public DateTime Update { get; set; }

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
        [MaxLength(20), Column("VALUE")]
        [DataMember]
        public string ValueProxy { get; set; }

    }
}
