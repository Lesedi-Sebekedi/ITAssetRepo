using ITAssetRepo.Models;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>()
                .Property(e => e.Asset_Cost)
                .HasColumnType("decimal(18, 2)");  // Precision: 18 digits, Scale: 2 decimal places

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Asset> Assets { get; set; } = default!;
    }
}
