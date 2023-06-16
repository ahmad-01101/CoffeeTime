using CoffeeTime.Models.Domain;

namespace CoffeeTime.Models
{
    public class CartListVm
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime DateAndTime { get; set; }
        public int TotalPrice { get; set; }

        //

        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }

    }
}
