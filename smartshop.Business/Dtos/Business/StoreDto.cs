namespace smartshop.Business.Dtos.Business
{
    public class StoreDto
    {
        public int Id { get; set; }     
        public string Name { get; set; }
        public string? BusinessName { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public StoreDto(int id, string name, string? businessName, string contactNo, string email, string address, string code, bool isActive)
        {
            Id = id;
            Name = name;
            BusinessName = businessName;
            ContactNo = contactNo;
            Email = email;
            Address = address;
            Code = code;
            IsActive = isActive;
        }
    }
}
