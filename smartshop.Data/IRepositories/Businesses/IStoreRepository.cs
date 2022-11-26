using smartshop.Entities.Businesses;

namespace smartshop.Data.IRepositories.Businesses
{
    public interface IStoreRepository : IRepository<Store>
    {
        void Active(int id);
        void Deactive(int id, string descriptions);

        IEnumerable<Store> Gets(int? businessId);
        IEnumerable<Store> Gets(int page, int pageSize, int? businessId);
        IEnumerable<Store> GetByBusinessId(int businessId);
    }
}
