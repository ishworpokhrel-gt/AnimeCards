using Microsoft.AspNetCore.Identity;

namespace Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
       
    }
}
