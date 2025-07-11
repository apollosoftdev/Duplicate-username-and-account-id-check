using UsernameValidationService.DTOs;

namespace UsernameValidationService.Services
{
    public interface IUsernameValidationService
    {
        Task<UsernameValidationResponse> ValidateUsernameAsync(string username);
        Task<UserAccountResponse> StoreUserAccountAsync(Guid accountId, string username);
        Task<UserAccountResponse> UpdateUsernameAsync(Guid accountId, string newUsername);
        Task<bool> IsUsernameAvailableAsync(string username);
        Task<bool> IsAccountIdExistsAsync(Guid accountId);
    }
} 