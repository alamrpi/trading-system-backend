namespace smartshop.Data.Repositories
{
    internal class UnitRepository : IUnitRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UnitRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public int Create(Unit entity)
        {
            _applicationDbContext.Units.Add(entity);
           _ = Save();
            return entity.Id;
        }

        public void CreateUnitVariation(List<UnitVariation> variations)
        {
            _applicationDbContext.UnitVariations.AddRange(variations);
            _ = Save();
        }

        public void Delete(int id)
        {
            var unit = Query().FirstOrDefault(u => u.Id == id);
            _applicationDbContext.Units.Remove(unit);
            Save();
        }

        public void DeleteUnitVariation(int variationId)
        {
           var variations = _applicationDbContext.UnitVariations.Where(v => v.Id == variationId).FirstOrDefault();
            _applicationDbContext.UnitVariations.Remove(variations);
            Save();
        }

        public bool Exists(int businessId, int id) 
            => Query(businessId).AsNoTracking().Any(x => x.Id == id);

        public bool Exists(int businessId, string name, int? id = null)
        {
            var query = Query(businessId).AsNoTracking();
            if(id != null) query = query.Where(x => x.Id != id);

            return query.Any(x => x.Name.ToLower() == name.Trim().ToLower());
        }

        public PaginationResponse<Unit> Get(int businessId, int currentPage, int pageSize)
        {
            var query = Query(businessId);

            return new PaginationResponse<Unit>
            {
                Rows = query.Skip((currentPage - 1) * pageSize).Take(pageSize),
                TotalRows = query.Count()
            };
        }

        public Unit? Get(int businessId, int id) 
            => Query(businessId).AsNoTracking().SingleOrDefault(x => x.Id == id);

        public IEnumerable<Unit> Get(int businessId)
        {
            return Query(businessId);
        }

        public IEnumerable<UnitVariation> GetVariations(int businessId, int unitId)
        {
            return _applicationDbContext.UnitVariations
                .Include(uv => uv.Unit)
                .Where(uv => uv.Unit.BusinessId == businessId && uv.UnitId == unitId)
                .ToList();

        }

        public void Update(int id, Unit entity)
        {
            var unit = Query().SingleOrDefault(x => x.Id == id);
            if (unit == null)
                throw new Exception("unit has been not found by this id.");

            unit.Name = entity.Name;
            unit.Symbol = entity.Symbol;
            unit.Comments = entity.Comments;
           _applicationDbContext.Entry(unit).State = EntityState.Modified;

            Save();
        }

        private IQueryable<Unit> Query(int? businessId = null)
        {
            var query = _applicationDbContext.Units
                .Include(x => x.UnitVariations)
                .AsQueryable();

            if (businessId != null) query = query.Where(x => x.BusinessId == businessId);

            return query;
        }

        private bool Save() 
            => _applicationDbContext.SaveChanges() > 0;
    }
}
