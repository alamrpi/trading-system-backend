namespace smartshop.Business.Dtos
{
    public class EmployeeDetailsDto : EmployeeDto
    {
        public EmployeeDetailsDto(int id, string firstName, string lastName, string storeName, string designationName, string email, 
            string phoneNumber, bool isActive, string? photo, DateTime joiningDate, DateTime? resignDate,
            string gender, string fatherName, string nationalIdNumber, DateTime dateOfBirth, string address) 
            : base(id, firstName, lastName, storeName, designationName, email, phoneNumber, photo, isActive, joiningDate, resignDate)
        {
            Gender = gender;
            FatherName = fatherName;
            NationalIdNumber = nationalIdNumber;
            DateOfBirth = dateOfBirth;
            Address = address;
        }

        public string Gender { get; set; }
 
        public string FatherName { get; set; }

        public string NationalIdNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }

        public int StoreId { get; set; }
        public int DesignationId { get; set; }

        public IList<EmployeeHistoryDto> EmployeeHistories { get; set; }
    }
}
