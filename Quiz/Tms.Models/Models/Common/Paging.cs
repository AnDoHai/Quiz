using System;

namespace Tms.Models.Common
{
    public class Paging
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }
    }
}
