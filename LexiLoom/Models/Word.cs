using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LexiLoom.Models
{
    public class Word : BaseEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        public IEnumerable<Translation>? Translations { get; set; }

        [Required]
        public int UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [Required]
        public DateTime AddingTime { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public IEnumerable<WordInModule>? WordInModules { get; set; }
    }
}
