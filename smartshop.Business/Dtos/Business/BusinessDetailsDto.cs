namespace smartshop.Business.Dtos.Business
{
    public class BusinessDetailsDto : BusinessDto
    {
        public BusinessDetailsDto(int id, string name, bool isActive, string contactNo, string email, string address, string webAddress, 
           string? logo, string? objective,  IList<DeactiveHistoryDto> deactives)
            : base(id, name, isActive, contactNo, email, address, webAddress, logo)
        {
            Objective = objective;
            BusinessDeactives = deactives;
        }

        public string? Objective { get; set; }

        public IList<DeactiveHistoryDto> BusinessDeactives { get; set; }
    }
}
