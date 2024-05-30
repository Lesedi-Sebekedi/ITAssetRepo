using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITAssetRepo.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Adding Roles
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);

        //    var admin = new IdentityRole("admin");
        //    admin.NormalizedName = "admin";
        //    var technician = new IdentityRole("technician");
        //    technician.NormalizedName = "technician";
        //    var asset_team = new IdentityRole("asset_team");
        //    asset_team.NormalizedName = "asset_team";

        //    builder.Entity<IdentityRole>().HasData(admin, technician, asset_team);
        //}
        public DbSet<ITAssetRepo.Models.Asset_list> Asset_list { get; set; } = default!;
    }
}
