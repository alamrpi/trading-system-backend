using smartshop.Business.Dtos;
using smartshop.Business.IServices;
using smartshop.Business.ViewModels;
using smartshop.Common.Dto;
using smartshop.Data.IRepositories;
using smartshop.Entities.ProductManagement;

namespace smartshop.Business.Service
{
    internal class GroupService : IGroupService
    {
        private readonly IGroupRepository _repository;

        public GroupService(IGroupRepository repository)
        {
            this._repository = repository;
        }
        public int Create(int businessId, GroupViewModel entity) 
            => _repository.Create(new Group(businessId, entity.Name, entity.Comments));

        public void Delete(int id)
            => _repository.Delete(id);

        public bool Exists(int businessId, int id) 
            => _repository.Exists(businessId, id);

        public bool Exists(int businessId, string name, int? id = null)
            => _repository.Exists(businessId, name, id);

        public PaginationResponse<GroupDto> Get(int businessId, int currentPage, int pageSize)
        {
            var response = _repository.Get(businessId, currentPage, pageSize);
            return new PaginationResponse<GroupDto>
            {
                TotalRows = response.TotalRows,
                Rows = response.Rows.Select(b => MapEntityToDto(b)).ToList()
            };
        }

        public GroupDto? Get(int businessId, int id)
        {
            var brand = _repository.Get(businessId, id);

            if (brand == null) return null;

            return MapEntityToDto(brand);
        }

        public IEnumerable<DropdownDto> Get(int businessId)
            => _repository.Get(businessId).Select(b => new DropdownDto(b.Name, b.Id)).ToList();

        public void Update(int id, GroupViewModel entity)
            => _repository.Update(id, new Group(0, entity.Name, entity.Comments));

        private GroupDto MapEntityToDto(Group b)
        {
            return new GroupDto
            {
                Id = b.Id,
                Name = b.Name,
                Comments = b.Comments
            };
        }
    }
}
