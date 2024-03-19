using System;
using System.Collections.Generic;

namespace Tms.Models.UserTestQuestionAnswerModel
{
    public class UserTestQuestionAnswerModel
    {
		public int UserTestQuestionAnswerID { get; set; }
		public string UserTestQuestionAnswerText { get; set; }
		public int? UserTestQuestionID { get; set; }
		public string Title { get; set; }
		public int? QuestionId { get; set; }
		public string QuestionName { get; set; }
		public System.DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public string NameQuestion { get; set; }
		public System.DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public bool Status { get; set; }
		public string Description { get; set; }
		public int? Type { get; set; }
		public string Code { get; set; }
		public double? Point { get; set; }

    }

}
