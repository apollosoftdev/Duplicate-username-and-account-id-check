using System.ComponentModel.DataAnnotations;

namespace UsernameValidationService.Models
{
    public class UserAccount
    {
        [Key]
        public Guid AccountId { get; set; }
        
        [Required]
        [StringLength(30, MinimumLength = 6)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username must contain only alphanumeric characters")]
        public string Username { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
    }
} 