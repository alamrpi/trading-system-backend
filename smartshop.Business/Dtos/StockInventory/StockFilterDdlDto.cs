namespace smartshop.Business.Dtos.StockInventory
{
    public class StockFilterDdlDto
    {
        public IEnumerable<DropdownDto> Products { get; set; }
        public IEnumerable<DropdownDto> Brands { get; set; }
        public IEnumerable<DropdownDto> Categories { get; set; }
        public IEnumerable<DropdownDto> Groups { get; set; }
        public IEnumerable<DropdownDto> Units { get; set; }
        public IEnumerable<DropdownDto> Stores { get; set; }
    }
}
