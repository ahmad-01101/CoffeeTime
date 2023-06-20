using CoffeeTime.Models;
using Microsoft.AspNetCore.Identity;

namespace CoffeeTime.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUser(SignUpUser signUpUser);
        Task<SignInResult> PasswordSignInAsync(SignInUser signInUser);
        Task SignOutAsync();

    }
}