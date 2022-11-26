using smartshop.Entities.Accounting;

namespace smartshop.Data.IRepositories
{
    public interface IInvestorRepository
    {
        PaginationResponse<Investor> Get(int businessId, int currentPage, int pageSize);
        IEnumerable<Investor> Get(int businessId);
        Investor? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        int Create(Investor entity);
        void Update(int id, Investor entity);
        void Delete(int id);
    }
}
