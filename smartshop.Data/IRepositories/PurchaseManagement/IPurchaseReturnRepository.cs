using smartshop.Common.QueryParams.Purchase;

namespace smartshop.Data.IRepositories
{
    public interface IPurchaseReturnRepository
    {
        PaginationResponse<PurchaseReturn> Get(int businessId, int currentPage, int pageSize, PurchaseQueryParams searchParams);
        IEnumerable<PurchaseReturn> Get(int businessId, PurchaseQueryParams searchParams);
        PurchaseReturn? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        int Create(PurchaseReturn entity);
        void Update(int id, PurchaseReturn entity);
        void Delete(int id);
    }
}
