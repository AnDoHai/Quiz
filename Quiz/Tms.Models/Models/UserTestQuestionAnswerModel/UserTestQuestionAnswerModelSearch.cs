
using System;
using System.Collections.Generic;
using Tms.Models.Common;

namespace Tms.Models.UserTestQuestionAnswerModel
{
    public class UserTestQuestionAnswerSearchModel : Paging
    {
        public string TextSearch { get; set; }
		public string SortColumn { get; set; }
        public int TypeCategory { get; set; }
        public string SortDirection { get; set; }
        public string UrlAudio { get; set; }
        public string UserName { get; set; }
        public int UserTestId { get; set; }
        public string QuizName { get; set; }
        public int TimeLimit { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<UserTestQuestionAnswerModel> UserTestQuestionAnswers { get; set; }
    }
}