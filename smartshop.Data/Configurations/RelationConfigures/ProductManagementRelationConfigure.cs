namespace smartshop.Data.Configurations.RelationConfigures
{
    internal static class ProductManagementRelationConfigure
    {
        internal static void ProductManagementRelations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Unit>()
                .HasOne(x => x.Business)
                .WithMany(x => x.Units)
                .HasForeignKey(x => x.BusinessId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(); 
            
            modelBuilder.Entity<UnitVariation>()
                .HasOne(x => x.Unit)
                .WithMany(x => x.UnitVariations)
                .HasForeignKey(x => x.UnitId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            
            modelBuilder.Entity<Brand>()
                .HasOne(x => x.Business)
                .WithMany(x => x.Brands)
                .HasForeignKey(x => x.BusinessId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();  
            
            modelBuilder.Entity<Group>()
                .HasOne(x => x.Business)
                .WithMany(x => x.Groups)
                .HasForeignKey(x => x.BusinessId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            
            modelBuilder.Entity<Category>()
                .HasOne(x => x.Group)
                .WithMany(x => x.Categories)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();   

            modelBuilder.Entity<Product>()
                .HasOne(x => x.Head)
                .WithOne(x => x.Product)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(); 
            
            modelBuilder.Entity<Product>()
                .HasOne(x => x.Brand)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(); 
            
            modelBuilder.Entity<Product>()
                .HasOne(x => x.Unit)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.UnitId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
