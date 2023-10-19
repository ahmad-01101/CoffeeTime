using CoffeeTime.Models;
using Microsoft.AspNetCore.Identity;

namespace CoffeeTime.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        public async Task<SignInResult> PasswordSignInAsync(SignInUser signInUser)
        {
            return await _signInManager.PasswordSignInAsync(signInUser.Email, signInUser.Password, signInUser.RememberMe, false);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}