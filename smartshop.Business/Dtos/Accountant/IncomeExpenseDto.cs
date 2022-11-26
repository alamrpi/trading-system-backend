namespace smartshop.Business.Dtos.Accountant
{
    public class IncomeExpenseDto
    {
        public int Id { get; set; }

        public int HeadId { get; set; }
        public string HeadName { get; set; }

        public int BankAccountId { get; set; }
        public string BankAccountName { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public bool Income { get; set; }
    }
}
