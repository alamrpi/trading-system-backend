namespace smartshop.Business.ViewModels.Accountant
{
    public class IncomeExpenseViewModel
    {
        public int HeadId { get; set; }

        public int BankAccountId { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public bool Income { get; set; }
    }
}
