namespace smartshop.Data.Repositories
{
    internal class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GroupRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public int Create(Group entity)
        {
            _dbContext.Groups.Add(entity);
            Save();
            return entity.Id;
        }

        public void Delete(int id)
        {
           var entity = GetGroups().SingleOrDefault(b => b.Id == id);
            _dbContext.Groups.Remove(entity);
            Save();
        }

        public bool Exists(int businessId, int id) 
            => GetGroups(businessId).Any(x => x.Id == id);

        public bool Exists(int businessId, string name, int? id = null)
        {
            var query = GetGroups(businessId);
            if(id != null) query = query.Where(b => b.Id != id);

            return query.Any(b => b.Name.Trim().ToLower() == name.Trim().ToLower());
        }

        public PaginationResponse<Group> Get(int businessId, int currentPage, int pageSize)
        {
            var brands = GetGroups(businessId);
            return new PaginationResponse<Group>
            {
                Rows = brands.Skip((currentPage - 1) * pageSize).Take(pageSize),
                TotalRows = brands.Count()
            };
        }

        public Group? Get(int businessId, int id)
            => GetGroups(businessId).FirstOrDefault(b => b.Id == id);

        public IEnumerable<Group> Get(int businessId) 
            => GetGroups(businessId).ToList();

        public void Update(int id, Group entity)
        {
            var group = GetGroups().SingleOrDefault(x => x.Id == id);
            group.Name = entity.Name;
            group.Comments = entity.Comments;
            _dbContext.Entry(group).State = EntityState.Modified;
            Save();
        }

        private IQueryable<Group> GetGroups(int? businessId = null)
        {
            var query = _dbContext.Groups
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
