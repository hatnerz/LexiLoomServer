using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LexiLoom.Models
{
    public class Module : BaseEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        public string? Descriptions { get; set; }

        public IEnumerable<WordInModule>? Words { get; set; }

        [Required]
        public int UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
    }
}