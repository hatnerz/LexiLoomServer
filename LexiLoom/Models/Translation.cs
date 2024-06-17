using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LexiLoom.Models
{
    public class Translation : BaseEntity
    {
        [Required]
        public int WordId { get; set; }

        [JsonIgnore]
        public Word? Word { get; set; }

        [Required]
        public int LanguageId { get; set; }
        
        public Language? Language { get; set; }

        [Required]
        public string TranslationText { get; set; } = null!;
    }
}
