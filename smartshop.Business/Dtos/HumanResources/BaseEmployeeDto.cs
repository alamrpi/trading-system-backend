namespace smartshop.Business.Dtos
{
    public class BaseEmployeeDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StoreName { get; set; }
        public string DesignationName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Photo { get; set; }
        public bool IsActive { get; set; }
        public BaseEmployeeDto(int id, string firstName, string lastName,string storeName, string designationName, string email, string phoneNumber, bool isActive, string? photo)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            FullName = $"{firstName} {lastName}";
            StoreName = storeName;
            DesignationName = designationName;
            Email = email;
            PhoneNumber = phoneNumber;
            IsActive = isActive;
            Photo = photo;
        }
    }
}
