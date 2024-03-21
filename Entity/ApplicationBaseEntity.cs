using Entity.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace Entity
{
    public class ApplicationBaseEntity : IBaseEntity, IDeletableEntity, IAuditableEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string LastModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset LastModifiedOn { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
