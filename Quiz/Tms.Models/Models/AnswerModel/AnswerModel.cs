using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tms.Models.AnswerModel
{
    public class AnswerModel
    {
		public int AnswerID { get; set; }
		public string AnswerText { get; set; }
		public int? QuestionID { get; set; }
		public string NameQuestion { get; set; }
		public string NameType { get; set; }
		public string Title { get; set; }
		public System.DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public bool Status { get; set; }
		public string Description { get; set; }
		public int? Type { get; set; }
		public string Code { get; set; }

    }

}
