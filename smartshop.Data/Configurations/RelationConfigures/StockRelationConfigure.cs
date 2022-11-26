using smartshop.Entities.Stocks;

namespace smartshop.Data.Configurations.RelationConfigures
{
    internal static class StockRelationConfigure
    {
        internal static void StockRelations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>()
             .HasOne(x => x.Store)
             .WithMany(x => x.Stocks)
             .HasForeignKey(x => x.StoreId)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();
            
            modelBuilder.Entity<StockDamage>()
             .HasOne(x => x.Stock)
             .WithMany(x => x.StockDamages)
             .HasForeignKey(x => x.StockId)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();
        }
    }
}
