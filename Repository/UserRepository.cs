using CoffeeTime.Data;
using CoffeeTime.Models;
using CoffeeTime.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CoffeeTime.Repository
{
    public class UserRepository
    {
        private readonly CoffeeTimeDbContext coffeeTimeDbContext;
        public UserRepository(CoffeeTimeDbContext coffeeTimeDbContext)
        {
            this.coffeeTimeDbContext = coffeeTimeDbContext;
        }
        public async Task<string?> AddItem(CartListVm cartListVm, string ItemStatus)
        {
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
    }
}