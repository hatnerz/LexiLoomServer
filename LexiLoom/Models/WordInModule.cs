using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LexiLoom.Models
{
    public class WordInModule : BaseEntity
    {
        [Required]
        public int WordId { get; set; }

        public Word? Word { get; set; }

        [Required]
        public int ModuleId { get; set; }

        [JsonIgnore]
        public Module? Module { get; set; }

        public DateTime AddingTime { get; set; } = DateTime.UtcNow;
    }
}
