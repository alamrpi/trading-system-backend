using smartshop.Common.Enums.Accounting;

namespace smartshop.Data.Repositories
{
    internal class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SupplierRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public int Create(Supplier entity)
        {
            _dbContext.Suppliers.Add(entity);
            Save();
            return entity.Id;
        }

        public void Delete(int id)
        {
            var entity = GetSuppliers().Single(b => b.Id == id);
            _dbContext.Suppliers.Remove(entity);
            Save();
        }

        public bool Exists(int businessId, int id)
            => GetSuppliers(businessId).Any(x => x.Id == id);

        public bool Exists(int businessId, string name, int? id = null)
        {
            var query = GetSuppliers(businessId);
            if (id != null) query = query.Where(b => b.Id != id);

            return query.Any(b => b.Head.Name.Trim().ToLower() == name.Trim().ToLower() && b.Head.HeadType == HeadTypes.Supplier);
        }

        public PaginationResponse<Supplier> Get(int businessId, int currentPage, int pageSize)
        {
            var suppliers = GetSuppliers(businessId);
            return new PaginationResponse<Supplier>
            {
                Rows = suppliers.Skip((currentPage - 1) * pageSize).Take(pageSize),
                TotalRows = suppliers.Count()
            };
        }

        public Supplier? Get(int businessId, int id)
            => GetSuppliers(businessId).FirstOrDefault(b => b.Id == id);

        public IEnumerable<Supplier> Get(int businessId)
            => GetSuppliers(businessId).ToList();

        public int TotalCount(int businessId) 
            => GetSuppliers(businessId).Count();

        public void Update(int id, Supplier entity)
        {
            var supplier = GetSuppliers().SingleOrDefault(x => x.Id == id);
            supplier.Head.Name = entity.Head.Name;
            supplier.Email = entity.Email;
            supplier.Mobile = entity.Mobile;
            supplier.Address = entity.Address;
            supplier.Head.Descriptions = entity.Head.Descriptions;
            _dbContext.Entry(supplier).State = EntityState.Modified;
            Save();
        }

        private IQueryable<Supplier> GetSuppliers(int? businessId = null)
        {
            var query = _dbContext.Suppliers
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
