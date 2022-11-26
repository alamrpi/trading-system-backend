using smartshop.Common.Enums.Accounting;
using smartshop.Entities.Accounting;

namespace smartshop.Business.Service
{
    public class InvestorService : IInvestorService
    {
        private readonly IInvestorRepository _repository;

        public InvestorService(IInvestorRepository repository)
        {
            _repository = repository;
        }
        public int Create(int businessId, InvestorViewModel entity)
        {
            var head = new Head(businessId, entity.Name, entity.Descriptions, HeadTypes.Investor);

            return _repository.Create(new Investor()
            {
                 PhoneNumber = entity.PhoneNumber,
                 Email = entity.Email,
                 Head = head,
            });
        }

        public void Delete(int id)
            => _repository.Delete(id);

        public bool Exists(int businessId, int id)
            => _repository.Exists(businessId, id);

        public PaginationResponse<InvestorDto> Get(int businessId, int currentPage, int pageSize)
        {
            var response = _repository.Get(businessId, currentPage, pageSize);
            return new PaginationResponse<InvestorDto>
            {
                TotalRows = response.TotalRows,
                Rows = response.Rows.Select(b => MapEntityToDto(b)).ToList()
            };
        }

        public InvestorDto? Get(int businessId, int id)
        {
            var investor = _repository.Get(businessId, id);

            if (investor == null) return null;

            return MapEntityToDto(investor);
        }

        public IEnumerable<DropdownDto> Get(int businessId)
            => _repository.Get(businessId).Select(b => new DropdownDto(b.Head.Name, b.Id)).ToList();

        public void Update(int id, InvestorViewModel entity)
        {
            var head = new Head(0, entity.Name, entity.Descriptions, HeadTypes.Investor);
            _repository.Update(id, new Investor()
            {
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                Head = head,
            });
        }

        private InvestorDto MapEntityToDto(Investor b)
        {
            return new InvestorDto
            {
                Id = b.Id,
                Name = b.Head.Name,
                PhoneNumber = b.PhoneNumber,
                Email = b.Email,
                Descriptions = b.Head.Descriptions
            };
        }
    }
}
