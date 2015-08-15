using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Z4Net.Dto.Services.Definitions
{
    /// <summary>
    /// Product definition.
    /// </summary>
    [Table("Products")]
    public class ProductDto
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        [ForeignKey("ConstructorIdentifier")]
        public virtual ConstructorDto Constructor { get; set; }

        /// <summary>
        /// Constructor identifier.
        /// </summary>
        [Column("CST_ID"), MaxLength(4)]
        public string ConstructorIdentifier { get; set; }

        /// <summary>
        /// Unique parameter identifier.
        /// </summary>
        [Key, Column("ID"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Identifier { get; set; }

        /// <summary>
        /// Product name.
        /// </summary>
        [NotMapped]
        public string Name { get; set; }

        /// <summary>
        /// Product name label identifier.
        /// </summary>
        [Column("NAME_ID")]
        public int NameIdentifier { get; set; }

        /// <summary>
        /// Product parameters.
        /// </summary>
        public virtual ICollection<ProductParameterDto> Parameters { get; set; }

        /// <summary>
        /// Constructor product identifier.
        /// </summary>
        [Column("PRD_ID"), MaxLength(4)]
        public string ProductIdentifier { get; set; }

    }
}
