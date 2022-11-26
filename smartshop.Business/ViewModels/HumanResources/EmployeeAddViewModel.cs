namespace smartshop.Business.ViewModels.HumanResources
{
    public class EmployeeAddViewModel : EmployeePersonalInfoViewModel
    {
        [Required]
        public int DesignationId { get; set; }

        [Required]
        public DateTime JoiningDate { get; set; }
    }
}
