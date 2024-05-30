using Microsoft.AspNetCore.Identity;

namespace ITAssetRepo.Data.Seeder
{
    public static class RoleSeeder
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if(!roleManager.Roles.Any())
            {
                string[] rolenames = { "admin", "technician", "asset_team" };
                foreach(var rolename in rolenames)
                {
                    if(!await roleManager.RoleExistsAsync(rolename))
                    {
                        var roleResult = await roleManager.CreateAsync(new IdentityRole(rolename));
                        if (!roleResult.Succeeded)
                        {
                            throw new Exception($"Failed to create role {rolename}");
                        }
                    }
                }
            }
        }
    }
}
