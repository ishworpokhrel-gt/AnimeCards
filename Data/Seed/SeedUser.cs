using Common_Shared.SystemList;
using Entity;
using Entity.Account;
using Microsoft.AspNetCore.Identity;

namespace Data.Seed
{
    public static class SeedUser
    {
        public static async Task UserSeeder(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            if (!context.Users.Any())
            {
                var userAdmin = new ApplicationUser
                {
                    UserName = "Admin",
                    FullName = "Admin",
                    Email = "Admin@Admin.com",
                    Id = Guid.NewGuid().ToString(),
                    PhoneNumber = "1234567890",
                    IsRegistrationComplete = true,
                };

                var userCustomer = new ApplicationUser
                {
                    UserName = "Customer",
                    FullName = "Customer",
                    Email = "Customer@Customer.com",
                    Id = Guid.NewGuid().ToString(),
                    PhoneNumber = "1234567890",
                    IsRegistrationComplete = true,
                };

                var customer = new Customer
                {
                    UserId = userCustomer.Id,
                    FullName = userCustomer.FullName
                };
                var admin = new Admin
                {
                    UserId = userAdmin.Id,
                    FullName = userAdmin.FullName
                };

                await userManager.CreateAsync(userCustomer, "Admin@123");
                await userManager.AddToRoleAsync(userCustomer, SystemConstant.CustomerRole);
                await userManager.CreateAsync(userAdmin, "Admin@123");
                await userManager.AddToRoleAsync(userAdmin, SystemConstant.AdminRole);

          
                await context.Customer.AddAsync(customer);
                await context.Admin.AddAsync(admin);
                await context.SaveChangesAsync();

            }



        }
    }
}
