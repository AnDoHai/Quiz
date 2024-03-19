
using System.Collections.Generic;
using Tms.Models.Common;

namespace Tms.Models.QuizModel
{
    public class QuizSearchModel : Paging
    {
        public string TextSearch { get; set; }
		public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public int CategoryId { get; set; }
        public List<QuizModel> Quizs { get; set; }
    }
}