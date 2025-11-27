using Microsoft.AspNetCore.Identity;
namespace DataAccess.Entity
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public IList<Order> Orders { get; set; } = new List<Order>();
    }
}
