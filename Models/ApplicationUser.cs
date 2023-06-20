using CoffeeTime.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace CoffeeTime.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
