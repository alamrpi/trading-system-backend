using smartshop.Business.Dtos.Accountant;
using smartshop.Common.QueryParams;

namespace smartshop.Business.IServices
{
    public interface IInvestorTransactionService
    {
        PaginationResponse<InvestorTransactionDto> Get(int businessId, InvestorTransactionQueryParams queryParams);
        InvestorTransactionDto? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        int Create(InvestorTransactionViewModel entity, string userId);
        void Update(int id, InvestorTransactionViewModel entity);
        void Delete(int id);
        InvestorTransactionDdlDataDto GetDdlData(int businessId);
    }
}
