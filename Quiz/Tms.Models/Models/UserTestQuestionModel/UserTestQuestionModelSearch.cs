
using System.Collections.Generic;
using Tms.Models.Common;

namespace Tms.Models.UserTestQuestionModel
{
    public class UserTestQuestionSearchModel : Paging
    {
        public string TextSearch { get; set; }
		public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public List<UserTestQuestionModel> UserTestQuestions { get; set; }
    }
}