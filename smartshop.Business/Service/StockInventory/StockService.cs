using smartshop.Business.Dtos.StockInventory;
using smartshop.Common.QueryParams;
using smartshop.Data.IRepositories.Businesses;
using smartshop.Entities.Stocks;

namespace smartshop.Business.Service
{
    internal class StockService : IStockService
    {
        private readonly IStockRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IStoreRepository _storeRepository;

        public StockService(IStockRepository repository, IProductRepository productRepository, IBrandRepository brandRepository, ICategoryRepository categoryRepository,
            IGroupRepository groupRepository, IUnitRepository unitRepository, IStoreRepository storeRepository)
        {
            _repository = repository;
            this._productRepository = productRepository;
            this._brandRepository = brandRepository;
            this._categoryRepository = categoryRepository;
            this._groupRepository = groupRepository;
            this._unitRepository = unitRepository;
            this._storeRepository = storeRepository;
        }
        public int CreateDamage(StockDamageViewModel model, string userId, string clientIp)
            => _repository.CreateDamage(new StockDamage(model.StockId, model.DamageQnty, model.Descriptions, clientIp, userId));

        public bool ExistsStock(int businessId, int stockId) 
            => _repository.ExistsStock(businessId, stockId);

        public IEnumerable<StockDamageDto> GetDamageReports(int businessId, StockOrDamageQueryParams queryParams)
        {
            return _repository.GetDamageReports(businessId, queryParams).Select(damage => MapEntityToDto(damage));
        }
        public PaginationResponse<StockDamageDto> GetDamages(int businessId, int currentPage, int pageSize, StockOrDamageQueryParams queryParams)
        {
            var response = _repository.GetDamages(businessId, currentPage, pageSize, queryParams);

            return new PaginationResponse<StockDamageDto>()
            {
                TotalRows = response.TotalRows,
                Rows = response.Rows.Select(damage => MapEntityToDto(damage))
            };
        }

        public IEnumerable<StockDto> GetReports(int businessId, StockOrDamageQueryParams queryParams)
        {
            return _repository.GetReports(businessId, queryParams).Select(stock => MapStockEntityToDto(stock));
        }

        public PaginationResponse<StockDto> Gets(int businessId, int currentPage, int pageSize, StockOrDamageQueryParams queryParams)
        {
            var response = _repository.Gets(businessId, currentPage, pageSize, queryParams);

            return new PaginationResponse<StockDto>()
            {
                TotalRows = response.TotalRows,
                Rows = response.Rows.Select(stock => MapStockEntityToDto(stock))
            };
        }

        private StockDto MapStockEntityToDto(Stock stock)
        {
            return new StockDto
            {
                Id = stock.Id,
                Stock = stock.StockQty,
                TradePrice = stock.TradePrice,
                ProductName = stock.Product.Head.Name,
                BrandName = stock.Product.Brand.Name,
                CategoryName = stock.Product.Category.Name,
                GroupName = stock.Product.Category.Group.Name,
                StoreName = stock.Store.Name,
                UnitName = stock.Product.Unit.Name
            };
        }  
        
        private StockDamageDto MapEntityToDto(StockDamage damage)
        {
            return new StockDamageDto
            {
                Id = damage.Id,
                DamageQty = damage.DamageQnty,
                Descriptions = damage.Descriptions,
                ProductName = damage.Stock.Product.Head.Name,
                BrandName = damage.Stock.Product.Brand.Name,
                CategoryName = damage.Stock.Product.Category.Name,
                GroupName = damage.Stock.Product.Category.Group.Name,
                StoreName = damage.Stock.Store.Name,
                UnitName = damage.Stock.Product.Unit.Name
            };
        }
        
        public IEnumerable<DropdownDto> GetForDdl(int businessId, int storeId)
        {
            return _repository.GetForDdl(businessId, storeId).Select(s => new DropdownDto()
            {
                Text = $"{s.Product.Head.Name} - {s.Product.Category.Name}-{s.Product.Category.Group.Name}-{s.Product.Brand.Name}({Math.Round(s.StockQty, 2)} {s.Product.Unit.Symbol})",
                Value = s.Id
            });
        }

        public StockFilterDdlDto GetStockDdlData(int businessId)
        {
            return new StockFilterDdlDto
            {
                Brands = _brandRepository.Get(businessId).Select(b => new DropdownDto(b.Name, b.Id)),
                Groups = _groupRepository.Get(businessId).Select(g => new DropdownDto(g.Name, g.Id)),
                Units = _unitRepository.Get(businessId).Select(g => new DropdownDto(g.Name, g.Id)),
                Categories = _categoryRepository.Get(businessId).Select(g => new DropdownDto(g.Name, g.Id)),
                Products = _productRepository.Get(businessId, new ProductQueryParams()).Select(g => new DropdownDto(g.Head.Name, g.Id)),
                Stores = _storeRepository.GetByBusinessId(businessId).Select(g => new DropdownDto(g.Name, g.Id))
            };
        }
    }
}
