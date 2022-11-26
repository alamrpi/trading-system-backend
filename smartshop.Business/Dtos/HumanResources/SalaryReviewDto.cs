namespace smartshop.Business.Dtos
{
    public class SalaryReviewDto 
    {
        public int Id { get; set; }
      
        public int EmployeeId { get; set; }

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

        public BaseEmployeeDto EmployeeInfo { get; set; }

    }
}
