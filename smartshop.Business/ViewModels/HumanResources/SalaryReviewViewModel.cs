using System.ComponentModel;

namespace smartshop.Business.ViewModels.HumanResources
{
    public class SalaryReviewViewModel
    {
        [DisplayName("Basic Salary")]
        public decimal BasicSalary { get; set; }

        [DisplayName("House Rent")]
        public decimal HouseRent { get; set; }

        [DisplayName("Transport Allowance")]
        public decimal TransportAllowance { get; set; }

        [DisplayName("Medical Allowance")]
        public decimal MedicalAllowance { get; set; }

        [DisplayName("Meal Allowance")]
        public decimal MealAllowance { get; set; }

        [DisplayName("Providence Fund")]
        public decimal ProvidenceFund { get; set; }

        public decimal Insurance { get; set; }
        public decimal Tax { get; set; }

        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
    }
}
