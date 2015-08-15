using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Z4Net.Dto.Services.Definitions
{
    /// <summary>
    /// Represent a text for a specified language.
    /// </summary>
    [Table("Labels")]
    public class LabelDto
    {

        /// <summary>
        /// Unique identifier.
        /// </summary>
        [Key, Column("ID"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Identifier { get; set; }

        /// <summary>
        /// Label identifier.
        /// </summary>
        [Column("LABEL_ID")]
        public int LabelIdentifier { get; set; }

        /// <summary>
        /// Label language.
        /// </summary>
        [Column("LANG_ID"), MaxLength(5)]
        public string Language { get; set; }

        /// <summary>
        /// Label.
        /// </summary>
        [Column("LABEL")]
        public string Value { get; set; }

    }
}
