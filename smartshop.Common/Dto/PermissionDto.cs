namespace smartshop.Common.Dto
{
    public class PermissionDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? PicturePath { get; set; }
        public IList<UserRolesDto> Roles { get; set; }
    }
}
