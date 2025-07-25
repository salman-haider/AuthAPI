using Microsoft.AspNetCore.Identity;

namespace API.helper.services
{
    public static class SeedRoles
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var config = serviceProvider.GetRequiredService<IConfiguration>();

            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            if (await userManager.FindByEmailAsync("admin@admin.com") == null)
            {
                var IdResult = new IdentityResult();
                IdentityUser admin = new IdentityUser("admin") { Email = "admin@admin.com" };
                IdResult = await userManager.CreateAsync(admin, config["AdPass"]);
                if (IdResult.Succeeded)
                {
                    IdResult = await userManager.AddToRoleAsync(admin, "Admin");
                }

            }

        }
    }
}
