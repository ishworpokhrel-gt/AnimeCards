using Common_Shared.SystemList;
using Entity;
using Microsoft.AspNetCore.Identity;

namespace Data.Seed
{
    public static class SeedRoles
    {
        public static async Task RoleSeeder(RoleManager<ApplicationRole> roleManager)
        {
            var listOfRoles = new List<ApplicationRole>
            {
                new ApplicationRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemConstant.AdminRole,
                    NormalizedName = SystemConstant.AdminRole
                },

                 new ApplicationRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemConstant.AgentRole,
                    NormalizedName = SystemConstant.AgentRole
                },

                  new ApplicationRole
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
