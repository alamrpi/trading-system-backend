using smartshop.Data.IRepositories.Businesses;
using smartshop.Entities.Businesses;

namespace smartshop.Data.Repositories.Businesses
{
    internal class StoreRepository : Repository<Store>, IStoreRepository
    {
        public StoreRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public void Active(int id)
        {
            var entity = _databaseContext.StoreDeactives.AsNoTracking().FirstOrDefault(x => x.ReActivateDate == null);

            if (entity != null)
            {
                entity.ReActivateDate = DateTime.UtcNow;
                _databaseContext.Entry(entity).State = EntityState.Modified;
                Save();
            }
            else
            {
                throw new Exception($"Store deactive data not found by {id} Store Id");
            }
        }

        public void Deactive(int id, string descriptions)
        {
            _databaseContext.StoreDeactives.Add(new StoreDeactive(id, descriptions));
            Save();
        }

        public override IEnumerable<Store> Gets(int page, int pageSize) 
            => Query().Skip((page - 1) * pageSize).Take(pageSize);

        public override Store? Get(int id) 
            => Query().FirstOrDefault(x => x.Id == id);

        private IQueryable<Store> Query() 
            => _databaseContext.Stores.Include(x => x.StoreDeactives).Include(x => x.Business);

        public IEnumerable<Store> Gets(int? businessId)
        {
            var query = Query();
            if(businessId != null)
                return query.Where(x => x.BusinessId == businessId);
            return query.ToList();
        }

        public IEnumerable<Store> Gets(int page, int pageSize, int? businessId)
        {
            var query = Query();
            if(businessId != null)
                query = query.Where(x => x.BusinessId == businessId);

           return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Store> GetByBusinessId(int businessId)
        {
            return Query().Where(x => x.BusinessId == businessId).ToList();
        }
    }
}
