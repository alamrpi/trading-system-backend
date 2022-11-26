using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using smartshop.Data.IRepositories.Accountant;
using smartshop.Data.IRepositories.Auth;
using smartshop.Data.IRepositories.Businesses;
using smartshop.Data.IRepositories.Users;
using smartshop.Data.Repositories.Accountant;
using smartshop.Data.Repositories.Auth;
using smartshop.Data.Repositories.Businesses;
using smartshop.Data.Repositories.Users;
using smartshop.Entities.Common;

namespace smartshop.Data
{
    public static class ISeviceCollectionExtensions
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseMySql(configuration.GetConnectionString("DefaultConnection").ToString(), ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();

            //Business related repository register
            services.AddTransient<IBusinessRepository, BusinessRepository>();
            services.AddTransient<IStoreRepository, StoreRepository>();
            services.AddTransient<IAdminRepository, AdminRepository>();

            /// HR related repository register
            services.AddScoped<IDesignationRepository, DesignationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<ISalaryReviewRepository, SalaryReviewRepository>();

            /// Product Management repository register
            services.AddTransient<IUnitRepository, UnitRepository>();
            services.AddTransient<IBrandRepository, BrandRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();

            /// Purchase Management repostiory register
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddTransient<IPurchaseRepository, PurchaseRepository>();
            services.AddTransient<IPurchaseReturnRepository, PurchaseReturnRepository>();

            /// Stock Management repository register
            services.AddTransient<IStockRepository, StockRepository>();

            /// sales Management repository register
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ISaleRepository, SaleRepository>();
            services.AddTransient<ISalesReturnRepository, SalesReturnRepository>();

            /// Accountant Repository 
            services.AddTransient<IBankAccountRepsitory, BankAccountRepsitory>();
            services.AddTransient<IDuePaymentRepository, DuePaymentRepository>();
            services.AddTransient<IDueCollectionRepository, DueCollectionRepository>();
            services.AddTransient<IInvestorRepository, InvestorRepository>();
            services.AddTransient<IInvestorTransactionRepository, InvestorTransactionRepository>();
            services.AddTransient<IHeadRepository, HeadRepository>();
            services.AddTransient<IHeadTransactionRepository, HeadTransactionRepository>();
            services.AddTransient<IIncomeExpenseRepository, IncomeExpenseRepository>();
            services.AddTransient<IAccountTransactionRepository, AccountTransactionRepository>();
            return services;
        }
    }
}
