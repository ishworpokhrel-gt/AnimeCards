using System.ComponentModel.DataAnnotations;

namespace Entity
{
    public class ApplicationBaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? LastModifiedBy { get; set; }
        public DateTimeOffset LastModifiedOn { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
