using smartshop.Entities;
using smartshop.Entities.Accounting;
using smartshop.Entities.PurchaseManagement;

namespace smartshop.Data.Configurations.RelationConfigures
{
    internal static class PurchaseManagementRelationConfigure
    {
        internal static void PurchaseManagementRelations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Supplier>()
             .HasOne(x => x.Head)
             .WithOne(x => x.Supplier)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();

            modelBuilder.Entity<Purchase>()
              .HasOne(x => x.Store)
              .WithMany(x => x.Purchases)
              .HasForeignKey(x => x.StoreId)
              .OnDelete(DeleteBehavior.Cascade)
              .IsRequired();
            
            modelBuilder.Entity<Purchase>()
              .HasOne(x => x.Supplier)
              .WithMany(x => x.Purchases)
              .HasForeignKey(x => x.SupplierId)
              .OnDelete(DeleteBehavior.NoAction)
              .IsRequired();
            
            modelBuilder.Entity<Purchase>()
              .HasOne(x => x.BankAccount)
              .WithMany(x => x.Purchases)
              .HasForeignKey(x => x.BankAccountId)
              .OnDelete(DeleteBehavior.NoAction)
              .IsRequired();  
            
            modelBuilder.Entity<PurchaseProduct>()
              .HasOne(x => x.Purchase)
              .WithMany(x => x.PurchaseProducts)
              .HasForeignKey(x => x.PurchaseId)
              .OnDelete(DeleteBehavior.Cascade)
              .IsRequired();  
            
            modelBuilder.Entity<PurchaseProduct>()
              .HasOne(x => x.Product)
              .WithMany(x => x.PurchaseProducts)
              .HasForeignKey(x => x.ProductId)
              .OnDelete(DeleteBehavior.NoAction)
              .IsRequired();

            modelBuilder.Entity<PurchaseProduct>()
              .HasOne(x => x.UnitVariation)
              .WithMany(x => x.PurchaseProducts)
              .HasForeignKey(x => x.UnitVariationId)
              .OnDelete(DeleteBehavior.NoAction)
              .IsRequired();
            
            modelBuilder.Entity<PurchaseReturn>()
              .HasOne(x => x.Purchase)
              .WithMany(x => x.PurchaseReturns)
              .HasForeignKey(x => x.PurchaseId)
              .OnDelete(DeleteBehavior.Cascade)
              .IsRequired();
            
            modelBuilder.Entity<PurchaseReturnProduct>()
              .HasOne(x => x.PurchaseReturn)
              .WithMany(x => x.PurchaseReturnProducts)
              .HasForeignKey(x => x.PurchaseReturnId)
              .OnDelete(DeleteBehavior.Cascade)
              .IsRequired();
              
            modelBuilder.Entity<PurchaseReturnProduct>()
              .HasOne(x => x.PurchaseProduct)
              .WithMany(x => x.PurchaseReturnProducts)
              .HasForeignKey(x => x.PurchaseProductId)
              .OnDelete(DeleteBehavior.NoAction)
              .IsRequired(); 
            
            modelBuilder.Entity<DuePayment>()
              .HasOne(x => x.Purchase)
              .WithMany(x => x.DuePayments)
              .HasForeignKey(x => x.PurchaseId)
              .OnDelete(DeleteBehavior.Cascade)
              .IsRequired();


        }
    }
}
