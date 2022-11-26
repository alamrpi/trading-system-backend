
using smartshop.Common.QueryParams;

namespace smartshop.Data.Repositories
{
    internal class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public int Create(Product entity)
        {
            _dbContext.Products.Add(entity);
            Save();
            return entity.Id;
        }

        public void Delete(int id)
        {
            var entity = GetProducts().AsNoTracking().SingleOrDefault(b => b.Id == id);
            _dbContext.Heads.Remove(entity.Head);
            _dbContext.Products.Remove(entity);
            Save();
        }

        public bool Exists(int businessId, int id)
            => GetProducts(businessId).Any(x => x.Id == id);

        public bool Exists(int businessId, string name, int? id = null)
        {
            var query = GetProducts(businessId);
            if (id != null) query = query.Where(b => b.Id != id);

            return query.Any(b => b.Head.Name.Trim().ToLower() == name.Trim().ToLower());
        }

        public PaginationResponse<Product> Get(int businessId, int currentPage, int pageSize, ProductQueryParams queryParams)
        {
            var categories = FilterProducts(businessId, queryParams);

            return new PaginationResponse<Product>
            {
                Rows = categories.Skip((currentPage - 1) * pageSize).Take(pageSize),
                TotalRows = categories.Count()
            };
        }

        public Product? Get(int businessId, int id)
            => GetProducts(businessId).FirstOrDefault(b => b.Id == id);

        public IEnumerable<Product> Get(int businessId, ProductQueryParams queryParams)
        {
            return FilterProducts(businessId, queryParams).ToList();
        }

        public void Update(int id, Product entity)
        {
            var products = GetProducts().SingleOrDefault(x => x.Id == id);
            products.Head.Name = entity.Head.Name;
            products.CategoryId = entity.CategoryId;
            products.UnitId = entity.UnitId;
            products.BrandId = entity.BrandId;
            products.AlertQty = entity.AlertQty;
            products.Descriptions = entity.Descriptions;

            _dbContext.Entry(products).State = EntityState.Modified;
            Save();
        }

        private IQueryable<Product> FilterProducts(int businessId, ProductQueryParams queryParams)
        {
            var query = GetProducts(businessId);

            if(queryParams.GroupId != null) query = query.Where(p => p.Category.GroupId == queryParams.GroupId);

            if (queryParams.CategoryId != null) query = query.Where(p => p.CategoryId == queryParams.CategoryId);

            if (queryParams.BrandId != null) query = query.Where(p => p.BrandId == queryParams.BrandId);

            if (queryParams.UnitId != null) query = query.Where(p => p.UnitId == queryParams.UnitId);


            return query;
        }

        private IQueryable<Product> GetProducts(int? businessId = null)
        {
            var query = _dbContext.Products
                .Include(p => p.Category)
                .ThenInclude(c => c.Group)
                .Include(p => p.Brand)
                .Include(p => p.Unit)
                .Include(p => p.Head)
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
