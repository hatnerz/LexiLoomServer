using System.ComponentModel.DataAnnotations;

namespace LexiLoom.Models
{
    public class Language : BaseEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string? IsoCode { get; set; } = null!;
    }
}
