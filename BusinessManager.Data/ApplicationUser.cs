
using Microsoft.AspNetCore.Identity;

namespace BusinessManager.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Team Team { get; set; }
        public int? TeamId { get; set; }
    }
}
