using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.HumanResource
{
    public class EmployeeSalaryReview
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }

        public decimal BasicSalary { get; set; }
        public decimal HouseRent { get; set; }
        public decimal TransportAllowance { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal MealAllowance { get; set; }
        public decimal ProvidenceFund { get; set; }
        public decimal Insurance { get; set; }
        public decimal Tax { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public EmployeeSalaryReview(int employeeId, decimal basicSalary, decimal houseRent, decimal transportAllowance, decimal medicalAllowance, decimal mealAllowance,
            decimal providenceFund, decimal insurance, decimal tax, DateTime startDate)
        {
            EmployeeId = employeeId;
            BasicSalary = basicSalary;
            HouseRent = houseRent;
            TransportAllowance = transportAllowance;
            MedicalAllowance = medicalAllowance;
            MealAllowance = mealAllowance;
            ProvidenceFund = providenceFund;
            Insurance = insurance;
            Tax = tax;
            StartDate = startDate;
        }
    }
}
