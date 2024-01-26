using CoffeeTime.Models;
using CoffeeTime.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeTime.Data;
using CoffeeTime.Repository;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;

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
        
        public async Task<IActionResult> CustomerOrders(int Id, bool isSuccess = false)
        {
            ViewBag.IsSuccess = isSuccess;

            if (Id != 0)
            {
                var SelectedOrder = await coffeeTimeDbContext.Order.Where(m => m.Id == Id).ToListAsync();
                return View(SelectedOrder);
            }
            var Item = await coffeeTimeDbContext.Order.OrderByDescending(e => e.Id).ToListAsync();
            return View(Item);
        }

        [Route("Order-Ready/{Id}", Name = "OrderReady")]
        public async Task<IActionResult> OrderReady(string Id)
        {
            var SelectedItem = await coffeeTimeDbContext.Order.FindAsync(Id);

            if (SelectedItem != null)
            {
                SelectedItem.OrderStatus = "Ready";
                await coffeeTimeDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(CustomerOrders), new { isSuccess = true });
            }
            return RedirectToAction(nameof(CustomerOrders), new { isSuccess = false });
        }
        
        [Route("Order-Closed/{Id}", Name = "OrderClosed")]
        public async Task<IActionResult> OrderClosed(string Id)
        {
            var SelectedItem = await coffeeTimeDbContext.Order.FindAsync(Id);

            if (SelectedItem != null)
            {
                SelectedItem.OrderStatus = "Closed";
                await coffeeTimeDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(CustomerOrders), new { isSuccess = true });
            }
            return RedirectToAction(nameof(CustomerOrders), new { isSuccess = false });
        }
        
        [Route("Order-Canceled/{Id}", Name = "OrderCanceled")]
        public async Task<IActionResult> OrderCanceled(string Id)
        {
            var SelectedItem = await coffeeTimeDbContext.Order.FindAsync(Id);

            if (SelectedItem != null)
            {
                SelectedItem.OrderStatus = "Canceled";
                await coffeeTimeDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(CustomerOrders), new { isSuccess = true });
            }
            return RedirectToAction(nameof(CustomerOrders), new { isSuccess = false });
        }

        public async Task<IActionResult> MenuList(MenuListVm menuListVm, bool isSuccess = false)
        {
            ViewBag.IsSuccess = isSuccess;
            var Item = await coffeeTimeDbContext.MenuList.ToListAsync();
            return View(Item);
        }

        [Route("Item-Details/{Id}", Name = "ItemDetails")]
        public async Task<IActionResult> EditItem(string Id, bool isSuccess = false)
        {
            ViewBag.IsSuccess = isSuccess;
            var SelectedItem = await coffeeTimeDbContext.MenuList.SingleOrDefaultAsync(x => x.Id == Id);
            return View(SelectedItem);
        }

        
        [HttpPost]
        [Route("Update-Item/{Id}", Name = "UpdateItem")]
        public async Task<IActionResult> UpdateItem(string Id, MenuListVm menuListVm)
        {
            var SelectedItem = await coffeeTimeDbContext.MenuList.FindAsync(menuListVm.Id);

            if (SelectedItem != null)
            {
                SelectedItem.ItemName = menuListVm.ItemName;
                SelectedItem.ItemPrice = (float)menuListVm.ItemPrice;
                await coffeeTimeDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(MenuList), new { isSuccess = true });
            }
            return RedirectToAction("MenuList");
        }

        [Route("Delete-Menu-Item/{Id}", Name = "AdminDeleteItem")]
        public async Task<IActionResult> DeleteMenuItem(string Id)
        {
            ViewBag.IsSuccess = true;
            var SelectedItem = await coffeeTimeDbContext.MenuList.SingleOrDefaultAsync(x => x.Id == Id);
            coffeeTimeDbContext.MenuList.Remove(SelectedItem);
            await coffeeTimeDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(MenuList), new { isSuccess = true });
        }

    }
}