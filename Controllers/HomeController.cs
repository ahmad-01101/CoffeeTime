using CoffeeTime.Data;
using CoffeeTime.Models;
using CoffeeTime.Models.Domain;
using CoffeeTime.Repository;
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

        public async Task<IActionResult> Index()
        {
            var Item = await coffeeTimeDbContext.MenuList.ToListAsync();
            return View(Item);
        }

        [Route("Add-Item/{Id}", Name = "AddItem")]
        public async Task<IActionResult> AddCartItem(int Id)
        {
            //var CheckItem = await coffeeTimeDbContext.Products.FirstOrDefaultAsync(x => x.ItemStatus == menuList.ItemName+"open");
            //var CheckItem = await coffeeTimeDbContext.Products.ToListAsync();
            ////var name = "Americano";
            //var ItemStatus = ;
            _ = await _userRepository.AddItem1(Id);
            return RedirectToAction("Index");
        }   
        
        public async Task<IActionResult> ViewCart()
        {
            var CartItems = await _userRepository.ViewCart();
            if (CartItems != null)
            {

            }
            return View(CartItems);
        }

        public async Task<IActionResult> AddCartItem1(CartListVm cartListVm)
        {
            //var name = "Americano";
            var ItemStatus = "AmericanoOpen";
            _ = await _userRepository.AddItem(cartListVm, ItemStatus);
            return RedirectToAction("Index");
        } 

        public async Task<IActionResult> AddCartItem2(CartListVm cartListVm)
        {
            //var name = "Espresso";
            var ItemStatus = "EspressoOpen";
            _ = await _userRepository.AddItem(cartListVm, ItemStatus);
            return RedirectToAction("Index");
        }
    }
}