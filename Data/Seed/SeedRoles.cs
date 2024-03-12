using Common_Shared.SystemList;
using Microsoft.AspNetCore.Identity;

namespace Data.Seed
{
    public static class SeedRoles
    {
        public static async Task RoleSeeder(RoleManager<IdentityRole> roleManager)
        {
            var listOfRoles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemConstant.AdminRole,
                    NormalizedName = SystemConstant.AdminRole
                },

                 new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemConstant.AgentRole,
                    NormalizedName = SystemConstant.AgentRole
                },

                  new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemConstant.CustomerRole,
                    NormalizedName = SystemConstant.CustomerRole
                }
            };

            foreach (var role in listOfRoles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }
    }
}
