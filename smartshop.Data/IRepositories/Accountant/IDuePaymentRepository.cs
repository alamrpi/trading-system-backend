using smartshop.Entities.Accounting;

namespace smartshop.Data.IRepositories
{
    public interface IDuePaymentRepository
    {
        PaginationResponse<DuePayment> Get(int businessId, DuePaymentQueryParams queryParams);
        DuePayment? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        int Create(DuePayment entity);
        void Update(int id, DuePayment entity);
        void Delete(int id);
        string GenerateSlip(int businessId);
    }
}
