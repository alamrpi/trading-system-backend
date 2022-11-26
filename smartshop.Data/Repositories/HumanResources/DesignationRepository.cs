namespace smartshop.Data.Repositories
{
    internal class DesignationRepository : IDesignationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DesignationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int? CreateOrUpdate(Designation designation)
        {
            if(designation.Id == 0)
                _dbContext.Designations.Add(designation);
            else
                _dbContext.Entry(designation).State = EntityState.Modified;

            _dbContext.SaveChanges();
            return designation.Id;
        }

        public void Delete(int businessId, int id)
        {
            var designation = _dbContext.Designations.AsNoTracking().FirstOrDefault(d => d.BusinessId == businessId && d.Id == id);
            _dbContext.Designations.Remove(designation);
            _dbContext.SaveChanges();
        }

        public bool Exits(int businessId, string name, int? id = null)
        {
            var designations = _dbContext.Designations
                .Where(x => x.BusinessId == businessId && x.Name.ToLower() == name.Trim().ToLower())
                .AsQueryable();

            if(id != null)
                designations = designations.Where(x => x.Id != id);

            return designations.Any();
        }

        public bool Exits(int businessId, int id)
            => _dbContext.Designations.Any(d => d.BusinessId == businessId && d.Id == id);

        public IEnumerable<Designation> Get(int businessId) 
            => _dbContext.Designations.Where(des => des.BusinessId == businessId).OrderBy(x => x.Priority).ToList();

        public PaginationResponse<Designation> Get(int businessId, int currentPage, int pageSize)
        {
            var designations = _dbContext.Designations.Where(d => d.BusinessId == businessId).OrderBy(x => x.Priority).AsQueryable();

            return new PaginationResponse<Designation>()
            {
                Rows = designations.Skip((currentPage - 1) * pageSize).Take(pageSize),
                TotalRows = designations.Count()
            };
        }

        public Designation? Get(int businessId, int id)
        {
            return _dbContext.Designations.SingleOrDefault(d => d.BusinessId == businessId && d.Id == id);
        }
    }
}
