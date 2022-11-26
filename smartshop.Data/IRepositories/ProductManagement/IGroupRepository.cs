
namespace smartshop.Data.IRepositories
{
    public interface IGroupRepository
    {
        PaginationResponse<Group> Get(int businessId, int currentPage, int pageSize);
        IEnumerable<Group> Get(int businessId);
        Group? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        bool Exists(int businessId, string name, int? id = null);
        int Create(Group entity);
        void Update(int id, Group entity);
        void Delete(int id);
     
    }
}
