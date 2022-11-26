using Microsoft.AspNetCore.Identity;
using smartshop.Common.Constants;
using smartshop.Entities.Businesses;
using smartshop.Entities.Common;

namespace smartshop.Data.Configurations.SeedingDataConfigurations
{
    internal static class DefaultSeeder
    {
        internal static void SeedData(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Business>().HasData(new Business("Super Admin", "12345678912", "sunit@gmail.com", "N/A", "N/A")
            {
                Id = 1,
            });

            modelBuilder.Entity<Designation>().HasData(new Designation(1, "Admin")
            {
                Id = 1
            });

            var hasher = new PasswordHasher<ApplicationUser>();

            var user = new ApplicationUser("Super", "Admin", "admin@gmail.com", "admin@gmail.com", "01740857126", true)
            {
                Id = "6c3957d2-86ca-4ed9-86a4-5a87dcbcc71a",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true,
            };

            user.PasswordHash = hasher.HashPassword(user, "SuperAdmin1!");
            user.NormalizedUserName = user.UserName.ToUpper();
            user.NormalizedEmail = user.Email.ToUpper();

            //Add Admin User
            modelBuilder.Entity<ApplicationUser>().HasData(user);
            modelBuilder.Entity<IdentityRole>().HasData(new List<IdentityRole>
            {
                new IdentityRole { Id = "af6fa21b-ee6a-4381-a607-fafd97817cd5", Name = Roles.SUPER_ADMIN, NormalizedName = Roles.SUPER_ADMIN.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() },
                new IdentityRole { Id = "ef9acd24-7668-4fa2-a17a-a901cacf2bc5", Name = Roles.ADMIN, NormalizedName = Roles.ADMIN.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Roles.ACCOUNTANT, NormalizedName = Roles.ACCOUNTANT.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Roles.PURCHASE_MANAGEMENT, NormalizedName = Roles.PURCHASE_MANAGEMENT.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Roles.STOCK_INVENTORY, NormalizedName = Roles.STOCK_INVENTORY.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Roles.PRODUCT_MANAGEMENT, NormalizedName = Roles.PRODUCT_MANAGEMENT.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Roles.SALES_MANAGEMENT, NormalizedName = Roles.SALES_MANAGEMENT.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Roles.HR_MANAGEMENT, NormalizedName = Roles.HR_MANAGEMENT.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() },
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "af6fa21b-ee6a-4381-a607-fafd97817cd5",
                UserId = "6c3957d2-86ca-4ed9-86a4-5a87dcbcc71a"
            });
            modelBuilder.Entity<Store>().HasData(new Store(1, "Super Store", "017xxxxxxxx", "test@gmail.com", "Test Address", "25")
            {
                Id = 1
            });
            modelBuilder.Entity<Employee>().HasData(new Employee("6c3957d2-86ca-4ed9-86a4-5a87dcbcc71a", 1, 1, new DateTime(), ";;", "6c3957d2-86ca-4ed9-86a4-5a87dcbcc71a")
            {
                Id = 1
            });
        }
    }
}
