using CoffeeTime.Data;
using CoffeeTime.Models;
using CoffeeTime.Models.Domain;
using CoffeeTime.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeTime.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CoffeeTimeDbContext coffeeTimeDbContext;
        private readonly UserRepository _userRepository;
        public HomeController(ILogger<HomeController> logger, CoffeeTimeDbContext coffeeTimeDbContext, UserRepository userRepository)
        {
            this.coffeeTimeDbContext = coffeeTimeDbContext;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Home()
        {
            var Item = await coffeeTimeDbContext.MenuList.ToListAsync();
            return View(Item);
        }

        [Authorize]
        [Route("Add-Item/{Id}", Name = "AddItem")]
        public async Task<IActionResult> AddCartItem(int Id)
        {
            _ = await _userRepository.AddItem4(Id);
            return RedirectToAction("Home");
        }   
        
        [Authorize]
        [Route("Increase-Quantity/{Id}", Name = "Increase")]
        public async Task<IActionResult> IncreaseQuantity(int Id)
        {
            _ = await _userRepository.IncreaseQuantity(Id);
            return RedirectToAction("ViewCart");
        } 
        
        [Authorize]
        [Route("Decrease-Quantity/{Id}", Name = "Decrease")]
        public async Task<IActionResult> DecreaseQuantity(int Id)
        {
            _ = await _userRepository.DecreaseQuantity(Id);
            return RedirectToAction("ViewCart");
        }    
        
        [Authorize]
        [Route("Delete-Item/{Id}", Name = "DeleteItem")]
        public async Task<IActionResult> DeleteItem(int Id)
        {
            _ = await _userRepository.DeleteItem(Id);
            return RedirectToAction("ViewCart");
        }    
        
        public async Task<IActionResult> ViewCart()
        {
            var CartItems = await _userRepository.ViewCart1();
            if (CartItems != null)
            {

            }
            return View(CartItems);
        }
    }
}