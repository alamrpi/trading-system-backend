using smartshop.Common.QueryParams.Purchase;
using smartshop.Entities.PurchaseManagement;

namespace smartshop.Data.IRepositories
{
    public interface IPurchaseRepository
    {
        PaginationResponse<Purchase> Get(int businessId, int currentPage, int pageSize, PurchaseQueryParams searchParams);
        IEnumerable<Purchase> Get(int businessId, int? storeId, int? supplierId);
        IEnumerable<Purchase> Get(int businessId, PurchaseQueryParams searchParams);
        Purchase? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        bool AnyDuePaymentOrReturn(int businessId, int id);
        int Create(Purchase entity, int businessId);
        void Update(int id, Purchase entity);
        void Delete(int id);
        IEnumerable<PurchaseProduct> GetPurchaseProducts(int businessId, int id);
    }
}
