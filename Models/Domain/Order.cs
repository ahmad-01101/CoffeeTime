namespace CoffeeTime.Models.Domain
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime DateAndTime { get; set; }
        public float TotalPrice { get; set;}
        public List<Products> Products { get; set; }
    }
}