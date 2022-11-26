namespace smartshop.Data.Configurations.RelationConfigures
{
    internal static class HumanResourceRelationConfigure
    {
        internal static void HumanResourceRelations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Designation>()
               .HasOne(x => x.Business)
               .WithMany(x => x.Designations)
               .HasForeignKey(x => x.BusinessId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();
            
            modelBuilder.Entity<EmployeePersonalInfo>()
               .HasOne(x => x.ApplicationUser)
               .WithOne(x => x.EmployeePersonalInfo)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();
            
            modelBuilder.Entity<Employee>()
               .HasOne(x => x.User)
               .WithMany(x => x.Employees)
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired(); 
            
            modelBuilder.Entity<Employee>()
               .HasOne(x => x.Store)
               .WithMany(x => x.Employees)
               .HasForeignKey(e => e.StoreId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired(); 
            
            modelBuilder.Entity<Employee>()
               .HasOne(x => x.Designation)
               .WithMany(x => x.Employees)
               .HasForeignKey(e => e.DesignationId)
               .OnDelete(DeleteBehavior.NoAction)
               .IsRequired();
            
            modelBuilder.Entity<EmployeeSalaryReview>()
               .HasOne(x => x.Employee)
               .WithMany(x => x.EmployeeSalaryReviews)
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();
        }
    }
}
