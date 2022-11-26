using smartshop.Entities.SalesManagement;

namespace smartshop.Data.IRepositories
{
    public interface ISalesReturnRepository
    {
        PaginationResponse<SaleReturn> Get(int businessId, int currentPage, int pageSize, SalesQueryParams searchParams);
        IEnumerable<SaleReturn> Get(int businessId, SalesQueryParams searchParams);
        SaleReturn? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        int Create(SaleReturn entity);
        void Update(int id, SaleReturn entity);
        void Delete(int id);
        IEnumerable<SaleReturnProduct> GetSaleReturnProducts(int id);
        string GeneratSaleInvoiceNumber(int businessId);
    }
}
