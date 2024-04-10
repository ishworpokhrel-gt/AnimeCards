using Common_Shared.Accessor;
using Data.Extension;
using Entity;
using Entity.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private readonly IUserAccessor _userAccessor;
        public AppDbContext(DbContextOptions<AppDbContext> options, IUserAccessor userAccessor) : base(options)
        {
            _userAccessor = userAccessor;
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            UpdateShadowFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateShadowFields();
            return base.SaveChanges();
        }

        private void UpdateShadowFields()
        {
            var userId = _userAccessor.GetUserId().ToString();
            ChangeTracker.SetAuditableEntityPropertyValues(userId);
            ChangeTracker.SetSoftDeleteEntityPropertyValues(userId);
        }


        public DbSet<Anime> Animes { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<RoleClaims> RoleClaims { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<UserOtp> UserOtp { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<PreviousPassword> PreviousPassword { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(entity => { entity.ToTable(name: "Users"); });
            modelBuilder.Entity<ApplicationRole>(entity => { entity.ToTable(name: "Roles"); });
            modelBuilder.Entity<Anime>().HasData(
                new Anime { Name = "Naruto", Language = "JPN", RatingLevel = 5, ImageUrl = "" },
                new Anime { Name = "One Punch Man", Language = "KOR", RatingLevel = 4, ImageUrl = "" },
                new Anime { Name = "Baruto", Language = "NPL", RatingLevel = 2, ImageUrl = "" }
                );
        }
    }
}
