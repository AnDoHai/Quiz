using System;
using System.Collections.Generic;

namespace Tms.Models.UserTestQuestionModel
{
    public class UserTestQuestionModel
    {
		public int UserTestQuestionId { get; set; }
		public int? UserTestId { get; set; }
		public int? QuestionID { get; set; }
		public string Title { get; set; }
		public System.DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public bool Status { get; set; }
		public string Description { get; set; }
		public int? ContestID { get; set; }
		public int? SectionID { get; set; }

		public List<UserTestQuestionAnswerModel.UserTestQuestionAnswerModel> UserTestQuestionAnswerModels { get; set; }

    }

}
