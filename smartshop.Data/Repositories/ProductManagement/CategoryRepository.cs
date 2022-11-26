namespace smartshop.Data.Repositories
{
    internal class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public int Create(Category entity)
        {
            _dbContext.Categories.Add(entity);
            Save();
            return entity.Id;
        }

        public void Delete(int id)
        {
            var entity = GetCategories().AsNoTracking().SingleOrDefault(b => b.Id == id);
            _dbContext.Categories.Remove(entity);
            Save();
        }

        public bool Exists(int businessId, int id)
            => GetCategories(businessId).Any(x => x.Id == id);

        public bool Exists(int businessId, string name, int? id = null)
        {
            var query = GetCategories(businessId);
            if (id != null) query = query.Where(b => b.Id != id);

            return query.Any(b => b.Name.Trim().ToLower() == name.Trim().ToLower());
        }

        public PaginationResponse<Category> Get(int businessId, int currentPage, int pageSize)
        {
            var categories = GetCategories(businessId);
            return new PaginationResponse<Category>
            {
                Rows = categories.Skip((currentPage - 1) * pageSize).Take(pageSize),
                TotalRows = categories.Count()
            };
        }

        public Category? Get(int businessId, int id)
            => GetCategories(businessId).FirstOrDefault(b => b.Id == id);

        public IEnumerable<Category> Get(int businessId)
          => GetCategories(businessId).ToList();

        public IEnumerable<Category> GetDdl(int businessId, int groupId)
            => GetCategories(businessId).Where(x => x.GroupId == groupId).ToList();

        public void Update(int id, Category entity)
        {
            var categories = GetCategories().SingleOrDefault(x => x.Id == id);
            categories.Name = entity.Name;
            categories.GroupId = entity.GroupId;

            _dbContext.Entry(categories).State = EntityState.Modified;
            Save();
        }

        private IQueryable<Category> GetCategories(int? businessId = null)
        {
            var query = _dbContext.Categories
                .Include(c => c.Group)
                .AsQueryable();

            if (businessId != null) query = query.Where(x => x.Group.BusinessId == businessId);

            return query;
        }

        private bool Save()
        {
            return _dbContext.SaveChanges() > 0;
        }
    }
}
