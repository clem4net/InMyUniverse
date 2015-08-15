using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Z4Net.Dto.Services.Definitions
{
    /// <summary>
    /// Description a parameter of a product.
    /// </summary>
    [Table("ProductsParameters")]
    public class ProductParameterDto
    {

        /// <summary>
        /// Parameter properties.
        /// </summary>
        [Column("DATA")]
        public string Data { get; set; }

        /// <summary>
        /// Parameter default value.
        /// </summary>
        [Required, Column("DEFAULT_VALUE"), MaxLength(20)]
        public string DefaultValue { get; set; }

        /// <summary>
        /// Parameter description.
        /// </summary>
        [NotMapped]
        public string Description { get; set; }

        /// <summary>
        /// Identifier of description labels.
        /// </summary>
        [Column("DESC_ID")]
        public int DescriptionIdentifier { get; set; }

        /// <summary>
        /// Unique parameter identifier.
        /// </summary>
        [Key, Column("ID"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Identifier { get; set; }

        /// <summary>
        /// Product names.
        /// </summary>
        [NotMapped]
        public string Name { get; set; }

        /// <summary>
        /// Identifier of name labels.
        /// </summary>
        [Column("LABEL_ID")]
        public int NameLabelIdentifier { get; set; }

        /// <summary>
        /// Link to product.
        /// </summary>
        [Column("PRD_ID")]
        public int ProductIdentifier { get; set; }

        /// <summary>
        /// Product.
        /// </summary>
        [ForeignKey("ProductIdentifier")]
        public ProductDto Product { get; set; }

        /// <summary>
        /// Parameter type.
        /// </summary>
        [Column("TYPE")]
        public ParameterType Type { get; set; }

    }
}
