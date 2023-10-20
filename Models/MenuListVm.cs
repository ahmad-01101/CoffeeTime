using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeTime.Models
{
    public class MenuListVm
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Please enter Item Name")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Please enter Item Price")]
        public float? ItemPrice { get; set; }
    }
}