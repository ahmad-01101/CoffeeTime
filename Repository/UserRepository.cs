using CoffeeTime.Data;
using CoffeeTime.Models;
using CoffeeTime.Models.Domain;
using CoffeeTime.Service;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        public async Task<string?> AddItem(CartListVm cartListVm, string ItemStatus)
        {
            var currentUserId = _userService.GetUserId();
            //var OrderStatus = "Open";
            //var ItemStatus = "AmericanoOpen";
            var CheckStatus = await coffeeTimeDbContext.Order.Where(m => m.OrderStatus == "Open").FirstOrDefaultAsync();
            //var CheckItem = await coffeeTimeDbContext.Products.FirstOrDefaultAsync(x => x.Name == name);
            var CheckItem = await coffeeTimeDbContext.Products.FirstOrDefaultAsync(x => x.ItemStatus == ItemStatus);
            if (CheckStatus == null)
            {
                var NewCart = new Products()
                {
                    Name = cartListVm.Name,
                    Price = cartListVm.Price,
                    Quantity = cartListVm.Quantity,
                    ItemStatus = ItemStatus,
                    Order = new Order()
                    {
                        UserId = currentUserId,
                        OrderStatus = "Open",
                        //TotalPrice = 9*cartListVm.Quantity,
                        DateAndTime = DateTime.UtcNow,
                    }

                };
                await coffeeTimeDbContext.Products.AddAsync(NewCart);
                await coffeeTimeDbContext.SaveChangesAsync();
            }
            else
            {
                if (CheckItem == null)
                {
                    var NewItem = new Products()
                    {
                        Name = cartListVm.Name,
                        Price = cartListVm.Price,
                        Quantity = cartListVm.Quantity,
                        ItemStatus = ItemStatus,
                        OrderId = CheckStatus.OrderId
                    };
                    await coffeeTimeDbContext.Products.AddAsync(NewItem);
                    await coffeeTimeDbContext.SaveChangesAsync();
                }
                else
                {
                    CheckItem.Quantity = cartListVm.Quantity;
                    await coffeeTimeDbContext.SaveChangesAsync();
                }
            }
            return null;
        }   
        
        public async Task<string?> AddItem1(int Id)
        {
            var currentUserId = _userService.GetUserId();
            //var OrderStatus = "Open";
            //var ItemStatus = "AmericanoOpen";
            var SelectedItem = await coffeeTimeDbContext.MenuList.SingleOrDefaultAsync(x => x.Id == Id);
            var CheckStatus = await coffeeTimeDbContext.Order.Where(m => m.OrderStatus == "Open").FirstOrDefaultAsync();
            //var CheckItem = await coffeeTimeDbContext.Products.FirstOrDefaultAsync(x => x.Name == name);
            var ItemStatus = SelectedItem.ItemName+"Open";
            var CheckItem = await coffeeTimeDbContext.Products.FirstOrDefaultAsync(x => x.ItemStatus == ItemStatus);
            if (CheckStatus == null)
            {
                var NewCart = new Products()
                {
                    Name = SelectedItem.ItemName,
                    Price = SelectedItem.ItemPrice,
                    Quantity = 1,
                    ItemStatus = ItemStatus,
                    Order = new Order()
                    {
                        UserId = currentUserId,
                        OrderStatus = "Open",
                        //TotalPrice = 9*cartListVm.Quantity,
                        DateAndTime = DateTime.UtcNow,
                    }

                };
                await coffeeTimeDbContext.Products.AddAsync(NewCart);
                await coffeeTimeDbContext.SaveChangesAsync();
            }
            else
            {
                if (CheckItem == null)
                {
                    var NewItem = new Products()
                    {
                        Name = SelectedItem.ItemName,
                        Price = SelectedItem.ItemPrice,
                        Quantity = 1,
                        ItemStatus = ItemStatus,
                        OrderId = CheckStatus.OrderId
                    };
                    await coffeeTimeDbContext.Products.AddAsync(NewItem);
                    await coffeeTimeDbContext.SaveChangesAsync();
                }
                else
                {
                    CheckItem.Quantity = 1;
                    await coffeeTimeDbContext.SaveChangesAsync();
                }
            }
            return null;
        }
        public async Task<string?> AddItem2(int Id)
        {
            // get the current user id
            var currentUserId = _userService.GetUserId();
            var SelectedItem = await coffeeTimeDbContext.MenuList.SingleOrDefaultAsync(x => x.Id == Id);
            var CheckUser = await coffeeTimeDbContext.Order.Where(m => m.UserId == currentUserId).FirstOrDefaultAsync();
            var CheckItem = await coffeeTimeDbContext.Products.Where(m => m.Order.UserId == currentUserId).FirstOrDefaultAsync();
            if (CheckUser == null)
            {
                var NewCart = new Products()
                {
                    Name = SelectedItem.ItemName,
                    Price = SelectedItem.ItemPrice,
                    Quantity = 1,
                    ItemStatus = "open",
                    Order = new Order()
                    {
                        UserId = currentUserId,
                        OrderStatus = "Open",
                        DateAndTime = DateTime.UtcNow,
                    }
                };
                await coffeeTimeDbContext.Products.AddAsync(NewCart);
                await coffeeTimeDbContext.SaveChangesAsync();
            }

            else
            {
                if (CheckUser.OrderStatus == "Open")
                {
                    if (CheckItem.Name == SelectedItem.ItemName)
                    {
                        CheckItem.Quantity += 1;
                        CheckItem.Price = SelectedItem.ItemPrice * CheckItem.Quantity;
                        await coffeeTimeDbContext.SaveChangesAsync();
                    }
                    else
                    {
                        var NewItem = new Products()
                        {
                            Name = SelectedItem.ItemName,
                            Price = SelectedItem.ItemPrice,
                            Quantity = 1,
                            ItemStatus = "open",
                            OrderId = CheckItem.OrderId
                        };
                        await coffeeTimeDbContext.Products.AddAsync(NewItem);
                        await coffeeTimeDbContext.SaveChangesAsync();
                    }
                }
            }
            return null;
        }
        
        public async Task<string?> AddItem3(int Id)
        {
            // get the current user id
            var currentUserId = _userService.GetUserId();

            // select item from menu list based on based on the id passed
            var SelectedItem = await coffeeTimeDbContext.MenuList.SingleOrDefaultAsync(x => x.Id == Id);

            // check if the current user have an open order
            var CheckUser = await coffeeTimeDbContext.Order.Where(m => m.UserId == currentUserId && m.OrderStatus == "Open").FirstOrDefaultAsync();

            // check if the current user have a product with a name that matches the selected product name
            var CheckItem = await coffeeTimeDbContext.Products.Include(m => m.Order).Where(m => m.Order.UserId == currentUserId && m.Name == SelectedItem.ItemName).FirstOrDefaultAsync();

            // if the current user don't have an open order then do the following
            if (CheckUser == null)
            {
                // create a new order with the selected item
                var NewCart = new Products()
                {
                    Name = SelectedItem.ItemName,
                    Price = SelectedItem.ItemPrice,
                    Quantity = 1,
                    ItemStatus = "open",
                    Order = new Order()
                    {
                        UserId = currentUserId,
                        OrderStatus = "Open",
                        DateAndTime = DateTime.UtcNow,
                    }
                };
                await coffeeTimeDbContext.Products.AddAsync(NewCart);
                await coffeeTimeDbContext.SaveChangesAsync();
            }
            // 
            else
            {
                if (CheckItem == null)
                {
                    var NewItem = new Products()
                    {
                        Name = SelectedItem.ItemName,
                        Price = SelectedItem.ItemPrice,
                        Quantity = 1,
                        ItemStatus = "open",
                        OrderId = CheckUser.OrderId
                    };
                    await coffeeTimeDbContext.Products.AddAsync(NewItem);
                    await coffeeTimeDbContext.SaveChangesAsync();
                }

                else if (CheckItem == null && CheckItem.Order.OrderStatus == "Close")
                {
                    var NewItem = new Products()
                    {
                        Name = SelectedItem.ItemName,
                        Price = SelectedItem.ItemPrice,
                        Quantity = 1,
                        ItemStatus = "open",
                        OrderId = CheckUser.OrderId
                    };
                    await coffeeTimeDbContext.Products.AddAsync(NewItem);
                    await coffeeTimeDbContext.SaveChangesAsync();
                }

                else if(CheckItem.Order.OrderStatus == "Open")
                {
                    CheckItem.Quantity += 1;
                    CheckItem.Price = SelectedItem.ItemPrice * CheckItem.Quantity;
                    await coffeeTimeDbContext.SaveChangesAsync();
                }
            }
            return null;
        }
        
        public async Task<string?> AddItem4(int Id)
        {
            // get the current user id
            var currentUserId = _userService.GetUserId();

            // select item from menu list based on based on the id passed
            var SelectedItem = await coffeeTimeDbContext.MenuList.SingleOrDefaultAsync(x => x.Id == Id);

            // check if the current user have an open product
            var CheckItem = await coffeeTimeDbContext.Products.Include(m => m.Order).Where(m => m.Order.UserId == currentUserId && m.Order.OrderStatus == "Open" && m.Name == SelectedItem.ItemName).FirstOrDefaultAsync();

            // check if the current user have a product with a name that matches the selected product name
            var CheckUser = await coffeeTimeDbContext.Products.Include(m => m.Order).Where(m => m.Order.UserId == currentUserId && m.Order.OrderStatus == "Open").FirstOrDefaultAsync();

            if (CheckItem != null)
            {
                CheckItem.Quantity += 1;
                CheckItem.Price = SelectedItem.ItemPrice * CheckItem.Quantity;
                await coffeeTimeDbContext.SaveChangesAsync();
            }
            else
            {
                if(CheckUser != null)
                {
                    var NewItem = new Products()
                    {
                        Name = SelectedItem.ItemName,
                        Price = SelectedItem.ItemPrice,
                        Quantity = 1,
                        ItemStatus = "open",
                        OrderId = CheckUser.OrderId
                    };
                    await coffeeTimeDbContext.Products.AddAsync(NewItem);
                    await coffeeTimeDbContext.SaveChangesAsync();
                }
                else
                {
                    var NewCart = new Products()
                    {
                        Name = SelectedItem.ItemName,
                        Price = SelectedItem.ItemPrice,
                        Quantity = 1,
                        ItemStatus = "open",
                        Order = new Order()
                        {
                            UserId = currentUserId,
                            OrderStatus = "Open",
                            DateAndTime = DateTime.UtcNow,
                        }
                    };
                    await coffeeTimeDbContext.Products.AddAsync(NewCart);
                    await coffeeTimeDbContext.SaveChangesAsync();
                }
            }

            return null;
        }
        public async Task<List<Products>> ViewCart()
        {
            //var CartItems = await coffeeTimeDbContext.Products
            //.Include(m => m.Order)
            //.SingleOrDefaultAsync(m => m.Order.OrderStatus == "Open");
            //return CartItems;

            //var CartItems = await coffeeTimeDbContext.Products
            //.Include(m => m.Order.OrderStatus == "Open")
            //.ToListAsync();
            //return CartItems;
            //var allItemFromProduct = await coffeeTimeDbContext.Products.SumAsync(n=>n.Price);
            var allItemFromProduct =  coffeeTimeDbContext.Products.Where(c => c.Order.OrderStatus == "Open").Sum(n => n.Price);
            var fortheTotal = coffeeTimeDbContext.Order.Where(c => c.OrderStatus == "Open");
            var fortheTotal1 = coffeeTimeDbContext.Order.Single(m => m.OrderStatus == "Open");


            fortheTotal1.TotalPrice = allItemFromProduct;
            coffeeTimeDbContext.SaveChanges();

            //var total = await coffeeTimeDbContext.Products.ToListAsync();
            var CartItems = await coffeeTimeDbContext.Products
            .Include(m => m.Order).Where(x=>x.Order.OrderStatus == "Open")
            .ToListAsync();
            if(CartItems != null)
            {
               
            }
            return CartItems;
        }
        
        public async Task<List<Products>> ViewCart1()
        {
            var currentUserId = _userService.GetUserId();
            
            var Getuser = await coffeeTimeDbContext.Order.Where(m => m.UserId == currentUserId && m.OrderStatus == "Open").FirstOrDefaultAsync();
            
            var TotalPrice = coffeeTimeDbContext.Products.Where(m => m.Order.UserId == currentUserId && m.Order.OrderStatus == "Open").Sum(n => n.Price);
            
            Getuser.TotalPrice = TotalPrice;
            coffeeTimeDbContext.SaveChanges();

            var Getuseritems = await coffeeTimeDbContext.Products.Where(m => m.Order.UserId == currentUserId && m.Order.OrderStatus == "Open").ToListAsync();

            if(Getuseritems != null)
            {
               
            }
            return Getuseritems;
        }

        public async Task<string?> IncreaseQuantity(int Id)
        {
            // get the current user id
            var currentUserId = _userService.GetUserId();

            var SelectedItem = await coffeeTimeDbContext.Products.SingleOrDefaultAsync(x => x.Id == Id);

            var ItemPrice = await coffeeTimeDbContext.MenuList.Where(m => m.ItemName == SelectedItem.Name).SingleOrDefaultAsync();

            if (SelectedItem != null)
            {
                SelectedItem.Quantity += 1;
                SelectedItem.Price = SelectedItem.Quantity * ItemPrice.ItemPrice;
                coffeeTimeDbContext.SaveChanges();
            }
             
            return null;
        }
        
        public async Task<string?> DecreaseQuantity(int Id)
        {
            // get the current user id
            var currentUserId = _userService.GetUserId();

            var SelectedItem = await coffeeTimeDbContext.Products.SingleOrDefaultAsync(x => x.Id == Id);

            var ItemPrice = await coffeeTimeDbContext.MenuList.Where(m => m.ItemName == SelectedItem.Name).SingleOrDefaultAsync();

            if (SelectedItem.Quantity == 0)
            {
                return await DeleteItem(SelectedItem.Id);
            }
            if (SelectedItem != null)
            {
                SelectedItem.Quantity -= 1;
                SelectedItem.Price = SelectedItem.Quantity * ItemPrice.ItemPrice;
                coffeeTimeDbContext.SaveChanges();
            }
             
            return null;
        }
        
        public async Task<string?> DeleteItem(int Id)
        {
            // get the current user id
            var currentUserId = _userService.GetUserId();

            var SelectedItem = await coffeeTimeDbContext.Products.SingleOrDefaultAsync(x => x.Id == Id);

            coffeeTimeDbContext.Products.Remove(SelectedItem);
            await coffeeTimeDbContext.SaveChangesAsync();

            return null;
        }

    }
}