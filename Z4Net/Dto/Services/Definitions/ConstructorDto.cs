using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Z4Net.Dto.Services.Definitions
{
    /// <summary>
    /// Product constructor.
    /// </summary>
    [Table("Constructors")]
    public class ConstructorDto
    {

        /// <summary>
        /// Unique parameter identifier.
        /// </summary>
        [Key, Column("ID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), MaxLength(4)]
        public string Identifier { get; set; }

        /// <summary>
        /// Constructor name.
        /// </summary>
        [Column("NAME"), MaxLength(50)]
        public string Name { get; set; }

    }
}
