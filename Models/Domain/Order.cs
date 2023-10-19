using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeTime.Models.Domain
{
    public class Order
    {
        [Key]
        public string OrderId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string OrderStatus { get; set; }

        public DateTime DateAndTime { get; set; }

        public float TotalPrice { get; set;}

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; } 
        public ApplicationUser ApplicationUser { get; set; }

        public List<Products> Products { get; set; }
    }
} 