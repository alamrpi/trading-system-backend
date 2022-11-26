namespace smartshop.Business.Dtos.Business
{
    public class DeactiveHistoryDto
    {
        public int Id { get; set; }
        public string Descriptions { get; set; }
        public DateTime DeactiveDate { get; set; }

        public DateTime? ReActivateDate { get; set; }
        public DeactiveHistoryDto(int id, string descriptions, DateTime deactiveDate, DateTime? reActivateDate)
        {
            Id = id;
            Descriptions = descriptions;
            DeactiveDate = deactiveDate;
            ReActivateDate = reActivateDate;
        }
    }
}
