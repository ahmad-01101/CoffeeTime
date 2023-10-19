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
        private readonly UserRepository _userRepository;
        public HomeController(ILogger<HomeController> logger, UserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Home(bool isSuccess = false)
        {
            ViewBag.IsSuccess = isSuccess;
            var MenuItems = await _userRepository.MenuItems();
            return View(MenuItems);
        }

        [Authorize]
        [Route("Add-Item/{Id}", Name = "AddItem")]
        public async Task<IActionResult> AddCartItem(string Id)
        {
            _ = await _userRepository.AddItem(Id);

            return RedirectToAction(nameof(Home), new { isSuccess = true });
        }
        
        [Authorize]
        [Route("Increase-Quantity/{Id}", Name = "Increase")]
        public async Task<IActionResult> IncreaseQuantity(string Id)
        {
            _ = await _userRepository.IncreaseQuantity(Id);
            return RedirectToAction("ViewCart");
        } 
        
        [Authorize]
        [Route("Decrease-Quantity/{Id}", Name = "Decrease")]
        public async Task<IActionResult> DecreaseQuantity(string Id)
        {
            _ = await _userRepository.DecreaseQuantity(Id);
            return RedirectToAction("ViewCart");
        }    
        
        [Authorize]
        [Route("Delete-Item/{Id}", Name = "DeleteItem")]
        public async Task<IActionResult> DeleteItem(string Id)
        {
            _ = await _userRepository.DeleteItem(Id);
            return RedirectToAction("ViewCart");
        }

        [Authorize]
        public async Task<IActionResult> ViewCart()
        {
            var CartItems = await _userRepository.ViewCart();
            if (CartItems != null)
            {

            }
            ViewBag.Total= CartItems.Sum(n => n.Price);
            return View(CartItems);
        }

        [Authorize]
        public async Task<IActionResult> Checkout(bool isSuccess = false)
        {
            var CartItems = await _userRepository.ViewCart();
            if (CartItems != null)
            {

            }
            ViewBag.Total= CartItems.Sum(n => n.Price);
            //ViewBag.CartId = CartItems.FirstOrDefault().OrderId;
            ViewBag.CartId = CartItems.FirstOrDefault().Order.OrderId;
            ViewBag.IsSuccess = isSuccess;
            return View(CartItems);
        }
        
        [Authorize]
        public async Task<IActionResult> NewOrderSuccessed(string Id)
        {
            var OrderDetail = await _userRepository.Invoice(Id);
            string NewOrder = await _userRepository.CheckIsSuccess(Id);
            if (NewOrder != null)
            {
                ViewBag.IsSuccess = true;
            }
            else
            {
                ViewBag.IsSuccess = false;
            }
            return View(OrderDetail);
        }

        [Authorize]
        [Route("New-Order/{Id}", Name = "NewOrder")]
        public async Task<IActionResult> NewOrder(string Id)
        {
            string NewOrder = await _userRepository.NewOrder(Id);
            if (NewOrder == Id)
            {
                return RedirectToAction(nameof(NewOrderSuccessed), new { Id });
            }
            return RedirectToAction(nameof(NewOrderSuccessed), new { Id });
        }
        
        [Authorize]
        [Route("NewReq/{Id}", Name = "NewReq")]
        public async Task<IActionResult> NewReq(string Id)
        {
            string NewOrder = await _userRepository.NewOrder(Id);
            if (NewOrder == Id)
            {
                return RedirectToAction(nameof(NewOrderSuccessed), new { Id });
            }
            return RedirectToAction(nameof(NewOrderSuccessed), new { Id });
        }

        [Authorize]
        public async Task<IActionResult> OrderHistory()
        {
            var CartItems = await _userRepository.OrderHistory();
            return View(CartItems);
        }

        [Authorize]
        [Route("Invoice/{Id}", Name = "Invoice")]
        public async Task<IActionResult> Invoice(string Id)
        {
            var CartItems = await _userRepository.Invoice(Id);
            if (CartItems.Count != 0)
            {
                return View(CartItems);
            }            
            else
            {
                return View(CartItems);
            }
        }
    }
}