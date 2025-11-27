using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DTOs;

namespace Interfaces.IManagers
{
    public interface IAuthManager
    {
        Task<Result<UserResponse>> RegisterUserAsync(Register dto);
        Task<Result<AuthResponse>> LoginUserAsync(Login dto);
        Task<Result> AssignRoleAsync(string userId, string role);
        Task<Result<UserResponse>> GetUserByIdAsync(string userId);

        Task<Result<IList<UserResponse>>> GetUsersInRoleAsync(string rolename);
        Task<Result<UserResponse>> UpdateUserAsync(string userId, UpdateUser dto);
        Task<Result> DeleteUserAsync(string userId);
        Task<Result> SignOut(string userId);

    }
}
