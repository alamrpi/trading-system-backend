using smartshop.Entities.Accounting;

namespace smartshop.Data.IRepositories
{
    public interface IInvestorTransactionRepository
    {
        PaginationResponse<InvestorTransaction> Get(int businessId, InvestorTransactionQueryParams queryParams);
        InvestorTransaction? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        int Create(InvestorTransaction entity);
        void Update(int id, InvestorTransaction entity);
        void Delete(int id);
    }
}
