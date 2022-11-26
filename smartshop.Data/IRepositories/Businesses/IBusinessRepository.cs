using smartshop.Entities.Businesses;
using smartshop.Entities.Settings;

namespace smartshop.Data.IRepositories.Businesses
{
    public interface IBusinessRepository : IRepository<Business>
    {
        void Active(int businessId);
        void Deactive(int businessId, string descriptions);
        IEnumerable<Business> Gets();
        BusinessConfigure? GetConfigure(int id);
        bool CreateConfigure(BusinessConfigure businessConfigure);
        bool UpdateConfigure(BusinessConfigure configure);
    }
}
