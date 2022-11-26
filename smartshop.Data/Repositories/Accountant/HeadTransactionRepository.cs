using smartshop.Entities.Accounting;

namespace smartshop.Data.Repositories
{
    internal class HeadTransactionRepository : IHeadTransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public HeadTransactionRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public int Create(IEnumerable<HeadTransaction> headTransactions)
        {
            _dbContext.HeadTransactions.AddRange(headTransactions);
            return _dbContext.SaveChanges();
        }

        public void DeleteByDueCollection(int id)
        {
            _dbContext.HeadTransactions.RemoveRange(_dbContext.HeadTransactions.Where(x => x.DueCollectionId == id));
            _dbContext.SaveChanges();
        }

        public void DeleteByDuePayment(int id)
        {
            _dbContext.HeadTransactions.RemoveRange(_dbContext.HeadTransactions.Where(x => x.DuePaymentId == id));
            _dbContext.SaveChanges();
        }

        public void DeleteByIncomeExpense(int id)
        {
            _dbContext.HeadTransactions.RemoveRange(_dbContext.HeadTransactions.Where(x => x.IncomeExpenseId == id));
            _dbContext.SaveChanges();
        }

        public void DeleteByInvestorTransaction(int id)
        {
            _dbContext.HeadTransactions.RemoveRange(_dbContext.HeadTransactions.Where(x => x.InvestorTransactionId == id));
            _dbContext.SaveChanges();
        }

        public void DeleteByPurchase(int purchaseId)
        {
            _dbContext.HeadTransactions.RemoveRange(_dbContext.HeadTransactions.Where(x => x.purchaseId == purchaseId));
            _dbContext.SaveChanges();
        }

        public void DeleteByPurchaseReturn(int purchaseReturnId)
        {
            _dbContext.HeadTransactions.RemoveRange(_dbContext.HeadTransactions.Where(x => x.purchaseReturnId == purchaseReturnId));
            _dbContext.SaveChanges();
        }

        public void DeleteBySale(int id)
        {
            _dbContext.HeadTransactions.RemoveRange(_dbContext.HeadTransactions.Where(x => x.SaleId == id));
            _dbContext.SaveChanges();
        }

        public void DeleteBySaleReturn(int id)
        {
            _dbContext.HeadTransactions.RemoveRange(_dbContext.HeadTransactions.Where(x => x.SaleReturnId == id));
            _dbContext.SaveChanges();
        }

        public IEnumerable<HeadTransaction> GetLedgers(LedgerFilterQueryParams queryParams)
        {
            var query = _dbContext.HeadTransactions
                .Include(x => x.Head)
                .Where(ht => ht.Head.HeadType == queryParams.HeadType)
                .AsQueryable();

            if (queryParams.HeadId != null)
                query = query.Where(x => x.HeadId == queryParams.HeadId);

            if (queryParams.FromDate != null)
                query = query.Where(x => x.Date.Date >= queryParams.FromDate.Value.Date);

            if (queryParams.ToDate != null)
                query = query.Where(x => x.Date.Date <= queryParams.ToDate.Value.Date);

            return query.ToList();
        }
    }
}
