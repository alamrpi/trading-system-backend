using smartshop.Business.ViewModels.ProductManagement;
using smartshop.Entities.ProductManagement;

namespace smartshop.Business.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStockRepository _stockRepository;

        public UnitService(IUnitRepository unitRepository, IProductRepository productRepository, IStockRepository stockRepository)
        {
            this._unitRepository = unitRepository;
            this._productRepository = productRepository;
            this._stockRepository = stockRepository;
        }
        public int Create(int businessId, UnitWithVariationViewModel entity)
        {
            return _unitRepository.Create(new Unit(businessId, entity.Name, entity.Symbol, entity.Comments)
            {
                UnitVariations = entity.Variations.Select(x => new UnitVariation(x.Name, x.Qnty)).ToList()
            });
        }

        public void CreateUnitVariation(int unitId, List<UnitVariationViewModel> variations)
        {
            _unitRepository.CreateUnitVariation(variations.Select(v => new UnitVariation(v.Name, v.Qnty)
            {
                UnitId = unitId,
            }).ToList());
        }

        public void Delete(int id)
        {
           _unitRepository.Delete(id);
        }

        public void DeleteUnitVariation(int variationId)
        {
            _unitRepository.DeleteUnitVariation(variationId);
        }

        public bool Exists(int businessId, int id) 
            => _unitRepository.Exists(businessId, id);

        public bool Exists(int businessId, string name, int? id = null) 
            => _unitRepository.Exists(businessId, name, id);

        public PaginationResponse<UnitDto> Get(int businessId, int currentPage, int pageSize)
        {
            var response = _unitRepository.Get(businessId, currentPage, pageSize);
            return new PaginationResponse<UnitDto>
            {
                TotalRows = response.TotalRows,
                Rows = response.Rows.Select(u => MapEntityToDto(u)).ToList()
            };
        }

        public UnitDto? Get(int businessId, int id)
        {
            var unit = _unitRepository.Get(businessId, id);
            if(unit == null)
                return null;
            return MapEntityToDto(unit);
        }

        public IEnumerable<DropdownDto> Get(int businessId)
            => _unitRepository.Get(businessId).Select(u => new DropdownDto(u.Symbol, u.Id));

        public IEnumerable<DropdownDto> GetVariations(int businessId, int unitId)
            => _unitRepository.GetVariations(businessId, unitId).Select(uv => new DropdownDto(uv.Name, uv.Id));

        public IEnumerable<DropdownDto> GetVariationsByProductId(int businessId, int productId)
        {
            var product = _productRepository.Get(businessId, productId);
            if (product == null)
                return null;

            return _unitRepository.GetVariations(businessId, product.UnitId)
                .Select(uv => new DropdownDto(uv.Name, uv.Id))
                .ToList();
        }

        public IEnumerable<DropdownDto> GetVariationsByStockId(int businessId, int productId)
        {
            int unitId = _stockRepository.GetUnitId(businessId, productId);

            return _unitRepository.GetVariations(businessId, unitId)
                .Select(uv => new DropdownDto(uv.Name, uv.Id))
                .ToList();
        }

        public void Update(int id, UnitViewModel entity)
        {
            _unitRepository.Update(id, new Unit(0, entity.Name, entity.Symbol, entity.Comments));
        }

        private UnitDto MapEntityToDto(Unit u)
        {
            return new UnitDto
            {
                Id = u.Id,
                Name = u.Name,
                Comments = u.Comments,
                Symbol = u.Symbol,
                UnitVariations = u.UnitVariations.Select(uv => new UnitVariationDto { Id = uv.Id, Name = uv.Name, Qnty = uv.Qnty }).ToList()
            };
        }
    }
}
