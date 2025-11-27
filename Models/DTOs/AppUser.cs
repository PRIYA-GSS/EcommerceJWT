using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class AppUser
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

    }
    public class UserResponse
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
      
        public string UserName { get; set; }
        public string Email { get; set; }
        public IList<string>? Roles { get; set; } = new List<string>();

        
    }
    public class UpdateUser
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
