using Common_Shared.SystemList;
using Entity;
using Microsoft.AspNetCore.Identity;

namespace Data.Seed
{
    public static class SeedUser
    {
        public static async Task UserSeeder(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            if (!context.Users.Any())
            {
                var user = new ApplicationUser
                {
                    UserName = "Admin",
                    FullName = "Admin",
                    Email = "Admin@Admin.com",
                    Id = Guid.NewGuid().ToString(),
                    PhoneNumber = "1234567890",
                };

                var userCustomer = new ApplicationUser
                {
                    UserName = "Customer",
                    FullName = "Customer",
                    Email = "Customer@Customer.com",
                    Id = Guid.NewGuid().ToString(),
                    PhoneNumber = "1234567890",
                };

                await userManager.CreateAsync(userCustomer, "Admin@123");
                await userManager.AddToRoleAsync(userCustomer, SystemConstant.CustomerRole);
                await userManager.CreateAsync(user, "Admin@123");
                await userManager.AddToRoleAsync(user, SystemConstant.AdminRole);

                await context.Users.AddAsync(userCustomer);
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

            }



        }
    }
}
