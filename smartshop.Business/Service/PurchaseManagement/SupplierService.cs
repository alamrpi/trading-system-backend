using smartshop.Common.Enums.Accounting;
using smartshop.Data.IRepositories.Businesses;
using smartshop.Entities;
using smartshop.Entities.Accounting;

namespace smartshop.Business.Service
{
    internal class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repository;
        private readonly IBusinessRepository _businessRepository;

        public SupplierService(ISupplierRepository repository, IBusinessRepository businessRepository)
        {
            _repository = repository;
            _businessRepository = businessRepository;
        }
        public int Create(int businessId, SupplierViewModel entity)
        {
            var head = new Head(businessId, entity.Name, entity.Descriptions, HeadTypes.Supplier);

            var totalSupplier = _repository.TotalCount(businessId);
            var config = _businessRepository.GetConfigure(businessId);
            var supplierId = $"{config.SupplierIdPrefix}-{totalSupplier + 1}";

            return _repository.Create(new Supplier(head, supplierId, entity.Mobile, entity.Email, entity.Address));
        }

        public void Delete(int id)
            => _repository.Delete(id);

        public bool Exists(int businessId, int id)
            => _repository.Exists(businessId, id);

        public bool Exists(int businessId, string name, int? id = null)
            => _repository.Exists(businessId, name, id);

        public PaginationResponse<SupplierDto> Get(int businessId, int currentPage, int pageSize)
        {
            var response = _repository.Get(businessId, currentPage, pageSize);
            return new PaginationResponse<SupplierDto>
            {
                TotalRows = response.TotalRows,
                Rows = response.Rows.Select(b => MapEntityToDto(b)).ToList()
            };
        }

        public SupplierDto? Get(int businessId, int id)
        {
            var supplier = _repository.Get(businessId, id);

            if (supplier == null) return null;

            return MapEntityToDto(supplier);
        }

        public IEnumerable<DropdownDto> Get(int businessId)
            => _repository.Get(businessId).Select(b => new DropdownDto($"{b.Head.Name}- {b.Mobile}", b.Id)).ToList();

        public void Update(int id, SupplierViewModel entity)
        {
            var head = new Head(0, entity.Name, entity.Descriptions, HeadTypes.Supplier);
            _repository.Update(id, new Supplier(head, "", entity.Mobile, entity.Email, entity.Address));
        }

        private SupplierDto MapEntityToDto(Supplier b)
        {
            return new SupplierDto
            {
                Id = b.Id,
                Name = b.Head.Name,
                Email = b.Email,
                Mobile = b.Mobile,
                Address = b.Address,
                Descriptions = b.Head.Descriptions,
                SupplierId = b.SupId
            };
        }
    }
}
