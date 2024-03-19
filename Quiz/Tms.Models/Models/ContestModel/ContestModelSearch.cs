
using System.Collections.Generic;
using Tms.Models.Common;

namespace Tms.Models.ContestModel
{
    public class ContestSearchModel : Paging
    {
        public string TextSearch { get; set; }
		public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public int QuizId { get; set; }
        public List<ContestModel> Contests { get; set; }
    }
}