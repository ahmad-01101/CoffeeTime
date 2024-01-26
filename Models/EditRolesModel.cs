using System.ComponentModel.DataAnnotations;

namespace CoffeeTime.Models
{
    public class EditRolesModel
    {
        public EditRolesModel()
        {
            Users = new List<string>();
        }

        public string Id { get; set; }

        [Required(ErrorMessage = " الرجاء ادخال اسم الدور ")]
        [Display(Name = " اسم الدور ")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
