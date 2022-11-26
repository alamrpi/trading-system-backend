namespace smartshop.Business.Dtos
{
    public class BankAccountDto
    {
        public int Id { get; set; }
        public string BankAccountName { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string Descriptions { get; set; }
    }
}
