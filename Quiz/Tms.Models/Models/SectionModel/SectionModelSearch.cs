
using System.Collections.Generic;
using Tms.Models.Common;

namespace Tms.Models.SectionModel
{
    public class SectionSearchModel : Paging
    {
        public string TextSearch { get; set; }
		public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public int ContestId { get; set; }
        public List<SectionModel> Sections { get; set; }
    }
}