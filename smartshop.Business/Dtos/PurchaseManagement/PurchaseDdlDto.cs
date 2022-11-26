namespace smartshop.Business.Dtos.PurchaseManagement
{
    public class PurchaseDdlDto
    {
        public IEnumerable<DropdownDto> Suppliers { get; set; }
        public IEnumerable<DropdownDto> Stores { get; set; }
        public IEnumerable<DropdownDto> BankAccounts { get; set; }
        public IEnumerable<DropdownDto> Products { get; set; }
    }
}
