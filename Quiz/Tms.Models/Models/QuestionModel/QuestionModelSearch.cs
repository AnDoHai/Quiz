
using System.Collections.Generic;
using Tms.Models.Common;

namespace Tms.Models.QuestionModel
{
    public class QuestionSearchModel : Paging
    {
        public string TextSearch { get; set; }
		public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public List<QuestionModel> Questions { get; set; }
    }
}