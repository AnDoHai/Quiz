
using System.Collections.Generic;
using Tms.Models.Common;

namespace Tms.Models.NewsModel
{
    public class NewsSearchModel : Paging
    {
        public string TextSearch { get; set; }
		public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public List<NewsModel> Newss { get; set; }
    }
}