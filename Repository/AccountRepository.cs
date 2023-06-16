using CoffeeTime.Models;
using Microsoft.AspNetCore.Identity;

namespace CoffeeTime.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IdentityResult> CreateUser(SignUpUser signUpUser)
        {
            var user = new ApplicationUser()
            {
                FirstName = signUpUser.FirstName,
                LastName = signUpUser.LastName,
                UserName = signUpUser.Email,
                Email = signUpUser.Email,
                PhoneNumber = signUpUser.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, signUpUser.Password);
            return result;
        }
    }
}
