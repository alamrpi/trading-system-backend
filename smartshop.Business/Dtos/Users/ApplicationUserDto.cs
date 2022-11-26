namespace smartshop.Business.Dtos.Users
{
    public class ApplicationUserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string BusinessName { get; set; }
        public int BusinessId { get; set; }
        public string StoreName { get; set; }
        public int StoreId { get; set; }
        public string? PhotoUrl { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public ApplicationUserDto(string id, string firstName, string? lastName, string userName, string email, string phoneNumber, string businessName, string storeName,
            string? photoUrl, bool isAdmin, bool isActive)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
            BusinessName = businessName;
            StoreName = storeName;
            PhotoUrl = photoUrl;
            IsActive = isActive;
            IsAdmin = isAdmin;

        }
    }
}
