using CoffeeTime.Models;
using CoffeeTime.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeTime.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Signup(bool isSuccess = false)
        {
            ViewBag.IsSuccess = isSuccess;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignUpUser signUpUser)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.CreateUser(signUpUser);
                if (!result.Succeeded)
                {
                    foreach(var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }
                    return View(signUpUser);
                }
                ModelState.Clear();
                return RedirectToAction(nameof(Signup), new { isSuccess = true });
            }
            return View();
        }

    }
}
