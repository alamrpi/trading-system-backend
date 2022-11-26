using smartshop.Entities.HumanResource;

namespace smartshop.Business.Service
{
    public class DesignationService : IDesignationService
    {
        private readonly IDesignationRepository _designationRepository;

        public DesignationService(IDesignationRepository designationRepository)
        {
            this._designationRepository = designationRepository;
        }
        public int? CreateOrUpdate(int businessId, DesignationViewModel model, int id = 0)
        {
            if(id == 0)
            {
                return _designationRepository.CreateOrUpdate(new Designation(businessId, model.Name, model.Priority, model.Descriptions));
            }
            else
            {
                var designations = _designationRepository.Get(businessId, id);
                if(designations == null)
                    return null;

                designations.Name = model.Name.Trim();
                designations.Priority = model.Priority;
                designations.Descriptions = model.Descriptions;

               return _designationRepository.CreateOrUpdate(designations);
            }

        }

        public void Delete(int businessId, int id)
        {
            _designationRepository.Delete(businessId, id);
        }

        public bool Exits(int businessId, string name, int? id = null)
        {
            return _designationRepository.Exits(businessId, name, id);
        }

        public bool Exits(int businessId, int id)
        {
            return _designationRepository.Exits(businessId, id);
        }

        public IEnumerable<DropdownDto> Get(int businessId)
        {
            return _designationRepository.Get(businessId).Select(d => new DropdownDto { Text = d.Name, Value = d.Id});
        }

        public PaginationResponse<DesignationDto> Get(int businessId, int currentPage, int pageSize)
        {
            var result = _designationRepository.Get(businessId, currentPage, pageSize);

            return new PaginationResponse<DesignationDto>()
            {
                Rows = result.Rows.Select(d => MapEntityToDto(d)),
                TotalRows = result.TotalRows
            };
        }

        public DesignationDto? Get(int businessId, int id)
        {
            var designation = _designationRepository.Get(businessId, id);
            if (designation == null)
                return null;

            return MapEntityToDto(designation);
        }

        private DesignationDto MapEntityToDto(Designation d)
        {
            return new DesignationDto
            {
                Id = d.Id,
                Name = d.Name,
                Priority = d.Priority,
                Descriptions = d.Descriptions
            };
        }
    }
}
