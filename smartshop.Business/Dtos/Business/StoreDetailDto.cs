namespace smartshop.Business.Dtos.Business
{
    public class StoreDetailDto : StoreDto
    {
        public StoreDetailDto(int id, string name, string? businessName, string contactNo, string email, string address, string code, bool isActive, List<DeactiveHistoryDto> deactiveHistories, BusinessDto business)
            : base(id, name, businessName, contactNo, email, address, code, isActive)
        {
            DeactiveHistories = deactiveHistories;
            Business = business;
        }

        public List<DeactiveHistoryDto> DeactiveHistories { get; set; }
        public BusinessDto Business { get; set; }
    }
}
