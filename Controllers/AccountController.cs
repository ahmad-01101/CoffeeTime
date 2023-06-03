using Microsoft.AspNetCore.Mvc;

namespace CoffeeTime.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        
        public IActionResult Signup()
        {
            return View();
        } 
    }
}
