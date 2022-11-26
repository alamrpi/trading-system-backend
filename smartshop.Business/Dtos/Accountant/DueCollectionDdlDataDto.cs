namespace smartshop.Business.Dtos
{
    public class DueCollectionDdlDataDto
    {
        public IEnumerable<DropdownDto> Store { get; set; }
        public IEnumerable<DropdownDto> Customers { get; set; }
        public IEnumerable<DropdownDto> Banks { get; set; }
    }
}
