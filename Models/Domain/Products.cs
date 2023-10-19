namespace CoffeeTime.Models.Domain
{
    public class Products
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string ItemId { get; set; }
        public string OrderId { get; set; }
        public Order Order { get; set; }
    }
}
 