using smartshop.Entities.SalesManagement;

namespace smartshop.Data.IRepositories
{
    public interface ISaleRepository
    {
        PaginationResponse<Sale> Get(int businessId, int currentPage, int pageSize, SalesQueryParams searchParams);
        IEnumerable<Sale> Get(int businessId, int? storeId, int? customerId);
        IEnumerable<Sale> Get(int businessId, SalesQueryParams searchParams);
        Sale? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        bool AnyDueCollectionOrReturn(int businessId, int id);
        int Create(Sale entity);
        void Update(int id, Sale entity);
        void Delete(int id);
        IEnumerable<SaleProduct> GetSaleProduct(int businessId, int saleId);
        string GeneratSaleInvoiceNumber(int businessId);
    }
}
