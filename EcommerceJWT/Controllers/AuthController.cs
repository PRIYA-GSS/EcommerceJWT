using Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Services;
namespace EcommerceJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] Register dto)
        {
            var result = await _service.RegisterUserAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] Login dto)
        {
            var result = await _service.LoginUserAsync(dto);
            return result.Success ? Ok(result) : Unauthorized(result);
        }
        [HttpPost("assign-role")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AssignRole(string userId,string role)
        {
            var result = await _service.AssignRoleAsync(userId, role);
            return result.Success ? Ok(result) : BadRequest(result);

        }
        [HttpGet("users-by-role/{role}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetUsersByRole(string role)
        {
            var result = await _service.GetUsersInRoleAsync(role);
            return result.Success ? Ok(result) : NotFound(result);
        }
        [HttpPut("{userId}")]
        [Authorize] 
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUser dto)
        {
            var result = await _service.UpdateUserAsync(userId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _service.DeleteUserAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost("signout/{userId}")]
        [Authorize] 
        public async Task<IActionResult> SignOut(string userId)
        {
            var result = await _service.SignOut(userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }
}
