namespace CoffeeTime.Models.Domain
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string ItemStatus { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
