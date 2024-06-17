using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace LexiLoom.Models
{
    public class User : BaseEntity
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public IEnumerable<Module>? Modules { get; set; }
    
        public IEnumerable<Word>? Words { get; set; }
    }
}
