using Entity.BaseEntities;
using Microsoft.AspNetCore.Identity;
using static System.Net.WebRequestMethods;

namespace Entity
{
    public class ApplicationUser : IdentityUser<string> , IBaseEntity,IAuditableEntity,IDeletableEntity
    {
        public string FullName { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public ICollection<UserOtp> Otp { get; set; }
        public bool IsRegistrationComplete { get; set; }

        public string LastModifiedBy { get; set; }
        public DateTimeOffset? LastModifiedOn { get; set; }


        public bool IsDeleted { get; set; }

    }
}
