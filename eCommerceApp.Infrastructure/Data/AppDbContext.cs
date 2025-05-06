using eCommerceApp.Domain.Entities;
using eCommerceApp.Domain.Entities.Cart;
using eCommerceApp.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<PaymentMetod> PaymentMetods { get; set; }
        public DbSet<Achive> CheckoutAchives { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PaymentMetod>()
                .HasData(
                new PaymentMetod
                {
                    Id = Guid.Parse("d1a9c7b6-71e8-4a2e-b898-6fc237cd77c7"), // Use a fixed GUID
                    Name = "Credit Card",
                });

            modelBuilder.Entity<IdentityRole>()
                .HasData(
                new IdentityRole
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9", // Static GUID string
                    Name = "Admin",
                    NormalizedName = "ADMIN" // This should match the name but uppercase
                },
                new IdentityRole
                {
                    Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", // Static GUID string
                    Name = "User",
                    NormalizedName = "USER"
                }
                );
        }
    }
}
