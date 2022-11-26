using smartshop.Data.IRepositories.Businesses;
using smartshop.Entities.Businesses;
using smartshop.Entities.Settings;

namespace smartshop.Data.Repositories.Businesses
{
    internal class BusinessRepository : Repository<Business>, IBusinessRepository
    {
        public BusinessRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public void Active(int businessId)
        {
            var entity = _databaseContext.BusinessDeactives.AsNoTracking().FirstOrDefault(x => x.ReActivateDate == null);
            
            if(entity != null)
            {
                entity.ReActivateDate = DateTime.UtcNow;
                _databaseContext.Entry(entity).State = EntityState.Modified;
                Save();
            }
            else
            {
                throw new Exception($"business deactive data not found by {businessId} businessId");
            }     
        }

        public bool CreateConfigure(BusinessConfigure businessConfigure)
        {
            _databaseContext.BusinessConfigures.Add(businessConfigure);
            return _databaseContext.SaveChanges() > 0;
        }

        public void Deactive(int businessId, string descriptions)
        {
            _databaseContext.BusinessDeactives.Add(new BusinessDeactive(businessId, descriptions));
            Save();
        }

        public override Business? Get(int id)
        {
            return _databaseContext.Businesses
                .Include(x => x.BusinessDeactives)
                .Include(x => x.BusinessConfigure)
                .FirstOrDefault(x => x.Id == id);
        }

        public BusinessConfigure? GetConfigure(int id)
        {
            return _databaseContext.BusinessConfigures.FirstOrDefault(x => x.Id == id);
        }

        public override IEnumerable<Business> Gets(int page, int pageSize)
        {
            return _databaseContext.Businesses
                .Include(x => x.BusinessDeactives)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public IEnumerable<Business> Gets()
        {
            return _databaseContext.Businesses.ToList();
        }

        public bool UpdateConfigure(BusinessConfigure configure)
        {
            _databaseContext.BusinessConfigures.Remove(_databaseContext.BusinessConfigures.Find(configure.Id));
            Save();
          return CreateConfigure(configure);
        }
    }
}
