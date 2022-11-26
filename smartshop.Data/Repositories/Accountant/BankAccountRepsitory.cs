using smartshop.Common.Enums.Accounting;
using smartshop.Entities.Accounting;

namespace smartshop.Data.Repositories
{
    internal class BankAccountRepsitory : IBankAccountRepsitory
    {
        private readonly ApplicationDbContext _dbContext;

        public BankAccountRepsitory(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public int Create(BankAccount entity)
        {
            _dbContext.BankAccounts.Add(entity);
            Save();
            return entity.Id;
        }

        public void Delete(int id)
        {
            var entity = GetBankAccounts().Single(b => b.Id == id);
            _dbContext.BankAccounts.Remove(entity);
            Save();
        }

        public bool Exists(int businessId, int id)
            => GetBankAccounts(businessId).Any(x => x.Id == id);

        public bool Exists(int businessId, string name, int? id = null)
        {
            var query = GetBankAccounts(businessId);
            if (id != null) query = query.Where(b => b.Id != id);

            return query.Any(b => b.Head.Name.Trim().ToLower() == name.Trim().ToLower() && b.Head.HeadType == HeadTypes.BankAccount);
        }

        public PaginationResponse<BankAccount> Get(int businessId, int currentPage, int pageSize)
        {
            var suppliers = GetBankAccounts(businessId);
            return new PaginationResponse<BankAccount>
            {
                Rows = suppliers.Skip((currentPage - 1) * pageSize).Take(pageSize),
                TotalRows = suppliers.Count()
            };
        }

        public BankAccount? Get(int businessId, int id)
            => GetBankAccounts(businessId).FirstOrDefault(b => b.Id == id);

        public IEnumerable<BankAccount> Get(int businessId)
            => GetBankAccounts(businessId).ToList();

        public BankAccount? GetById(int bankAccountId)
        {
            return _dbContext.BankAccounts.Find(bankAccountId);
        }

        public int TotalCount(int businessId)
            => GetBankAccounts(businessId).Count();

        public void Update(int id, BankAccount entity)
        {
            var bankAccount = GetBankAccounts().SingleOrDefault(x => x.Id == id);
            bankAccount.Head.Name = entity.Head.Name;
            bankAccount.AccountNumber = entity.AccountNumber;
            bankAccount.BranchName = entity.BranchName;
            bankAccount.Balance = entity.Balance;
            bankAccount.Head.Descriptions = entity.Head.Descriptions;
            _dbContext.Entry(bankAccount).State = EntityState.Modified;
            Save();
        }

        public void Update(BankAccount bankAccount)
        {
            _dbContext.Entry(bankAccount).State = EntityState.Modified;
            Save();
        }

        private IQueryable<BankAccount> GetBankAccounts(int? businessId = null)
        {
            var query = _dbContext.BankAccounts
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
