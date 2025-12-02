using System.Data;
using AutoMapper;
using Interfaces.IManagers;
using Interfaces.IRepository;
using Microsoft.AspNetCore.Identity;
using Models.Constants;
using Models.DTOs;
using Models.TokenHelper;
using Dto = Models.DTOs;
using Entity = DataAccess.Entity;
using Duende.IdentityModel.Client;
using System.Net.Http;

namespace Managers
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<Entity.AppUser> _usermanager;
        private readonly SignInManager<Entity.AppUser> _signInmanager;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly IMapper _mapper;
        private readonly TokenHelper _token;
        private readonly IBaseRepository<Entity.RefreshToken> _refreshRepo;
        public AuthManager(UserManager<Entity.AppUser> usermanager, SignInManager<Entity.AppUser> signInmanager, RoleManager<IdentityRole> rolemanager, IMapper mapper, TokenHelper token, IBaseRepository<Entity.RefreshToken> refreshRepo)
        {
            _usermanager = usermanager;
            _signInmanager = signInmanager;
            _rolemanager = rolemanager;
            _mapper = mapper;
            _token = token;
            _refreshRepo = refreshRepo;
        }
        public async Task<Dto.Result<Dto.UserResponse>> RegisterUserAsync(Dto.Register dto)
        {
            try
            {
                var exist = await _usermanager.FindByEmailAsync(dto.Email);
                if (exist != null)
                {
                    return new Dto.Result<Dto.UserResponse>
                    {
                        Success = false,
                        Message = ErrorConstants.Exists,
                        Data = null
                    };
                }
                var user = new Entity.AppUser
                {
                    FullName = dto.FullName,
                    UserName = dto.UserName,
                    Email = dto.Email

                };

                var result = await _usermanager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                    return new Dto.Result<Dto.UserResponse> { Success = false, Message = ErrorConstants.RegistrationFailed, Data = null };

                var roles = await _usermanager.GetRolesAsync(user);
                var userresponse = new Dto.UserResponse
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = roles.ToList()
                };


                return new Dto.Result<Dto.UserResponse>
                {
                    Success = true,
                    Message = "Registration Successfull",
                    Data = userresponse
                };
            }
            catch (Exception ex)
            {
                return new Dto.Result<Dto.UserResponse>
                {
                    Success = false,
                    Message = ErrorConstants.Internal

                };
            }
        }
        //public async Task<Result<AuthResponse>> LoginUserAsync(Login dto)
        //{
        //    try
        //    {
        //        var user = await _usermanager.FindByEmailAsync(dto.Email);
        //        if (user == null)
        //            return new Result<AuthResponse> { Success = false, Message = ErrorConstants.NotFound };

        //        var signInResult = await _signInmanager.PasswordSignInAsync(user, dto.Password, false, false);
        //        if (!signInResult.Succeeded)
        //            return new Result<AuthResponse> { Success = false, Message = ErrorConstants.LoginFailed };


        //        //token using identity server


        //        // Request token from IdentityServer
        //        using var client = new HttpClient();
        //        var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001"); // IdentityServer URL
        //        if (disco.IsError) throw new Exception(disco.Error);

        //        var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
        //        {
        //            Address = disco.TokenEndpoint,
        //            ClientId = "myClient",
        //            ClientSecret = "secret",
        //            UserName = dto.Email,
        //            Password = dto.Password,
        //            Scope = "myApi"
        //        });

        //        if (tokenResponse.IsError)
        //            return new Result<AuthResponse> { Success = false, Message = tokenResponse.Error };

        //        var roles = await _usermanager.GetRolesAsync(user);
        //        var role = roles.FirstOrDefault() ?? "User";
        //        var roleList = roles.ToList();
        //        var userdto = _mapper.Map<Dto.AppUser>(user);
        //        //var token = _token.GenerateToken(userdto, roleList);

        //        var response = new AuthResponse
        //        {
        //            Role = role,
        //            Token = tokenResponse.AccessToken,
        //            RefreshToken = tokenResponse.RefreshToken,
        //            TokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn),
        //            Username = user.UserName
        //        };


        //        //USING JWT SELF TOKEN GENERATION

        //        //var refreshToken = _token.GenerateRefreshToken();
        //        //var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        //        //var refreshEntity = new Entity.RefreshToken { Token = refreshToken, UserId = user.Id, ExpiryDate = refreshTokenExpiry };
        //        //await _refreshRepo.AddAsync(refreshEntity);

        //        //var response = new AuthResponse
        //        //{
        //        //    Role = role,
        //        //    Token = token,
        //        //    RefreshToken=refreshToken,
        //        //    TokenExpiry= DateTime.UtcNow.AddMinutes(2),
        //        //    Username = user.UserName
        //        //};
        //        return new Result<AuthResponse>
        //        {
        //            Success = true,
        //            Message = "Login Successful",
        //            Data = response
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result<AuthResponse>
        //        {
        //            Success = false,
        //            Message = ErrorConstants.Internal

        //        };
        //    }
        //}
  
    //   public async Task<Result<AuthResponse>> LoginUserAsync(Login dto)
    //{
    //    try
    //    {
    //        // Find user by email
    //        var user = await _usermanager.FindByEmailAsync(dto.Email);
    //        if (user == null)
    //        {
    //            return new Result<AuthResponse>
    //            {
    //                Success = false,
    //                Message = ErrorConstants.NotFound
    //            };
    //        }

    //        // Check password
    //        var signInResult = await _signInmanager.PasswordSignInAsync(user, dto.Password, false, false);
    //        if (!signInResult.Succeeded)
    //        {
    //            return new Result<AuthResponse>
    //            {
    //                Success = false,
    //                Message = ErrorConstants.LoginFailed
    //            };
    //        }

    //        // Request token from IdentityServer
    //        using var client = new HttpClient();

    //        var disco = await client.GetDiscoveryDocumentAsync("https://localhost:7141"); // IdentityServer URL
    //        if (disco.IsError)
    //        {
    //            return new Result<AuthResponse>
    //            {
    //                Success = false,
    //                Message = $"Discovery error: {disco.Error}"
    //            };
    //        }

    //        var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
    //        {
    //            Address = disco.TokenEndpoint,
    //            ClientId = "myClient",
    //            ClientSecret = "secret",
    //            UserName = dto.UserName,
    //            Password = dto.Password,
               
    //            Scope = "myApi openid profile offline_access roles"
    //        });

    //        if (tokenResponse.IsError)
    //        {
    //            return new Result<AuthResponse>
    //            {
    //                Success = false,
    //                Message = $"Token error: {tokenResponse.Error}"
    //            };
    //        }

    //        // Get user roles
    //        var roles = await _usermanager.GetRolesAsync(user);
    //        var role = roles.FirstOrDefault() ?? "User";

    //        var response = new AuthResponse
    //        {
    //            Role = role,
    //            Token = tokenResponse.AccessToken,
    //            RefreshToken = tokenResponse.RefreshToken,
    //            TokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn),
    //            Username = user.UserName
    //        };

    //        return new Result<AuthResponse>
    //        {
    //            Success = true,
    //            Message = "Login Successful",
    //            Data = response
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //            return new Result<AuthResponse>
    //            {
    //                Success = false,
    //                Message = ex.Message
    //            };
    //    }
    //}
    //public async Task<Result<Dto.TokenResponse>> GetNewTokenAsync(string refreshToken)
    //    {

    //        var refreshTokens = await _refreshRepo.GetAllAsync();
    //        var stored = refreshTokens.FirstOrDefault(t => t.Token == refreshToken);
    //        if (stored == null || stored.IsRevoked || stored.ExpiryDate < DateTime.UtcNow)
    //            return new Result<Dto.TokenResponse> { Success = false, Message = ErrorConstants.InvalidToken };

    //        var user = await _usermanager.FindByIdAsync(stored.UserId);
    //        var roles= await _usermanager.GetRolesAsync(user);
    //        var userdto = _mapper.Map<AppUser>(user);
    //        var newAccess = _token.GenerateToken(userdto, roles);
    //        var newrefreshToken = _token.GenerateRefreshToken();
    //        stored.IsRevoked = true;
    //        await _refreshRepo.UpdateAsync(stored);
    //        await _refreshRepo.AddAsync(new Entity.RefreshToken
    //        {
    //            Token = newrefreshToken,
    //            UserId = user.Id,
    //            ExpiryDate = DateTime.UtcNow.AddDays(7)
    //        });
    //        return new Result<Dto.TokenResponse>
    //        {
    //            Success = true,
    //            Message = "Your new token",
    //            Data = new Dto.TokenResponse
    //            {
    //                Token = newAccess,
    //                RefreshToken = newrefreshToken,
    //            }
    //        };

    //    }
 

//public async Task<Result<Dto.TokenResponse>> GetNewTokenAsync(string refreshToken)
//    {
//        try
//        {
//            using var client = new HttpClient();

//            // Discover endpoints from IdentityServer
//            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:7141");
//            if (disco.IsError)
//            {
//                return new Result<Dto.TokenResponse>
//                {
//                    Success = false,
//                    Message = $"Discovery error: {disco.Error}"
//                };
//            }

//            // Request new tokens using the refresh token
//            var tokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
//            {
//                Address = disco.TokenEndpoint,
//                ClientId = "myClient",
//                ClientSecret = "secret",
//                RefreshToken = refreshToken
//            });

//            if (tokenResponse.IsError)
//            {
//                return new Result<Dto.TokenResponse>
//                {
//                    Success = false,
//                    Message = $"Token error: {tokenResponse.Error}"
//                };
//            }

//            return new Result<Dto.TokenResponse>
//            {
//                Success = true,
//                Message = "Your new token",
//                Data = new Dto.TokenResponse
//                {
//                    Token = tokenResponse.AccessToken,
//                    RefreshToken = tokenResponse.RefreshToken,
//                    //ExpiresIn = tokenResponse.ExpiresIn
//                }
//            };
//        }
//        catch (Exception ex)
//        {
//            return new Result<Dto.TokenResponse>
//            {
//                Success = false,
//                Message = ex.Message
//            };
//        }
//    }
    public async Task<Result> AssignRoleAsync(string Username, string role)
        {
            try
            {
                var user = await _usermanager.FindByNameAsync(Username);
                if (user == null)
                    return new Result { Success = false, Message = ErrorConstants.InValid };
                if (!await _rolemanager.RoleExistsAsync(role))
                    await _rolemanager.CreateAsync(new IdentityRole(role));

                var result = await _usermanager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                    return new Result { Success = false, Message = ErrorConstants.Failed };

                return new Result { Success = true, Message = "Role assigned to the user" };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = ErrorConstants.Internal
                };

            }
        }
        public async Task<Result<UserResponse>> GetUserByIdAsync(string userId)
        {
            try
            {
                var user = await _usermanager.FindByIdAsync(userId);
                if (user == null)
                    return new Result<UserResponse> { Success = false, Message = ErrorConstants.NotFound };
                var userresponse = _mapper.Map<UserResponse>(user);
                return new Result<UserResponse>
                {
                    Success = true,
                    Message = "User retrieved",
                    Data = userresponse
                };
            }
            catch (Exception ex)
            {
                return new Result<UserResponse>
                {
                    Success = false,
                    Message = ErrorConstants.Internal
                };
            }
        }
        public async Task<Result<IList<UserResponse>>> GetUsersInRoleAsync(string rolename)
        {
            try
            {
                var exist = await _rolemanager.RoleExistsAsync(rolename);
                if (!exist) return new Result<IList<UserResponse>> { Success = false, Message = ErrorConstants.RoleNotFound };
                var users = await _usermanager.GetUsersInRoleAsync(rolename);
                var response = users.Select(u => _mapper.Map<UserResponse>(u)).ToList();
                return new Result<IList<UserResponse>> { Success = true, Message = "Users Found", Data = response };
            }
            catch (Exception ex)
            {
                return new Result<IList<UserResponse>>
                {
                    Success = false,
                    Message = ErrorConstants.Internal
                };
            }

        }
        public async Task<Result<UserResponse>> UpdateUserAsync(string userId, UpdateUser dto)
        {
            try
            {
                var user = await _usermanager.FindByIdAsync(userId);
                if (user == null) return new Result<UserResponse> { Success = false, Message = ErrorConstants.NotFound };
                user.Email = !string.IsNullOrEmpty(dto.Email) ? dto.Email : user.Email;
                user.FullName = !string.IsNullOrEmpty(dto.FullName) ? dto.FullName : user.FullName;
                user.UserName = !string.IsNullOrEmpty(dto.UserName) ? dto.UserName : user.UserName;
                var result = await _usermanager.UpdateAsync(user);
                return new Result<UserResponse> { Success = true, Message = "user updated successfully", Data = _mapper.Map<UserResponse>(user) };
            }
            catch
            {
                return new Result<UserResponse>
                {
                    Success = false,
                    Message = ErrorConstants.Internal
                };

            }
        }
        public async Task<Result> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await _usermanager.FindByIdAsync(userId);
                if (user == null) return new Result { Success = false, Message = ErrorConstants.NotFound };
                var result = await _usermanager.DeleteAsync(user);
                return new Result { Success = true, Message = "User Deleted" };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = ErrorConstants.Internal
                };
            }
        }
        public async Task<Result> SignOut(string userId)
        {
            try
            {
                var user = await _usermanager.FindByIdAsync(userId);
                if (user == null) return new Result { Success = false, Message = ErrorConstants.NotFound };
                await _signInmanager.SignOutAsync();
                return new Result { Success = true, Message = "Signed out successfully" };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = ErrorConstants.Internal
                };
            }
        }

    }
}
