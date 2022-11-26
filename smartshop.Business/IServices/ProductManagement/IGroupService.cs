namespace smartshop.Business.IServices
{
    public interface IGroupService
    {
        PaginationResponse<GroupDto> Get(int businessId, int currentPage, int pageSize);
        GroupDto? Get(int businessId, int id);
        IEnumerable<DropdownDto> Get(int businessId);
        bool Exists(int businessId, int id);
        bool Exists(int businessId, string name, int? id = null);
        int Create(int businessId, GroupViewModel entity);
        void Update(int id, GroupViewModel entity);
        void Delete(int id);
    }
}
