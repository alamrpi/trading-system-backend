using smartshop.Common.Enums.Accounting;
using smartshop.Data.IRepositories.Accountant;
using smartshop.Entities.Accounting;

namespace smartshop.Data.Repositories.Accountant
{
    internal class HeadRepository : IHeadRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public HeadRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int? CreateOrUpdate(Head head)
        {
            if (head.Id == 0)
                _dbContext.Heads.Add(head);
            else
                _dbContext.Entry(head).State = EntityState.Modified;

            _dbContext.SaveChanges();
            return head.Id;
        }

        public void Delete(int businessId, int id)
        {
            var head = _dbContext.Heads.AsNoTracking().FirstOrDefault(d => d.BusinessId == businessId && d.Id == id);
            _dbContext.Heads.Remove(head);
            _dbContext.SaveChanges();
        }

        public bool Exits(int businessId, string name, int? id = null)
        {
            var heads = _dbContext.Heads
                .Where(x => x.BusinessId == businessId && x.Name.ToLower() == name.Trim().ToLower() && x.HeadType == HeadTypes.General)
                .AsQueryable();

            if (id != null)
                heads = heads.Where(x => x.Id != id);

            return heads.Any();
        }

        public bool Exits(int businessId, int id)
            => _dbContext.Heads.Any(d => d.BusinessId == businessId && d.Id == id && d.HeadType == HeadTypes.General);

        public IEnumerable<Head> Get(int businessId)
            => _dbContext.Heads.Where(h => h.BusinessId == businessId && h.HeadType == HeadTypes.General).OrderBy(x => x.Name).ToList();

        public PaginationResponse<Head> Get(int businessId, int currentPage, int pageSize)
        {
            var heads = _dbContext.Heads
                .Include(h => h.HeadTransactions)
                .Where(h => h.BusinessId == businessId && h.HeadType == HeadTypes.General)
                .OrderBy(x => x.Name)
                .AsQueryable();

            return new PaginationResponse<Head>()
            {
                Rows = heads.Skip((currentPage - 1) * pageSize).Take(pageSize),
                TotalRows = heads.Count()
            };
        }

        public Head? Get(int businessId, int id)
        {
            return _dbContext.Heads
                 .Include(h => h.HeadTransactions)
                .SingleOrDefault(h => h.BusinessId == businessId && h.Id == id && h.HeadType == HeadTypes.General);
        }
    }
}
