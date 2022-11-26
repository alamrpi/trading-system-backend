using smartshop.Entities.Accounting;
using smartshop.Entities.SalesManagement;

namespace smartshop.Data.Configurations.RelationConfigures
{
    internal static class SalesManagementRelationConfigure
    {
        internal static void SalesManagementRelations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
             .HasOne(x => x.Head)
             .WithOne(x => x.Customer)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();
            
            modelBuilder.Entity<Sale>()
             .HasOne(x => x.Customer)
             .WithMany(x => x.Sales)
             .HasForeignKey(x => x.CustomerId)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();  
            
            modelBuilder.Entity<Sale>()
             .HasOne(x => x.Store)
             .WithMany(x => x.Sales)
             .HasForeignKey(x => x.StoreId)
             .OnDelete(DeleteBehavior.NoAction)
             .IsRequired(); 
            
            modelBuilder.Entity<Sale>()
             .HasOne(x => x.BankAccount)
             .WithMany(x => x.Sales)
             .HasForeignKey(x => x.BankAccountId)
             .OnDelete(DeleteBehavior.NoAction)
             .IsRequired();
            
            modelBuilder.Entity<SaleProduct>()
             .HasOne(x => x.Sale)
             .WithMany(x => x.SaleProducts)
             .HasForeignKey(x => x.SaleId)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired(); 
            
            modelBuilder.Entity<SaleProduct>()
             .HasOne(x => x.Stock)
             .WithMany(x => x.SaleProducts)
             .HasForeignKey(x => x.StockId)
             .OnDelete(DeleteBehavior.NoAction)
             .IsRequired();
             
            modelBuilder.Entity<SaleProduct>()
             .HasOne(x => x.UnitVariation)
             .WithMany(x => x.SaleProducts)
             .HasForeignKey(x => x.UnitVariationId)
             .OnDelete(DeleteBehavior.NoAction)
             .IsRequired(); 
            
            modelBuilder.Entity<SaleReturn>()
             .HasOne(x => x.Sale)
             .WithMany(x => x.SaleReturns) 
             .HasForeignKey(x => x.SaleId)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();
            
            modelBuilder.Entity<SaleReturnProduct>()
             .HasOne(x => x.SaleReturn)
             .WithMany(x => x.SaleReturnProducts) 
             .HasForeignKey(x => x.SaleReturnId)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();
            
            modelBuilder.Entity<SaleReturnProduct>()
             .HasOne(x => x.SaleProduct)
             .WithMany(x => x.SaleReturnProducts) 
             .HasForeignKey(x => x.SaleProductId)
             .OnDelete(DeleteBehavior.NoAction)
             .IsRequired(); 
            
            modelBuilder.Entity<DueCollection>()
             .HasOne(x => x.Sale)
             .WithMany(x => x.DueCollections) 
             .HasForeignKey(x => x.SaleId)
             .OnDelete(DeleteBehavior.NoAction)
             .IsRequired();
            
            modelBuilder.Entity<DueCollection>()
             .HasOne(x => x.BankAccount)
             .WithMany(x => x.DueCollections) 
             .HasForeignKey(x => x.BankAccountId)
             .OnDelete(DeleteBehavior.NoAction)
             .IsRequired();

        }
    }
}
