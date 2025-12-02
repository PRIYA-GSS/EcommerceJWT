using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using DataAccess.Entity;
namespace Services
{
    //public class AppClaimsFactory : UserClaimsPrincipalFactory<AppUser, IdentityRole>
    ////{
    ////    private readonly UserManager<AppUser> _usermanager;
       
    ////    public AppClaimsFactory(
    ////        UserManager<AppUser> userManager,
    ////        RoleManager<IdentityRole> roleManager,
    ////        IOptions<IdentityOptions> options)
    ////        : base(userManager, roleManager, options) 
    ////    {
    ////        _usermanager = userManager;

    ////    }

    ////    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
    ////    {
    ////        var identity = await base.GenerateClaimsAsync(user);
    ////        var roles = await _usermanager.GetRolesAsync(user);

    ////        foreach (var role in roles)
    ////        {
    ////            identity.AddClaim(new Claim("role", role));
    ////        }

    ////        return identity;
    ////    }
    ////}
}
