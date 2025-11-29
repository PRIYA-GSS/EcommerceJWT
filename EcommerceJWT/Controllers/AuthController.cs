using Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Services;

using Entity = DataAccess.Entity;
using Microsoft.Extensions.Logging;
namespace EcommerceJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService service, ILogger<AuthController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] Register dto)
        {
            _logger.LogInformation("Register request recieved for {Email}", dto.Email);
            var result = await _service.RegisterUserAsync(dto);
            if (result.Success)
            {
                _logger.LogInformation("User {Email} registered successfully", dto.Email);
                Ok(result);
            }
            
                _logger.LogWarning("Registration failed for {Email}. Reason:{Message}", dto.Email, result.Message);

                return BadRequest(result);
            
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] Login dto)
        {

            _logger.LogInformation("Login attempt for {Email}", dto.Email);

            var result = await _service.LoginUserAsync(dto);

            if(result.Success)
            {
                _logger.LogInformation("Login successful for {Email}", dto.Email);
                return Ok(result);
            }

            _logger.LogWarning("Login failed for {Email}. Reason: {Message}", dto.Email, result.Message);
            return Unauthorized(result);

        }
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody]string refreshToken)
        {
           //var result= await _service.GetNewTokenAsync(refreshToken);

            _logger.LogInformation("Refresh token request received");

            var result = await _service.GetNewTokenAsync(refreshToken);

            if (result.Success)
            {
                _logger.LogInformation("New token issued successfully for refresh token");
                return Ok(result);
            }

            _logger.LogWarning("Invalid refresh token provided");
            return Unauthorized(result);


        }
        [HttpPost("assign-role")]
        [AllowAnonymous]
        public async Task<IActionResult> AssignRole(string username,string role)
        {
            

            _logger.LogInformation("Assign role request: {Role} to user {Username}", role, username);

            var result = await _service.AssignRoleAsync(username, role);

            if (result.Success)
            {
                _logger.LogInformation("Role {Role} assigned to user {Username}", role, username);
                return Ok(result);

            }

            _logger.LogWarning("Failed to assign role {Role} to user {Username}. Reason: {Message}", role, username, result.Message);
            return BadRequest(result);


        }
        [HttpGet("users-by-role/{role}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetUsersByRole(string role)
        {

            _logger.LogInformation("Fetching users with role {Role}", role);

            var result = await _service.GetUsersInRoleAsync(role);

            if (result.Success)
            {
                _logger.LogInformation("Fetched {Count} users with role {Role}", result.Data.Count, role);
                return Ok(result);
            }

            _logger.LogWarning("No users found with role {Role}", role);
            return NotFound(result);

        }
        [HttpPut("{userId}")]
        [Authorize] 
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUser dto)
        {

            _logger.LogInformation("Update request for user {UserId}", userId);

            var result = await _service.UpdateUserAsync(userId, dto);

            if (result.Success)
            {
                _logger.LogInformation("User {UserId} updated successfully", userId);
                return Ok(result);
            }

            _logger.LogWarning("Failed to update user {UserId}. Reason: {Message}", userId, result.Message);
            return BadRequest(result);

        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> DeleteUser(string userId)
        {

            _logger.LogInformation("Delete request for user {UserId}", userId);

            var result = await _service.DeleteUserAsync(userId);

            if (result.Success)
            {
                _logger.LogInformation("User {UserId} deleted successfully", userId);
                return Ok(result);
            }

            _logger.LogWarning("Failed to delete user {UserId}. Reason: {Message}", userId, result.Message);
            return NotFound(result);
        }

        [HttpPost("signout/{userId}")]
        [Authorize] 
        public async Task<IActionResult> SignOut(string userId)
        {

            _logger.LogInformation("Sign out request for user {UserId}", userId);

            var result = await _service.SignOut(userId);

            if (result.Success)
            {
                _logger.LogInformation("User {UserId} signed out successfully", userId);
                return Ok(result);
            }

            _logger.LogWarning("Failed to sign out user {UserId}. Reason: {Message}", userId, result.Message);
            return BadRequest(result);

        }

    }
}
