using smartshop.Common.QueryParams;

namespace smartshop.Business.Service
{
    internal class HeadTransactionService : IHeadTransactionService
    {
        private readonly IHeadTransactionRepository _headTransactionRepository;

        public HeadTransactionService(IHeadTransactionRepository headTransactionRepository)
        {
            this._headTransactionRepository = headTransactionRepository;
        }
        public IEnumerable<LedgerDto> GetLedgers(LedgerFilterQueryParams queryParams)
        {
            return _headTransactionRepository.GetLedgers(queryParams).Select(x => new LedgerDto()
            {
                HeadName = x.Head.Name,
                Amount = x.Amount,
                Date = x.Date.ToString("dd/MM/yyyy"),
                Descriptions = x.Descriptions,
                Type = x.Type
            });
        }
    }
}
