using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartshop.Common.QueryParams.Accountant
{
    public class IncomeExpenseQueryParams : PaginateQueryParams
    {
        public int? HeadId { get; set; }
        public int? Income { get; set; }
        public DateTime? Date { get; set; }
    }
}
