using Entity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<Anime> Animes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Anime>().HasData(
                new Anime { Name = "Naruto", Language = "JPN", RatingLevel = 5 },
                new Anime { Name = "One Punch Man", Language = "KOR", RatingLevel = 4 },
                new Anime { Name = "Baruto", Language = "NPL", RatingLevel = 2 }
                );
        }
    }
}
