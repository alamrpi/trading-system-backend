using Microsoft.AspNetCore.Identity;
using smartshop.Data.IRepositories.Users;
using smartshop.Entities.Common;

namespace smartshop.Data.Repositories.Users
{
    internal class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminRepository(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            this._applicationDbContext = applicationDbContext;
            this._userManager = userManager;
        }

        public bool AnyAdminByBusiness(int storeId)
        {
            var businessId = _applicationDbContext.Stores.Find(storeId).BusinessId;

            return _applicationDbContext.Employees.AsNoTracking().Any(x => x.Store.BusinessId == businessId);
        }

        public ApplicationUser Create(ApplicationUser entity)
        {
            var result = _userManager.CreateAsync(entity, "Admin123!").Result;
          
            if(result.Succeeded)
            {
                _ = _userManager.AddToRoleAsync(entity, "Admin");
                return entity;
            }
            throw new Exception("Business Admin has been not added!");
        }

        public ApplicationUser? Get(string id) 
            => Query().FirstOrDefault(x => x.Id == id);

        public Employee? GetEmployee(string id)
        {
            return _applicationDbContext.Employees.AsNoTracking()
                .Include(x => x.Store)
                .ThenInclude(x => x.Business)
                .FirstOrDefault(x => x.ResignDate == null && x.UserId == id);
        }

        public IEnumerable<ApplicationUser> Gets(int page, int pageSize) 
            => Query().Skip((page - 1) * pageSize).Take(pageSize);

        public int TotalCount() 
            => _applicationDbContext.ApplicationUsers.Where(x => x.IsAdmin).Count();

        public void Update(ApplicationUser entity)
        {
            _applicationDbContext.Entry(entity).State = EntityState.Modified;
            if (!Save())
                throw new Exception("Business Admin has been not updated for internal server error.");
        }

        protected IQueryable<ApplicationUser> Query()
        {
            return _applicationDbContext.ApplicationUsers.AsNoTracking()
                .Include(x => x.Employees)
                .ThenInclude(e => e.Store)
                .ThenInclude(s => s.Business)
                .Where(x => x.IsAdmin)
                .Where(x => x.Id != "6c3957d2-86ca-4ed9-86a4-5a87dcbcc71a").AsNoTracking();
        }
        protected bool Save() 
            => _applicationDbContext.SaveChanges() > 0;
    }
}
