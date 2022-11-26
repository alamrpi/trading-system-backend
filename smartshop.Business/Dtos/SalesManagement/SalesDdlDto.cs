namespace smartshop.Business.Dtos.SalesManagement
{
    public class SalesDdlDto
    {
        public IEnumerable<DropdownDto> Banks { get; set; }
        public IEnumerable<DropdownDto> Stores { get; set; }
        public IEnumerable<DropdownDto> Customers { get; set; }
    }
}
