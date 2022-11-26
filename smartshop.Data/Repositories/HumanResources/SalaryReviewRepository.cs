namespace smartshop.Data.Repositories
{
    internal class SalaryReviewRepository : ISalaryReviewRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SalaryReviewRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Create(EmployeeSalaryReview employeeSalaryReview)
        {
            _dbContext.EmployeeSalaryReviews.Add(employeeSalaryReview);
            if (Save())
                return employeeSalaryReview.Id;

            throw new Exception("Employee salary has been not added for internal server error!");
        }

        public bool Exits(int businessId, int id)
        {
            return GetQuery(businessId).Any(s => s.Id == id);
        }

        public PaginationResponse<EmployeeSalaryReview> Get(int businessId, int currentPage, int pageSize, int? storeId)
        {
            var query = GetQuery(businessId, storeId);

            return new PaginationResponse<EmployeeSalaryReview>
            {
                TotalRows = query.Count(),
                Rows = query.Skip((currentPage - 1) * pageSize).Take(pageSize)
            };
        }

        public EmployeeSalaryReview? Get(int businessId, int id) 
            => GetQuery(businessId).SingleOrDefault(s => s.Id == id);

        public EmployeeSalaryReview? GetByEmployeeId(int businessId, int employeeId) 
            => GetQuery(businessId).SingleOrDefault(s => s.EmployeeId == employeeId && s.EndDate == null);

        public int Update(int businessId, int id, EmployeeSalaryReview employeeSalaryReview)
        {
            var salaryReview = GetQuery(businessId).SingleOrDefault(s => s.Id == id && s.EndDate == null);
            if(salaryReview == null)
                throw new Exception("Salary review not found for the id");

            salaryReview.BasicSalary = employeeSalaryReview.BasicSalary;
            salaryReview.HouseRent = employeeSalaryReview.HouseRent;
            salaryReview.Insurance = employeeSalaryReview.Insurance;
            salaryReview.MealAllowance = employeeSalaryReview.MealAllowance;
            salaryReview.MedicalAllowance = employeeSalaryReview.MedicalAllowance;
            salaryReview.ProvidenceFund = employeeSalaryReview.ProvidenceFund;
            salaryReview.Tax = employeeSalaryReview.Tax;
            salaryReview.TransportAllowance = employeeSalaryReview.TransportAllowance;

            _dbContext.Entry(salaryReview).State = EntityState.Modified;
            if (Save())
                return salaryReview.Id;

            throw new Exception("Salary review has been not updated!");
        }

        private IQueryable<EmployeeSalaryReview> GetQuery(int businessId, int? storeId = null) 
        {
            var query = _dbContext.EmployeeSalaryReviews
                .AsNoTracking()
                .Include(s => s.Employee)
                .ThenInclude(e => e.Store)
                .Include(s => s.Employee.User)
                .Include(s => s.Employee.Designation)
                .Where(s => s.Employee.Store.BusinessId == businessId)
                .AsQueryable();

            if(storeId != null)
                query.Include(s => s.Employee.StoreId == storeId.Value);

            return query;
        }
        private bool Save() 
            => _dbContext.SaveChanges() > 0;
    }
}
