namespace UsernameValidationService.DTOs
{
    public class UserAccountResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? AccountId { get; set; }
        public string? Username { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
} 