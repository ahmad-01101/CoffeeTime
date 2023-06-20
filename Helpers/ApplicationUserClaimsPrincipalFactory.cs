using CoffeeTime.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
namespace CoffeeTime.Helpers
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim("Email", user.Email ?? ""));
            identity.AddClaim(new Claim("FirstName", user.FirstName ?? ""));
            //identity.AddClaim(new Claim("EmpId", user.EmployeeId.ToString() ?? ""));

            return identity;
        } 
    }
}
