using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using smartshop.Data.Configurations.RelationConfigures;
using smartshop.Data.Configurations.SeedingDataConfigurations;
using smartshop.Entities.Accounting;
using smartshop.Entities.Businesses;
using smartshop.Entities.Common;
using smartshop.Entities.PurchaseManagement;
using smartshop.Entities.SalesManagement;
using smartshop.Entities.Settings;
using smartshop.Entities.Stocks;

namespace smartshop.Data.DataContext
{
   
    internal class ApplicationDbContext : IdentityDbContext
    {
#pragma warning disable CS8618 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
#pragma warning restore CS8618
        {
        }
        // Business Entities->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        public DbSet<Business> Businesses { get; set; }
        public DbSet<BusinessDeactive> BusinessDeactives { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreDeactive> StoreDeactives { get; set; }
        public DbSet<BusinessConfigure> BusinessConfigures { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }

        // HumanResource Entities ->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        public DbSet<Designation> Designations { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<EmployeePersonalInfo> EmployeePersonalInfos { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeSalaryReview> EmployeeSalaryReviews { get; set; }

        // Accounting Entities ---->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        public DbSet<Head> Heads { get; set; }
        public DbSet<Investor> Investors { get; set; }
        public DbSet<InvestorTransaction> InvestorTransactions { get; set; }
        public DbSet<Account> Accounts { get; set; } 
        public DbSet<HeadTransaction> HeadTransactions { get; set; }
        public DbSet<AccountTransaction> AccountTransactions { get; set; }
        public DbSet<IncomeExpense> IncomeExpenses { get; set; }


        // Product Management Entities
        public DbSet<Unit> Units { get; set; }
        public DbSet<UnitVariation> UnitVariations { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        // Stocks Tables
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockDamage> StockDamages { get; set; }

        // Purchase Management Tables
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseProduct> PurchaseProducts { get; set; }
        public DbSet<PurchaseReturn> PurchaseReturns { get; set; }
        public DbSet<PurchaseReturnProduct> PurchaseReturnProducts { get; set; }
        public DbSet<DuePayment> DuePayments { get; set; }

        // Sales Management Tables
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleProduct> SaleProducts { get; set; }
        public DbSet<SaleReturn> SaleReturns { get; set; }
        public DbSet<SaleReturnProduct> SaleReturnProducts { get; set; }
        public DbSet<DueCollection> DueCollections { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.BusinessRelations();
            builder.HumanResourceRelations();
            builder.AccountingRelations();
            builder.ProductManagementRelations();
            builder.StockRelations();
            builder.PurchaseManagementRelations();
            builder.SeedData();
        }
    }
}
