using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Z4Net.Dto.Services.Definitions;

namespace Z4Net.Dto.Services
{
    /// <summary>
    /// Node parameter.
    /// </summary>
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
        public int Identifier { get; set; }

        /// <summary>
        /// Node identifier.
        /// </summary>
        [Column("NODE_ID")]
        public int NodeIdentifier { get; set; }

        /// <summary>
        /// Parameter node.
        /// </summary>
        [ForeignKey("NodeIdentifier")]
        public virtual NodeDto Node { get; set; }

        /// <summary>
        /// Last parameter update.
        /// </summary>
        [Column("MODIFICATION")]
        public DateTime Update { get; set; }

        /// <summary>
        /// Parameter value.
        /// </summary>
        [Column("VALUE"), MaxLength(20)]
        public string Value { get; set; }

    }
}
