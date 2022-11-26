using smartshop.Entities.ProductManagement;

namespace smartshop.Business.Service
{
    internal class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            this._repository = repository;
        }
        public int Create(int businessId, CategoryViewModel entity)
            => _repository.Create(new Category(entity.Name, entity.GroupId));

        public void Delete(int id)
            => _repository.Delete(id);

        public bool Exists(int businessId, int id)
            => _repository.Exists(businessId, id);

        public bool Exists(int businessId, string name, int? id = null)
            => _repository.Exists(businessId, name, id);

        public PaginationResponse<CategoryDto> Get(int businessId, int currentPage, int pageSize)
        {
            var response = _repository.Get(businessId, currentPage, pageSize);
            return new PaginationResponse<CategoryDto>
            {
                TotalRows = response.TotalRows,
                Rows = response.Rows.Select(b => MapEntityToDto(b)).ToList()
            };
        }

        public CategoryDto? Get(int businessId, int id)
        {
            var brand = _repository.Get(businessId, id);

            if (brand == null) return null;

            return MapEntityToDto(brand);
        }

        public IEnumerable<DropdownDto> GetDropdown(int businessId, int groupId)
            => _repository.GetDdl(businessId, groupId).Select(b => new DropdownDto(b.Name, b.Id)).ToList();

        public void Update(int id, CategoryViewModel entity)
            => _repository.Update(id, new Category(entity.Name, entity.GroupId));

        private CategoryDto MapEntityToDto(Category b)
        {
            return new CategoryDto
            {
                Id = b.Id,
                Name = b.Name,
                GroupId = b.GroupId,
                GroupName = b.Group.Name
            };
        }
    }
}
