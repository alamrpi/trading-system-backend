using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using smartshop.Business.IServices.Additional;
using smartshop.Business.IServices.Auth;
using smartshop.Business.IServices.Businesses;
using smartshop.Business.IServices.HumanResources;
using smartshop.Business.IServices.Users;
using smartshop.Business.Service;
using smartshop.Business.Service.Accountant;
using smartshop.Business.Service.Additional;
using smartshop.Business.Service.Auth;
using smartshop.Business.Service.Businesses;
using smartshop.Business.Service.HumanResources;
using smartshop.Business.Service.Users;
using smartshop.Business.Services;
using smartshop.Common.Configs;
using smartshop.Data;

namespace smartshop.Business
{
    public static class ISeviceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataLayer(configuration);

            services.Configure<CloudinaryConfig>(configuration.GetSection("Cloudinary"));
            services.AddTransient<ICloudinaryService, CloudinaryService>();

            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IBusinessService, BusinessService>();
            services.AddTransient<IStoreService, StoreService>();
            services.AddTransient<IAdminService, AdminService>();

            //Hr Service Register 
            services.AddTransient<IDesignationService, DesignationService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<ISalaryReviewService, SalaryReviewService>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IUnitService, UnitService>();
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductService, ProductService>();

            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<IPurchaseService, PurchaseService>();
            services.AddTransient<IPurchaseReturnService, PurchaseReturnService>();

            services.AddTransient<IStockService, StockService>();

            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ISaleService, SaleService>();
            services.AddTransient<ISaleReturnService, SaleReturnService>();

            // Accountant Service register
            services.AddTransient<IBankAccountService, BankAccountService>();
            services.AddTransient<IDuePaymentService, DuePaymentService>();
            services.AddTransient<IDueCollectionService, DueCollectionService>();
            services.AddTransient<IInvestorService, InvestorService>();
            services.AddTransient<IInvestorTransactionService, InvestorTransactionService>();
            services.AddTransient<IHeadService, HeadService>();
            services.AddTransient<IIncomeExpenseService, IncomeExpenseService>();
            services.AddTransient<IHeadTransactionService, HeadTransactionService>();
            services.AddTransient<IAccountTransactionService, AccountTransactionService>();

            return services;
        }
    }
}
