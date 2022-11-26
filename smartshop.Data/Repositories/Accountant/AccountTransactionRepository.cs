using Microsoft.AspNetCore.Components;
using smartshop.Common.Enums.Accounting;
using smartshop.Common.QueryParams.Accountant;
using smartshop.Entities.Accounting;
using System.ComponentModel;

namespace smartshop.Data.Repositories.Accountant
{
    internal class AccountTransactionRepository : IAccountTransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountTransactionRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IEnumerable<AccountTransaction> GetBalaceSheet(int businessId, StatementQueryParams queryParams)
        {
            var query = GetQuery(businessId);

            if (queryParams.FromDate != null)
                query = query.Where(at => at.HeadTransaction.Date.Date >= queryParams.FromDate.Value.Date);

            if (queryParams.ToDate != null)
                query = query.Where(at => at.HeadTransaction.Date.Date <= queryParams.ToDate.Value.Date);

            return query.ToList();
        }

        public IEnumerable<AccountTransaction> GetTransactionByAccount(int accountId, int businessId, StatementQueryParams queryParams)
        {
            var query = GetQuery(businessId).Where(at => at.AccountId == accountId);

            if (queryParams.FromDate != null)
                query = query.Where(at => at.HeadTransaction.Date.Date >= queryParams.FromDate.Value.Date);

            if (queryParams.ToDate != null)
                query = query.Where(at => at.HeadTransaction.Date.Date <= queryParams.ToDate.Value.Date);

            return query.ToList();
        }

        public IEnumerable<AccountTransaction> GetTransactionByComponent(EquationComponent component, int businessId, StatementQueryParams queryParams)
        {
            var query = GetQuery(businessId).Where(at => at.Account.EquationComponent == component);

            if(queryParams.FromDate != null)
                query = query.Where(at => at.HeadTransaction.Date.Date >= queryParams.FromDate.Value.Date);

            if (queryParams.ToDate != null)
                query = query.Where(at => at.HeadTransaction.Date.Date <= queryParams.ToDate.Value.Date);

            return query.ToList();
        }

        private IQueryable<AccountTransaction> GetQuery(int businessId)
        {
            return _dbContext.AccountTransactions
                .Include(at => at.HeadTransaction)
                .ThenInclude(ht => ht.Head)
                .Include(at => at.Account)
                .Where(at => at.HeadTransaction.Head.BusinessId == businessId)
                .AsQueryable();
        }
      
    }
}
