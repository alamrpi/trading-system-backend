using smartshop.Common.QueryParams;

namespace smartshop.Business.IServices
{
    public interface IHeadTransactionService
    {
        IEnumerable<LedgerDto> GetLedgers(LedgerFilterQueryParams queryParams);
    }
}
