using Entity.BaseEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Data.Extension
{
    public static class AuditableShadowProperties
    {
        public static readonly Func<object, DateTimeOffset?> EfPropertyCreatedDateTime =
           entity => EF.Property<DateTimeOffset?>(entity, CreatedOn);

        public static readonly string CreatedOn = nameof(CreatedOn);

        public static readonly Func<object, DateTimeOffset?> EfPropertyModifiedDateTime =
            entity => EF.Property<DateTimeOffset?>(entity, LastModifiedOn);

        public static readonly string LastModifiedOn = nameof(LastModifiedOn);

        public static readonly Func<object, string> EfPropertyCreatedBy =
            entity => EF.Property<string>(entity, CreatedBy);

        public static readonly string CreatedBy = nameof(CreatedBy);

        public static readonly Func<object, string> EfPropertyLastModifiedBy =
            entity => EF.Property<string>(entity, LastModifiedBy);

        public static readonly string LastModifiedBy = nameof(LastModifiedBy);

        public static void AddAuditableShadowProperties(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model
                                                   .GetEntityTypes()
                                                   .Where(e => typeof(IBaseEntity).IsAssignableFrom(e.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType)
                            .Property<DateTimeOffset?>(CreatedOn);

                modelBuilder.Entity(entityType.ClrType)
                            .Property<DateTimeOffset?>(LastModifiedOn);

                modelBuilder.Entity(entityType.ClrType)
                            .Property<string>(CreatedBy)
                            .HasMaxLength(128);

                modelBuilder.Entity(entityType.ClrType)
                            .Property<string>(LastModifiedBy)
                            .HasMaxLength(128);
            }
        }

        public static void SetAuditableEntityPropertyValues(
            this ChangeTracker changeTracker, string userId = default)
        {
            var now = DateTimeOffset.UtcNow;

            var modifiedEntries = changeTracker.Entries<IBaseEntity>()
                                               .Where(x => x.State == EntityState.Modified);
            foreach (var modifiedEntry in modifiedEntries)
            {
                modifiedEntry.Property(LastModifiedOn).CurrentValue = now;
                if (userId is { Length: > 0 }) modifiedEntry.Property(LastModifiedBy).CurrentValue = userId;
            }

            var addedEntries = changeTracker.Entries<IBaseEntity>()
                                            .Where(x => x.State == EntityState.Added);
            foreach (var addedEntry in addedEntries)
            {
                addedEntry.Property(CreatedOn).CurrentValue = now;

                if (userId is { Length: > 0 }) addedEntry.Property(CreatedBy).CurrentValue = userId;
            }
        }
    }
}
