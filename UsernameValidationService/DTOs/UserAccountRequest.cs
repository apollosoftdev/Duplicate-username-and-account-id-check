namespace UsernameValidationService.DTOs
{
    public class UserAccountRequest
    {
        public Guid AccountId { get; set; }
        public string Username { get; set; } = string.Empty;
    }
} 