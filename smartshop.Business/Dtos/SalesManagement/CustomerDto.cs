using smartshop.Common.Enums.SalesManagement;

namespace smartshop.Business.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descriptions { get; set; }
        public string Identifier { get; set; }
        public string Mobile { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public CustomerTypes Type { get; set; }
    }
}
