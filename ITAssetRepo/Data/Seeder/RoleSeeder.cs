using Microsoft.AspNetCore.Identity;

namespace ITAssetRepo.Data.Seeder
{
    //Contains the logic for seeding the initial roles into the database.
    public static class RoleSeeder
    {
        public static async Task Initializer(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "admin", "technician", "asset_team" };
            IdentityResult roleResult;

            foreach (var rolename in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(rolename);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(rolename));
                }
            }
        }
    }
}
