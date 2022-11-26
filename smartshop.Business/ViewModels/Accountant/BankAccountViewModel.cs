namespace smartshop.Business.ViewModels
{
    public class BankAccountViewModel
    {

        [Required, StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Descriptions { get; set; }

        [Required]
        [StringLength(255)]
        public string BranchName { get; set; }

        [Required, StringLength(50)]
        public string AccountNumber { get; set; }

        [Required]
        public decimal Balance { get; set; }
    }
}
