namespace smartshop.Business.Dtos
{
    public class DuePaymentDdlDataDto
    {
        public IEnumerable<DropdownDto> Store { get; set; }
        public IEnumerable<DropdownDto> Suppliers { get; set; }
        public IEnumerable<DropdownDto> Banks { get; set; }
    }
}
