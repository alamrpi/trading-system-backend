using smartshop.Business.ViewModels.ProductManagement;
using smartshop.Common.Enums.Accounting;
using smartshop.Common.QueryParams;
using smartshop.Entities.Accounting;
using smartshop.Entities.ProductManagement;


namespace smartshop.Business.Service
{
    internal class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IUnitRepository _unitRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IGroupRepository _groupRepository;

        public ProductService(IProductRepository repository, IUnitRepository unitRepository, IBrandRepository brandRepository, IGroupRepository groupRepository)
        {
            this._repository = repository;
            this._unitRepository = unitRepository;
            this._brandRepository = brandRepository;
            this._groupRepository = groupRepository;
        }
        public int Create(int businessId, ProductViewModel entity)
            => _repository.Create(new Product()
            {
                Head = new Head(businessId, entity.Name, entity.Descriptions, HeadTypes.Product),
                CategoryId = entity.CategoryId,
                BrandId = entity.BrandId,
                UnitId = entity.UnitId,
                AlertQty = entity.AlertQty,
                Descriptions = entity.Descriptions,
                Barcode = DateTime.Now.ToString("yyyyMMddHHmmss")
            });

        public void Delete(int id)
            => _repository.Delete(id);

        public bool Exists(int businessId, int id)
            => _repository.Exists(businessId, id);

        public bool Exists(int businessId, string name, int? id = null)
            => _repository.Exists(businessId, name, id);

        public PaginationResponse<ProductDto> Get(int businessId, int currentPage, int pageSize, ProductQueryParams queryParams)
        {
            var response = _repository.Get(businessId, currentPage, pageSize, queryParams);
            return new PaginationResponse<ProductDto>
            {
                TotalRows = response.TotalRows,
                Rows = response.Rows.Select(b => MapEntityToDto(b)).ToList()
            };
        }

        public ProductDto? Get(int businessId, int id)
        {
            var product = _repository.Get(businessId, id);

            if (product == null) return null;

            return MapEntityToDto(product);
        }

        public IEnumerable<DropdownDto> Get(int businessId, ProductQueryParams queryParams)
            => _repository.Get(businessId, queryParams).Select(b => new DropdownDto(b.Head.Name, b.Id)).ToList();

        public DropdownDataForProductViewModel GetDdlData(int businessId)
        {
            return new DropdownDataForProductViewModel
            {
                Units = _unitRepository.Get(businessId).Select(x => new DropdownDto { Text = x.Name, Value = x.Id }).ToList(),
                Brands = _brandRepository.Get(businessId).Select(x => new DropdownDto { Text = x.Name, Value = x.Id }).ToList(),
                Groups = _groupRepository.Get(businessId).Select(x => new DropdownDto { Text = x.Name, Value = x.Id }).ToList()
            };
        }

        public void Update(int id, ProductViewModel entity)
            => _repository.Update(id, new Product()
            {
                Head = new Head(0, entity.Name, entity.Descriptions, HeadTypes.Product),
                CategoryId = entity.CategoryId,
                BrandId = entity.BrandId,
                UnitId = entity.UnitId,
                AlertQty = entity.AlertQty,
                Descriptions = entity.Descriptions
            });

        private ProductDto MapEntityToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Head.Name,
                GroupId = product.Category.GroupId,
                GroupName = product.Category.Group.Name,
                AlertQty = product.AlertQty,
                Barcode = product.Barcode,
                BrandId = product.BrandId,
                BrandName = product.Brand.Name,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                UnitId = product.UnitId,
                UnitName = product.Unit.Name,
                Descriptions = product.Descriptions
            };
        }
    }
}
