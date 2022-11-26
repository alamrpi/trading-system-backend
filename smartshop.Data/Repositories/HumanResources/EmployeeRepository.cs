namespace smartshop.Data.Repositories
{
    internal class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EmployeeRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public bool ChangeStatus(int businessId, int id, bool status)
        {
            var employee = GetQuery(businessId).FirstOrDefault(e => e.Id == id);
            if(employee == null)
                return false;

            var user = employee.User;
            user.IsActive = status;

            _applicationDbContext.Entry(user).State = EntityState.Modified;
            Save();
            return true;
        }

        public int Create(Employee employee)
        {
            var oldEmployee = _applicationDbContext.Employees.FirstOrDefault(x => x.UserId == employee.UserId && x.ResignDate == null);
            if(oldEmployee != null)
            {
                oldEmployee.ResignDate = employee.JoiningDate;
                _applicationDbContext.Entry(oldEmployee).State = EntityState.Modified;
            }
            _applicationDbContext.Employees.Add(employee);
            Save();
            return employee.Id;
        }

        public string Create(EmployeePersonalInfo personalInfo)
        {
            _applicationDbContext.EmployeePersonalInfos.Add(personalInfo);
            Save();
            return personalInfo.Id;
        }

        public bool Exists(int businessId, int id) 
            => GetQuery(businessId).Any(e => e.Id == id);

        public Employee? Get(int businessId, int id) 
            => GetQuery(businessId).FirstOrDefault(e => e.Id == id);

        public IEnumerable<Employee> Get(int businessId, int? storeId = null) 
            => GetQuery(businessId, storeId).ToList();

        public PaginationResponse<Employee> Get(int businessId, int currentPage, int pageSize, int? storeId = null, bool isCurrent = true)
        {
            var query = GetQuery(businessId, storeId, isCurrent);

            return new PaginationResponse<Employee>
            {
                TotalRows = query.Count(),
                Rows = query.Skip((currentPage - 1) * pageSize).Take(pageSize)
            };
        }

        public int Update(Employee entity)
        {
            var employee = _applicationDbContext.Employees.SingleOrDefault(e => e.Id == entity.Id);
            employee.Id = entity.Id;
            employee.StoreId = entity.StoreId;
            employee.DesignationId = entity.DesignationId;
            employee.JoiningDate = entity.JoiningDate;

            var oldEmployee = _applicationDbContext.Employees.FirstOrDefault(e => e.UserId == employee.UserId && e.ResignDate.Value.Date == employee.JoiningDate.Date);
            if(oldEmployee != null)
            {
                oldEmployee.ResignDate = entity.JoiningDate;
                _applicationDbContext.Entry(oldEmployee).State = EntityState.Modified;
            }

            _applicationDbContext.Entry(employee).State = EntityState.Modified;
            Save();
            return employee.Id;
        }

        public string Update(string userId, EmployeePersonalInfo employeePersonalInfo)
        {
            var personalInfo = _applicationDbContext.EmployeePersonalInfos.Include(pi => pi.ApplicationUser).SingleOrDefault(ep => ep.Id == userId);

            personalInfo.Gender = employeePersonalInfo.Gender;
            personalInfo.FatherName = employeePersonalInfo.FatherName;
            personalInfo.DateOfBirth = employeePersonalInfo.DateOfBirth;
            personalInfo.Address = employeePersonalInfo.Address;
            personalInfo.ApplicationUser.FirstName = employeePersonalInfo.ApplicationUser.FirstName;
            personalInfo.ApplicationUser.LastName = employeePersonalInfo.ApplicationUser.LastName;
            personalInfo.ApplicationUser.Email = employeePersonalInfo?.ApplicationUser.Email;
            personalInfo.ApplicationUser.NormalizedEmail = employeePersonalInfo?.ApplicationUser.Email.ToUpper();
            personalInfo.ApplicationUser.PhoneNumber = employeePersonalInfo?.ApplicationUser.PhoneNumber;
            personalInfo.ApplicationUser.UserName = employeePersonalInfo?.ApplicationUser.Email;
            personalInfo.ApplicationUser.NormalizedUserName = employeePersonalInfo.ApplicationUser?.Email.ToUpper();

            _applicationDbContext.Entry(personalInfo).State = EntityState.Modified;
            Save();
            return employeePersonalInfo.Id;
        }

        private IQueryable<Employee> GetQuery(int businessId, int? storeId = null, bool isCurrent = true)
        {
            var query = _applicationDbContext.Employees
                .Include(e => e.User)
                .ThenInclude(u => u.EmployeePersonalInfo)
                .Include(e => e.User.Employees)
                .ThenInclude(e => e.Store)
                .Include(e => e.User.Employees)
                .ThenInclude(e => e.Designation)
                .Include(e => e.Store)
                .ThenInclude(s => s.Business)
                 .Include(e => e.Designation)
                .Where(e => e.Store.BusinessId == businessId)
                .Where(e => !e.User.IsAdmin)
                .AsQueryable();

            if (storeId != null)
                query = query.Where(e => e.StoreId == storeId);
            if (isCurrent)
                query = query.Where(e => e.ResignDate == null);

            return query;
        }

        private void Save() 
            => _applicationDbContext.SaveChanges();
    }
}
