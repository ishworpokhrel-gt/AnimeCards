using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ApplicationRole : IdentityRole<string>
    {
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }

        public string LastModifiedBy { get; set; }
        public DateTimeOffset? LastModifiedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
