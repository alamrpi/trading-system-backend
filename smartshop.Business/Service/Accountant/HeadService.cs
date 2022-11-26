using smartshop.Data.IRepositories.Accountant;
using smartshop.Entities.Accounting;

namespace smartshop.Business.Service.Accountant
{
    internal class HeadService : IHeadService
    {
        private readonly IHeadRepository _repository;

        public HeadService(IHeadRepository headRepository)
        {
            this._repository = headRepository;
        }
        public int? CreateOrUpdate(int businessId, HeadViewModel model, int id = 0)
        {
            if (id == 0)
            {
                return _repository.CreateOrUpdate(new Head(businessId, model.Name, model.Descriptions, isConstant: false));
            }
            else
            {
                var head = _repository.Get(businessId, id);
                if (head == null)
                    return null;

                head.Name = model.Name.Trim();
                head.Descriptions = model.Descriptions;

                return _repository.CreateOrUpdate(head);
            }

        }

        public void Delete(int businessId, int id) 
            => _repository.Delete(businessId, id);

        public bool Exits(int businessId, string name, int? id = null)
            => _repository.Exits(businessId, name, id);

        public bool Exits(int businessId, int id) 
            => _repository.Exits(businessId, id);

        public IEnumerable<DropdownDto> Get(int businessId) 
            => _repository.Get(businessId).Select(d => new DropdownDto { Text = d.Name, Value = d.Id });

        public PaginationResponse<HeadDto> Get(int businessId, int currentPage, int pageSize)
        {
            var result = _repository.Get(businessId, currentPage, pageSize);

            return new PaginationResponse<HeadDto>()
            {
                Rows = result.Rows.Select(d => MapEntityToDto(d)),
                TotalRows = result.TotalRows
            };
        }

        public HeadDto? Get(int businessId, int id)
        {
            var designation = _repository.Get(businessId, id);
            if (designation == null)
                return null;

            return MapEntityToDto(designation);
        }

        private HeadDto MapEntityToDto(Head head)
        {
            return new HeadDto
            {
                Id = head.Id,
                Name = head.Name,
                IsAnyTransaction = head.HeadTransactions.Any(),
                Descriptions = head.Descriptions
            };
        }
    }
}
