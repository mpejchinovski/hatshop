using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using hatshop.Models;

namespace hatshop.Data
{
    public class HatShopDbContext : IdentityDbContext<ApplicationUser>
    {
        public HatShopDbContext(DbContextOptions<HatShopDbContext> options) : base(options)
        { }

        public DbSet<Hat> Hats { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public override DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<OrderHat>()
            .HasOne<Order>(orderHat => orderHat.Order)
            .WithMany(order => order.Hats)
            .HasForeignKey(orderHat => orderHat.OrderId);

            builder.Entity<OrderHat>()
            .HasOne<Hat>(orderHat => orderHat.Hat)
            .WithMany(hat => hat.Orders)
            .HasForeignKey(orderHat => orderHat.HatId);

            builder.Entity("hatshop.Models.ApplicationUser", b =>
            {
                b.ToTable("Users");
            });
            builder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
            {
                b.ToTable("Roles");
            });
            builder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.ToTable("RoleClaims");
            });
            builder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.ToTable("UserClaims");
            });
            builder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.ToTable("UserLogins");
            });
            builder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.ToTable("UserRoles");
            });
            builder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.ToTable("UserTokens");
            });
        }
    }
}
