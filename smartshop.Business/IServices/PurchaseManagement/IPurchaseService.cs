using smartshop.Business.Dtos.PurchaseManagement;
using smartshop.Common.QueryParams.Purchase;

namespace smartshop.Business.IServices
{
    public interface IPurchaseService 
    {
        PaginationResponse<PurchaseDto> Get(int businessId, int currentPage, int pageSize, PurchaseQueryParams searchParams);
        IEnumerable<DropdownDto> Get(int businessId, int? storeId, int? supplierId);
        IEnumerable<PurchaseDto> Get(int businessId, PurchaseQueryParams searchParams);
        PurchaseDetailsDto? Get(int businessId, int id);

        bool Exists(int businessId, int id);
        bool AnyDuePaymentOrReturn(int businessId, int id);
        int Create(PurchaseViewModel entity, string userId, string clientIp, int businessId);
        void Update(int id, PurchaseViewModel entity);
        void Delete(int id);
        PurchaseDdlDto GetPurchaseDdl(int v);
        IEnumerable<PurchaseProductDto> GetPurchaseProducts(int businessId, int id);
    }
}
