
using System.Collections.Generic;
using Tms.Models.Common;

namespace Tms.Models.AnswerModel
{
    public class AnswerSearchModel : Paging
    {
        public string TextSearch { get; set; }
		public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public List<AnswerModel> Answers { get; set; }
    }
}