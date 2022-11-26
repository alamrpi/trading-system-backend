namespace smartshop.Business.ViewModels.HumanResources
{
    public class EmployeeViewModel
    {
        [Required]
        public int StoreId { get; set; }

        [Required]
        public int DesignationId { get; set; }

        [Required]
        public DateTime JoiningDate { get; set; }
    }
}
