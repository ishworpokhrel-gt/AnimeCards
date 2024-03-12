using Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<Anime> Animes { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(entity => { entity.ToTable(name: "Users"); });
            modelBuilder.Entity<Anime>().HasData(
                new Anime { Name = "Naruto", Language = "JPN", RatingLevel = 5 },
                new Anime { Name = "One Punch Man", Language = "KOR", RatingLevel = 4 },
                new Anime { Name = "Baruto", Language = "NPL", RatingLevel = 2 }
                );
        }
    }
}
