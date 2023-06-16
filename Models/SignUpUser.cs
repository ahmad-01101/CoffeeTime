using System.ComponentModel.DataAnnotations;

namespace CoffeeTime.Models
{
    public class SignUpUser
    {
        [Required(ErrorMessage = "Please enter your First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your phone number")]
        [RegularExpression(@"^(?:\+?0*?966)?0?(5[0-9]{8})$", ErrorMessage = "Please give a valid phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage ="Please enter your email")]
        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage ="Please enter a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a strong password")]
        [Compare("ConfirmPassword", ErrorMessage ="Password dose not match")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Please confirm your password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
