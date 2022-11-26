using smartshop.Entities.Accounting;

namespace smartshop.Data.IRepositories
{
    public interface IHeadTransactionRepository 
    {
        int Create(IEnumerable<HeadTransaction> headTransactions);
        void DeleteByDueCollection(int id);
        void DeleteByDuePayment(int id);
        void DeleteByIncomeExpense(int id);
        void DeleteByInvestorTransaction(int id);
        void DeleteByPurchase(int purchaseId);
        void DeleteByPurchaseReturn(int purchaseReturnId);
        void DeleteBySale(int id);
        void DeleteBySaleReturn(int id);
        IEnumerable<HeadTransaction> GetLedgers(LedgerFilterQueryParams queryParams);
    }
}
