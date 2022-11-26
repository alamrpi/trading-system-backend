using smartshop.Entities.Accounting;
using smartshop.Entities.Businesses;
using smartshop.Entities.Settings;

namespace smartshop.Data.Configurations.RelationConfigures
{
    internal static class BusinessRelationConfigure
    {
        internal static void BusinessRelations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BusinessDeactive>()
               .HasOne(x => x.Business)
               .WithMany(x => x.BusinessDeactives)
               .HasForeignKey(x => x.BusinessId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();
            
            modelBuilder.Entity<Store>()
               .HasOne(x => x.Business)
               .WithMany(x => x.Stores)
               .HasForeignKey(x => x.BusinessId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();
            
            modelBuilder.Entity<StoreDeactive>()
               .HasOne(x => x.Store)
               .WithMany(x => x.StoreDeactives)
               .HasForeignKey(x => x.StoreId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();
              
            modelBuilder.Entity<BankAccount>()
               .HasOne(x => x.Head)
               .WithOne(x => x.BankAccount)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();
            
            modelBuilder.Entity<BusinessConfigure>()
               .HasOne(x => x.Business)
               .WithOne(x => x.BusinessConfigure)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();
        }
    }
}
