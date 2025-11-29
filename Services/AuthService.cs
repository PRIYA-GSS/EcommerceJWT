using Interfaces.IManagers;
using Interfaces.IServices;
using Models.DTOs;
namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthManager _authmanager;
        public AuthService(IAuthManager authmanager)
        {
            _authmanager = authmanager;
        }
        public async Task<Result<UserResponse>> RegisterUserAsync(Register dto) => await _authmanager.RegisterUserAsync(dto);
        public async Task<Result<AuthResponse>> LoginUserAsync(Login dto) => await _authmanager.LoginUserAsync(dto);
        public async Task<Result<TokenResponse>> GetNewTokenAsync(string refreshToken)=> await _authmanager.GetNewTokenAsync(refreshToken);
   
        public async Task<Result> AssignRoleAsync(string username, string role) => await _authmanager.AssignRoleAsync(username, role);
        public async Task<Result<UserResponse>> GetUserByIdAsync(string userId) => await _authmanager.GetUserByIdAsync(userId);


        public async Task<Result<IList<UserResponse>>> GetUsersInRoleAsync(string rolename) => await _authmanager.GetUsersInRoleAsync(rolename);
        public async Task<Result<UserResponse>> UpdateUserAsync(string userId, UpdateUser dto) => await _authmanager.UpdateUserAsync(userId, dto);
        public async Task<Result> DeleteUserAsync(string userId) => await _authmanager.DeleteUserAsync(userId);
        public async Task<Result> SignOut(string userId) => await _authmanager.SignOut(userId);
    }
}
