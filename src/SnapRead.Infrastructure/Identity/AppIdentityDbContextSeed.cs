
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SnapRead.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //string adminRole = "Administrators";
            //await roleManager.CreateAsync(new IdentityRole(adminRole));
            //var defaultUser = new ApplicationUser { UserName = "admin", Email = "info.snapread@gmail.com" };
            //await userManager.CreateAsync(defaultUser, AuthorizationConstants.DEFAULT_PASSWORD);

            //string adminUserName = "admin@microsoft.com";
            //var adminUser = new ApplicationUser { UserName = adminUserName, Email = adminUserName };
            //await userManager.CreateAsync(adminUser, AuthorizationConstants.DEFAULT_PASSWORD);
            //adminUser = await userManager.FindByNameAsync(adminUserName);
            //await userManager.AddToRoleAsync(adminUser, AuthorizationConstants.Roles.ADMINISTRATORS);
        }
    }
}
