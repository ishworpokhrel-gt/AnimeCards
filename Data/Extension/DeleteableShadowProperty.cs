using Entity.BaseEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Data.Extension
{
    public static class DeleteableShadowProperty
    {
        public static readonly Func<object, bool> EfPropertyIsDelete =
            entity => EF.Property<bool>(entity, IsDeleted);

        public static readonly string IsDeleted = nameof(IsDeleted);

        public static void AddSoftDeleteShadowProperty(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model
                                                   .GetEntityTypes()
                                                   .Where(e => typeof(IDeletableEntity).IsAssignableFrom(e.ClrType)))
                modelBuilder.Entity(entityType.ClrType)
                            .Property<bool>(IsDeleted);
        }

        public static void SetSoftDeleteEntityPropertyValues(
            this ChangeTracker changeTracker, string userId = default)
        {
            var now = DateTimeOffset.UtcNow;
            var modifiedEntries = changeTracker.Entries<IDeletableEntity>()
                                               .Where(x => x.State == EntityState.Deleted);

            foreach (var modifiedEntry in modifiedEntries)
            {
                modifiedEntry.State = EntityState.Modified;
                modifiedEntry.Property(IsDeleted).CurrentValue = true;
                modifiedEntry.Property(AuditableShadowProperties.LastModifiedOn).CurrentValue = DateTimeOffset.UtcNow;

                if (userId is { Length: > 0 })
                    modifiedEntry.Property(AuditableShadowProperties.LastModifiedBy).CurrentValue = userId;
            }
        }
    }
}
