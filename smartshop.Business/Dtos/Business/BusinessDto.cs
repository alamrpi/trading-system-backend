namespace smartshop.Business.Dtos.Business
{
    public class BusinessDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string WebAddress { get; set; }
        public string? Logo { get; set; }
        public BusinessDto(int id, string name, bool isActive, string contactNo, string email, string address, string webAddress, string? logo)
        {
            Id = id;
            Name = name;
            IsActive = isActive;
            ContactNo = contactNo;
            Email = email;
            Address = address;
            WebAddress = webAddress;
            Logo = logo;
        }
    }
}
