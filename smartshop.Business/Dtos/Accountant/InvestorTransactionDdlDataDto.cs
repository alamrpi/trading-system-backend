namespace smartshop.Business.Dtos.Accountant
{
    public class InvestorTransactionDdlDataDto
    {
        public IEnumerable<DropdownDto> Investors { get; set; }
        public IEnumerable<DropdownDto> Banks { get; set; }
    }
}
