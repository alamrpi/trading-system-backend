namespace smartshop.Common.Dto
{
    public class PaginationResponse<T> where T : class
    {
        public PaginationResponse()
        {

        }
        public PaginationResponse(int totalRows, IEnumerable<T> rows)
        {
            TotalRows = totalRows;
            Rows = rows;
        }
        public int TotalRows { get; set; }
        public IEnumerable<T> Rows { get; set; }
    }
}
