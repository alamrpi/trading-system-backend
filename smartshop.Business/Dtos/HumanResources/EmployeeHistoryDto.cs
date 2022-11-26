namespace smartshop.Business.Dtos
{
    public class EmployeeHistoryDto
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string Designations { get; set; }
        public string JoiningDate { get; set; }
        public string? ResignDate { get; set; }
    }
}
