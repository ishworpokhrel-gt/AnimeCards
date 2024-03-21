using Entity.BaseEntities;
using Microsoft.AspNetCore.Identity;

namespace Entity
{
    public class ApplicationUser : IdentityUser<string> , IBaseEntity,IAuditableEntity,IDeletableEntity
    {
        public string FullName { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }

        public string LastModifiedBy { get; set; }
        public DateTimeOffset? LastModifiedOn { get; set; }


        public bool IsDeleted { get; set; }

    }
}
