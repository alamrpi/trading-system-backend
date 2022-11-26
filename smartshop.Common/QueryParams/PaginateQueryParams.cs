namespace smartshop.Common.QueryParams
{
    public class PaginateQueryParams
    {
        public int Page { get; set; }
        public int Size { get; set; }

        public PaginateQueryParams()
        {
            Page = 1;
            Size = 50;
        }
    }
}
