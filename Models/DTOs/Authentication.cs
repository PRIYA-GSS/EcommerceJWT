using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class Register
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        

    }
    public class Login
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; }
        public DateTime TokenExpiry { get; set; }
       
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
    public class TokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
