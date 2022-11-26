using smartshop.Common.Enums.Accounting;
using smartshop.Entities.Accounting;

namespace smartshop.Data.Repositories
{
    internal class InvestorRepository : IInvestorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public InvestorRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public int Create(Investor entity)
        {
            _dbContext.Investors.Add(entity);
            Save();
            return entity.Id;
        }

        public void Delete(int id)
        {
            var entity = Query().Single(b => b.Id == id);
            _dbContext.Investors.Remove(entity);
            Save();
        }

        public bool Exists(int businessId, int id)
            => Query(businessId).Any(x => x.Id == id);

        public PaginationResponse<Investor> Get(int businessId, int currentPage, int pageSize)
        {
            var investors = Query(businessId);
            return new PaginationResponse<Investor>
            {
                Rows = investors.Skip((currentPage - 1) * pageSize).Take(pageSize),
                TotalRows = investors.Count()
            };
        }

        public Investor? Get(int businessId, int id)
            => Query(businessId).FirstOrDefault(b => b.Id == id);

        public IEnumerable<Investor> Get(int businessId)
            => Query(businessId).ToList();

        public int TotalCount(int businessId)
            => Query(businessId).Count();

        public void Update(int id, Investor entity)
        {
            var bankAccount = Query().SingleOrDefault(x => x.Id == id);
            bankAccount.Head.Name = entity.Head.Name;
            bankAccount.Email = entity.Email;
            bankAccount.PhoneNumber = entity.PhoneNumber;
            bankAccount.Head.Descriptions = entity.Head.Descriptions;
            _dbContext.Entry(bankAccount).State = EntityState.Modified;
            Save();
        }

        private IQueryable<Investor> Query(int? businessId = null)
        {
            var query = _dbContext.Investors
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
