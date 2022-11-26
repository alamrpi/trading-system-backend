namespace smartshop.Data.Repositories
{
    internal class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BrandRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public int Create(Brand entity)
        {
            _dbContext.Brands.Add(entity);
            Save();
            return entity.Id;
        }

        public void Delete(int id)
        {
           var entity = GetBrands().SingleOrDefault(b => b.Id == id);
            _dbContext.Brands.Remove(entity);
            Save();
        }

        public bool Exists(int businessId, int id) 
            => GetBrands(businessId).Any(x => x.Id == id);

        public bool Exists(int businessId, string name, int? id = null)
        {
            var query = GetBrands(businessId);
            if(id != null) query = query.Where(b => b.Id != id);

            return query.Any(b => b.Name.Trim().ToLower() == name.Trim().ToLower());
        }

        public PaginationResponse<Brand> Get(int businessId, int currentPage, int pageSize)
        {
            var brands = GetBrands(businessId);
            return new PaginationResponse<Brand>
            {
                Rows = brands.Skip((currentPage - 1) * pageSize).Take(pageSize),
                TotalRows = brands.Count()
            };
        }

        public Brand? Get(int businessId, int id)
            => GetBrands(businessId).FirstOrDefault(b => b.Id == id);

        public IEnumerable<Brand> Get(int businessId) 
            => GetBrands(businessId).ToList();

        public void Update(int id, Brand entity)
        {
            var brand = GetBrands().SingleOrDefault(x => x.Id == id);
            brand.Name = entity.Name;
            brand.Comments = entity.Comments;
            _dbContext.Entry(brand).State = EntityState.Modified;
            Save();
        }

        private IQueryable<Brand> GetBrands(int? businessId = null)
        {
            var query = _dbContext.Brands
                .AsQueryable();

            if(businessId != null) query = query.Where(x => x.BusinessId == businessId);

            return query;
        }

        private bool Save()
        {
            return _dbContext.SaveChanges() > 0;
        }
    }
}
