using Common_Shared.Constants;
using Common_Shared.SystemList;
using Microsoft.EntityFrameworkCore;

namespace Data.Seed
{
    public class PermissionSeeder
    {
        public static async Task SeedAdminPermission(AppDbContext context)
        {
            var roleId = await context.Roles
                                            .Where(x => x.Name == SystemConstant.AdminRole)
                                            .Select(x => x.Id)
                                            .FirstOrDefaultAsync();
            var roleClaims = await context.RoleClaims.FirstOrDefaultAsync(x => x.RoleId == roleId);
            var allPermissions = PermissionConstants.GetAllPermissions();

            if (roleClaims == null)
            {
                await context.RoleClaims.AddAsync(new Entity.RoleClaims
                {
                    RoleId = roleId,
                    Permissions = allPermissions
                });
            }
            else
            {
                roleClaims.Permissions = allPermissions;
                context.RoleClaims.Update(roleClaims);
            }

            await context.SaveChangesAsync();
        }

    }
}
