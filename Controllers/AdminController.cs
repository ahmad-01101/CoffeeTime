using CoffeeTime.Models;
using CoffeeTime.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeTime.Data;
namespace CoffeeTime.Controllers
{
    public class AdminController : Controller
    {
        private readonly CoffeeTimeDbContext coffeeTimeDbContext;
        public AdminController(CoffeeTimeDbContext coffeeTimeDbContext)
        {
            this.coffeeTimeDbContext = coffeeTimeDbContext;
        }
        public IActionResult AddItem(bool isSuccess = false)
        {
            ViewBag.IsSuccess = isSuccess;
            return View();
        }
        Guid Guidid = Guid.NewGuid();
        [HttpPost]
        public async Task<IActionResult> AddItem(MenuListVm menuListVm)
        {
            if (ModelState.IsValid)
            {
                var NewItem = new MenuList()
                {
                    Id = Guidid.ToString(),
                    ItemName = menuListVm.ItemName,
                    ItemPrice = (float)menuListVm.ItemPrice,
                };
                await coffeeTimeDbContext.AddAsync(NewItem);
                await coffeeTimeDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(AddItem), new { isSuccess = true });
            }
            return View(menuListVm);
        }   
        
        public async Task<IActionResult> MenuList(MenuListVm menuListVm)
        {
            var Item = await coffeeTimeDbContext.MenuList.ToListAsync();
            return View(Item);
        }
    }
}