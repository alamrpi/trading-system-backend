using smartshop.Common.Enums.Accounting;
using smartshop.Entities.SalesManagement;

namespace smartshop.Data.Repositories
{
    internal class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public int Create(Customer entity)
        {
            _dbContext.Customers.Add(entity);
            Save();
            return entity.Id;
        }

        public void Delete(int id)
        {
            var entity = GetCustomer().Single(b => b.Id == id);
            _dbContext.Customers.Remove(entity);
            Save();
        }

        public bool Exists(int businessId, int id)
            => GetCustomer(businessId).Any(x => x.Id == id);

        public bool Exists(int businessId, string name, int? id = null)
        {
            var query = GetCustomer(businessId);
            if (id != null) query = query.Where(b => b.Id != id);

            return query.Any(b => b.Head.Name.Trim().ToLower() == name.Trim().ToLower() && b.Head.HeadType == HeadTypes.Customer);
        }

        public PaginationResponse<Customer> Get(int businessId, int currentPage, int pageSize)
        {
            var customers = GetCustomer(businessId);
            return new PaginationResponse<Customer>
            {
                Rows = customers.Skip((currentPage - 1) * pageSize).Take(pageSize),
                TotalRows = customers.Count()
            };
        }

        public Customer? Get(int businessId, int id)
            => GetCustomer(businessId).FirstOrDefault(b => b.Id == id);

        public IEnumerable<Customer> Get(int businessId)
            => GetCustomer(businessId).ToList();

        public int TotalCount(int businessId)
            => GetCustomer(businessId).Count();

        public void Update(int id, Customer entity)
        {
            var customer = GetCustomer().SingleOrDefault(x => x.Id == id);
            customer.Head.Name = entity.Head.Name;
            customer.Email = entity.Email;
            customer.Mobile = entity.Mobile;
            customer.Address = entity.Address;
            customer.Type = entity.Type;
            customer.Head.Descriptions = entity.Head.Descriptions;
            _dbContext.Entry(customer).State = EntityState.Modified;
            Save();
        }

        private IQueryable<Customer> GetCustomer(int? businessId = null)
        {
            var query = _dbContext.Customers
                .Include(s => s.Head)
                .AsQueryable();

            if (businessId != null) query = query.Where(x => x.Head.BusinessId == businessId);

            return query;
        }

        private bool Save()
        {
            return _dbContext.SaveChanges() > 0;
        }
    }
}
