namespace smartshop.Business.Dtos
{
    public class EmployeeDto : BaseEmployeeDto
    {
        public EmployeeDto(int id, string firstName, string lastName, string storeName, string designationName, string email, string phoneNumber, string? photo , bool isActive, DateTime joiningDate, DateTime? resignDate) 
            : base(id, firstName, lastName, storeName, designationName, email, phoneNumber, isActive, photo)
        {
            JoiningDate = joiningDate;
            ResignDate = resignDate;
        }

        public DateTime JoiningDate { get; set; }
        public DateTime? ResignDate { get; set; }
    }
}
