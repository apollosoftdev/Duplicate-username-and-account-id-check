using Microsoft.EntityFrameworkCore;
using UsernameValidationService.Data;
using UsernameValidationService.DTOs;
using UsernameValidationService.Models;
using System.ComponentModel.DataAnnotations;

namespace UsernameValidationService.Services
{
    public class UsernameValidationService : IUsernameValidationService
    {
        private readonly ApplicationDbContext _context;

        public UsernameValidationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UsernameValidationResponse> ValidateUsernameAsync(string username)
        {
            var response = new UsernameValidationResponse();
            var errors = new List<string>();

            // Check if username is null or empty
            if (string.IsNullOrWhiteSpace(username))
            {
                errors.Add("Username cannot be null or empty");
                response.IsValid = false;
                response.Errors = errors;
                response.Message = "Username validation failed";
                return response;
            }

            // Check length (6-30 characters)
            if (username.Length < 6 || username.Length > 30)
            {
                errors.Add("Username must be between 6 and 30 characters");
            }

            // Check alphanumeric only
            if (!System.Text.RegularExpressions.Regex.IsMatch(username, @"^[a-zA-Z0-9]+$"))
            {
                errors.Add("Username must contain only alphanumeric characters");
            }

            // Check if username is already taken
            var isAvailable = await IsUsernameAvailableAsync(username);
            if (!isAvailable)
            {
                errors.Add("Username is already taken");
            }

            response.IsValid = errors.Count == 0;
            response.Errors = errors;
            response.Message = response.IsValid ? "Username is valid" : "Username validation failed";

            return response;
        }

        public async Task<UserAccountResponse> StoreUserAccountAsync(Guid accountId, string username)
        {
            var response = new UserAccountResponse();

            try
            {
                // Validate username first
                var validationResponse = await ValidateUsernameAsync(username);
                if (!validationResponse.IsValid)
                {
                    response.Success = false;
                    response.Message = "Username validation failed";
                    response.Errors = validationResponse.Errors;
                    return response;
                }

                // Check if account ID already exists
                var existingAccount = await _context.UserAccounts
                    .FirstOrDefaultAsync(u => u.AccountId == accountId);

                if (existingAccount != null)
                {
                    response.Success = false;
                    response.Message = "Account ID already exists";
                    response.Errors.Add("Account ID is already registered");
                    return response;
                }

                // Create new user account
                var userAccount = new UserAccount
                {
                    AccountId = accountId,
                    Username = username,
                    CreatedAt = DateTime.UtcNow
                };

                _context.UserAccounts.Add(userAccount);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = "User account created successfully";
                response.AccountId = accountId;
                response.Username = username;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error creating user account";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public async Task<UserAccountResponse> UpdateUsernameAsync(Guid accountId, string newUsername)
        {
            var response = new UserAccountResponse();

            try
            {
                // Validate new username
                var validationResponse = await ValidateUsernameAsync(newUsername);
                if (!validationResponse.IsValid)
                {
                    response.Success = false;
                    response.Message = "Username validation failed";
                    response.Errors = validationResponse.Errors;
                    return response;
                }

                // Find existing account
                var existingAccount = await _context.UserAccounts
                    .FirstOrDefaultAsync(u => u.AccountId == accountId);

                if (existingAccount == null)
                {
                    response.Success = false;
                    response.Message = "Account not found";
                    response.Errors.Add("Account ID does not exist");
                    return response;
                }

                // Update username
                existingAccount.Username = newUsername;
                existingAccount.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = "Username updated successfully";
                response.AccountId = accountId;
                response.Username = newUsername;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error updating username";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public async Task<bool> IsUsernameAvailableAsync(string username)
        {
            return !await _context.UserAccounts
                .AnyAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<bool> IsAccountIdExistsAsync(Guid accountId)
        {
            return await _context.UserAccounts
                .AnyAsync(u => u.AccountId == accountId);
        }
    }
} 