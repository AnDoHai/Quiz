using System;
using System.Collections.Generic;
using Tms.Models.CategoryModel;

namespace Tms.Models.QuizModel
{
    public class QuizModel
    {
		public int QuizID { get; set; }
		public string QuizName { get; set; }
		public string Title { get; set; }
		public System.DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public int Status { get; set; }
		public string Description { get; set; }
		public int? CategoryId { get; set; }
		public int? CategoryType { get; set; }
		public int? TimeLimit { get; set; }
		public int? Type { get; set; }
		public string HSKName { get; set; }
		public List<CategoryModel.CategoryModel> Categories { get; set; }

    }

}
