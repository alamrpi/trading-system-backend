using smartshop.Entities.ProductManagement;

namespace smartshop.Business.Service
{
    internal class BrandService : IBrandService
    {
        private readonly IBrandRepository _repository;

        public BrandService(IBrandRepository repository)
        {
            this._repository = repository;
        }
        public int Create(int businessId, BrandViewModel entity) 
            => _repository.Create(new Brand(businessId, entity.Name, entity.Comments));

        public void Delete(int id)
            => _repository.Delete(id);

        public bool Exists(int businessId, int id) 
            => _repository.Exists(businessId, id);

        public bool Exists(int businessId, string name, int? id = null)
            => _repository.Exists(businessId, name, id);

        public PaginationResponse<BrandDto> Get(int businessId, int currentPage, int pageSize)
        {
            var response = _repository.Get(businessId, currentPage, pageSize);
            return new PaginationResponse<BrandDto>
            {
                TotalRows = response.TotalRows,
                Rows = response.Rows.Select(b => MapEntityToDto(b)).ToList()
            };
        }

        public BrandDto? Get(int businessId, int id)
        {
            var brand = _repository.Get(businessId, id);

            if (brand == null) return null;

            return MapEntityToDto(brand);
        }

        public IEnumerable<DropdownDto> Get(int businessId)
            => _repository.Get(businessId).Select(b => new DropdownDto(b.Name, b.Id)).ToList();

        public void Update(int id, BrandViewModel entity)
            => _repository.Update(id, new Brand(0, entity.Name, entity.Comments));

        private BrandDto MapEntityToDto(Brand b)
        {
            return new BrandDto
            {
                Id = b.Id,
                Name = b.Name,
                Comments = b.Comments
            };
        }
    }
}
