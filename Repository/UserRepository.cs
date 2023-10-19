using CoffeeTime.Data;
using CoffeeTime.Models.Domain;
using CoffeeTime.Service;
using Microsoft.EntityFrameworkCore;

namespace CoffeeTime.Repository
{
    public class UserRepository
    {
        private readonly CoffeeTimeDbContext coffeeTimeDbContext;
        private readonly IUserService _userService;
        public UserRepository(CoffeeTimeDbContext coffeeTimeDbContext, IUserService userService)
        {
            this.coffeeTimeDbContext = coffeeTimeDbContext;
            _userService = userService;
        }
        // generate new guid id
        Guid Guidid = Guid.NewGuid();

        public async Task<List<MenuList>> MenuItems()
        {
            // get the menu list and return it back
            var MenuItems = await coffeeTimeDbContext.MenuList.ToListAsync();
            return MenuItems;
        }

        public async Task<MenuList> AddItem(string Id)
        {
            // get the current user id
            var currentUserId = _userService.GetUserId();

            // select item from menu list based on based on the id passed
            var SelectedItem = await coffeeTimeDbContext.MenuList.SingleOrDefaultAsync(x => x.Id == Id);

            // check if the current user have a product with the selected item name and belong to an open order
            var CheckItem = await coffeeTimeDbContext.Products.Include(m => m.Order).Where(m => m.Order.UserId == currentUserId && m.Order.OrderStatus == "Open" && m.Name == SelectedItem.ItemName).FirstOrDefaultAsync();

            // check if the current user have an open order
            var CheckUser = await coffeeTimeDbContext.Products.Include(m => m.Order).Where(m => m.Order.UserId == currentUserId && m.Order.OrderStatus == "Open").FirstOrDefaultAsync();

            // increase the quantity if the current user have a product with the selected item name and belong to an open order
            if (CheckItem != null)
            {
                CheckItem.Quantity += 1;
                CheckItem.Price = SelectedItem.ItemPrice * CheckItem.Quantity;
                await coffeeTimeDbContext.SaveChangesAsync();
            }
            else
            {
                // if the current user have an open order but the selected item name doesn't exist in his products then add that item to his products
                if (CheckUser != null)
                {
                    var NewItem = new Products()
                    {
                        Id = Guidid.ToString(),
                        Name = SelectedItem.ItemName,
                        Price = SelectedItem.ItemPrice,
                        Quantity = 1,
                        ItemId = SelectedItem.Id,
                        OrderId = CheckUser.OrderId
                    };
                    await coffeeTimeDbContext.Products.AddAsync(NewItem);
                    await coffeeTimeDbContext.SaveChangesAsync();
                }
                // if he doesn't have an open order then create a new one and assign the selected item to his open order
                else
                {
                    var NewCart = new Products()
                    {
                        Id = Guidid.ToString(),
                        Name = SelectedItem.ItemName,
                        Price = SelectedItem.ItemPrice,
                        Quantity = 1,
                        ItemId = SelectedItem.Id,
                        Order = new Order()
                        {
                            OrderId = Guidid.ToString(),
                            UserId = currentUserId,
                            OrderStatus = "Open",
                            DateAndTime = DateTime.UtcNow,
                        }
                    };
                    await coffeeTimeDbContext.Products.AddAsync(NewCart);
                    await coffeeTimeDbContext.SaveChangesAsync();
                }
            }
            return SelectedItem;
        }

        public async Task<List<Products>> ViewCart()
        {
            // get the current user id
            var currentUserId = _userService.GetUserId();

            // check if the current user have an open order
            var CheckUser = await coffeeTimeDbContext.Order.Where(m => m.UserId == currentUserId && m.OrderStatus == "Open").FirstOrDefaultAsync();

            // calculate the total for the current user that have an open order
            var TotalPrice = coffeeTimeDbContext.Products.Where(m => m.Order.UserId == currentUserId && m.Order.OrderStatus == "Open").Sum(n => n.Price);

            // if the total price is not 0 then assign it to the specified field 
            if (TotalPrice != 0)
            {
                CheckUser.TotalPrice = TotalPrice;
                coffeeTimeDbContext.SaveChanges();
            }

            // get the list of the current user product that belong to an open order
            var Getuseritems = await coffeeTimeDbContext.Products.Include(m => m.Order).Where(m => m.Order.UserId == currentUserId && m.Order.OrderStatus == "Open").ToListAsync();

            return Getuseritems;
        }

        public async Task<string?> IncreaseQuantity(string Id)
        {
            // get the current user id
            var currentUserId = _userService.GetUserId();

            // get the selected item with passed id from the product table
            var SelectedItem = await coffeeTimeDbContext.Products.SingleOrDefaultAsync(x => x.Id == Id);

            // get the price of the selected item from the menu list based on the item id
            var ItemPrice = await coffeeTimeDbContext.MenuList.Where(m => m.Id == SelectedItem.ItemId).SingleOrDefaultAsync();

            // if the selected item is exist the increase the quantity by 1
            if (SelectedItem != null)
            {
                SelectedItem.Quantity += 1;
                SelectedItem.Price = SelectedItem.Quantity * ItemPrice.ItemPrice;
                coffeeTimeDbContext.SaveChanges();
            }

            return null;
        }

        public async Task<string?> DecreaseQuantity(string Id)
        {
            // get the current user id
            var currentUserId = _userService.GetUserId();

            // get the selected item with passed id from the product table
            var SelectedItem = await coffeeTimeDbContext.Products.SingleOrDefaultAsync(x => x.Id == Id);

            // get the price of the selected item from the menu list based on the item id
            var ItemPrice = await coffeeTimeDbContext.MenuList.Where(m => m.Id == SelectedItem.ItemId).SingleOrDefaultAsync();

            // if the selected item quantity less than or equal 1 then call the delete function the pass the selected item id to it
            // otherwise decrease the quantity by 1
            if (SelectedItem.Quantity <= 1)
            {
                return await DeleteItem(SelectedItem.Id);
            }
            else
            {
                SelectedItem.Quantity -= 1;
                SelectedItem.Price = SelectedItem.Quantity * ItemPrice.ItemPrice;
                coffeeTimeDbContext.SaveChanges();
            }

            return null;
        }

        public async Task<string?> DeleteItem(string Id)
        {
            // get the current user id
            var currentUserId = _userService.GetUserId();

            // get the selected item with passed id from the product table and delete it
            var SelectedItem = await coffeeTimeDbContext.Products.SingleOrDefaultAsync(x => x.Id == Id);
            coffeeTimeDbContext.Products.Remove(SelectedItem);
            await coffeeTimeDbContext.SaveChangesAsync();

            // check if the current user have an open order
            var CheckUser = await coffeeTimeDbContext.Order.Where(m => m.UserId == currentUserId && m.OrderStatus == "Open").FirstOrDefaultAsync();

            // get the list of the current user product that belong to an open order
            var Getuseritems = await coffeeTimeDbContext.Products.Include(m => m.Order).Where(m => m.Order.UserId == currentUserId && m.Order.OrderStatus == "Open").ToListAsync();

            // if the current user doesn't have products in his open order then close the order
            if (Getuseritems.Count == 0)
            {
                CheckUser.OrderStatus = "Closed";
                await coffeeTimeDbContext.SaveChangesAsync();
            }
            return null;
        }

        public async Task<string> NewOrder(string Id)
        {
            // get the order with passed id 
            var SelectedItem = await coffeeTimeDbContext.Products.Include(m => m.Order).Where(x => x.OrderId == Id).FirstOrDefaultAsync();

            // change the status of the order to in process and return OrderId
            if (SelectedItem != null)
            {
                SelectedItem.Order.OrderStatus = "In Process";
                coffeeTimeDbContext.SaveChanges();
                return SelectedItem.OrderId;
            }

            return null;
        }

        public async Task<string> CheckIsSuccess(string Id)
        {
            // get the current user id
            var currentUserId = _userService.GetUserId();

            // check if the current user have an order with passed id
            var SelectedItem = await coffeeTimeDbContext.Products.Include(m => m.Order).Where(x => x.OrderId == Id && x.Order.UserId == currentUserId).FirstOrDefaultAsync();

            // if the current user have an order with passed id then return the Orderod otherwise return null
            if (SelectedItem != null)
            {
                return SelectedItem.OrderId;
            }
            else
                return null;
        }

        public async Task<List<Order>> OrderHistory()
        {
            // get the current user id
            var currentUserId = _userService.GetUserId();

            // get a list of the current user orders that don't have an open status
            var Getuseritems = await coffeeTimeDbContext.Order.OrderByDescending(e => e.Id).Where(m => m.UserId == currentUserId && m.OrderStatus != "Open" && m.OrderStatus != "Closed").ToListAsync();

            return Getuseritems;
        }

        public async Task<List<Products>> Invoice(string Id)
        {
            // get the current user id
            var currentUserId = _userService.GetUserId();

            // get a list of the current user product with passed id
            var Getuseritems = await coffeeTimeDbContext.Products.Include(m => m.Order).Where(m => m.Order.UserId == currentUserId && m.Order.OrderId == Id).ToListAsync();

            return Getuseritems;
        }
    }
}