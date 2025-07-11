using Microsoft.AspNetCore.Mvc;
using UsernameValidationService.DTOs;
using UsernameValidationService.Services;

namespace UsernameValidationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsernameController : ControllerBase
    {
        private readonly IUsernameValidationService _usernameValidationService;

        public UsernameController(IUsernameValidationService usernameValidationService)
        {
            _usernameValidationService = usernameValidationService;
        }

        /// <summary>
        /// Validates if a username meets the requirements
        /// </summary>
        /// <param name="username">The username to validate</param>
        /// <returns>Validation result</returns>
        [HttpGet("validate")]
        public async Task<ActionResult<UsernameValidationResponse>> ValidateUsername([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(new UsernameValidationResponse
                {
                    IsValid = false,
                    Message = "Username parameter is required",
                    Errors = new List<string> { "Username cannot be null or empty" }
                });
            }

            var result = await _usernameValidationService.ValidateUsernameAsync(username);
            return Ok(result);
        }

        /// <summary>
        /// Stores a username and account ID combination
        /// </summary>
        /// <param name="request">The user account request</param>
        /// <returns>Operation result</returns>
        [HttpPost("store")]
        public async Task<ActionResult<UserAccountResponse>> StoreUserAccount([FromBody] UserAccountRequest request)
        {
            if (request == null)
            {
                return BadRequest(new UserAccountResponse
                {
                    Success = false,
                    Message = "Request body is required",
                    Errors = new List<string> { "Request body cannot be null" }
                });
            }

            if (request.AccountId == Guid.Empty)
            {
                return BadRequest(new UserAccountResponse
                {
                    Success = false,
                    Message = "Invalid Account ID",
                    Errors = new List<string> { "Account ID cannot be empty" }
                });
            }

            var result = await _usernameValidationService.StoreUserAccountAsync(request.AccountId, request.Username);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Updates the username for an existing account
        /// </summary>
        /// <param name="request">The user account update request</param>
        /// <returns>Operation result</returns>
        [HttpPost("update")]
        public async Task<ActionResult<UserAccountResponse>> UpdateUsername([FromBody] UserAccountRequest request)
        {
            if (request == null)
            {
                return BadRequest(new UserAccountResponse
                {
                    Success = false,
                    Message = "Request body is required",
                    Errors = new List<string> { "Request body cannot be null" }
                });
            }

            if (request.AccountId == Guid.Empty)
            {
                return BadRequest(new UserAccountResponse
                {
                    Success = false,
                    Message = "Invalid Account ID",
                    Errors = new List<string> { "Account ID cannot be empty" }
                });
            }

            var result = await _usernameValidationService.UpdateUsernameAsync(request.AccountId, request.Username);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Checks if a username is available
        /// </summary>
        /// <param name="username">The username to check</param>
        /// <returns>Availability status</returns>
        [HttpGet("check-availability")]
        public async Task<ActionResult<object>> CheckUsernameAvailability([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(new { Available = false, Message = "Username parameter is required" });
            }

            var isAvailable = await _usernameValidationService.IsUsernameAvailableAsync(username);
            return Ok(new { Available = isAvailable, Username = username });
        }

        /// <summary>
        /// Checks if an account ID exists
        /// </summary>
        /// <param name="accountId">The account ID to check</param>
        /// <returns>Existence status</returns>
        [HttpGet("check-account")]
        public async Task<ActionResult<object>> CheckAccountExists([FromQuery] Guid accountId)
        {
            if (accountId == Guid.Empty)
            {
                return BadRequest(new { Exists = false, Message = "Valid Account ID is required" });
            }

            var exists = await _usernameValidationService.IsAccountIdExistsAsync(accountId);
            return Ok(new { Exists = exists, AccountId = accountId });
        }
    }
} 