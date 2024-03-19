using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tms.Models.SectionModel
{
    public class SectionModel
    {
		public int SectionID { get; set; }
		[Required(ErrorMessage = "Tên phần thi không được để trống!")]
		public string SectionName { get; set; }
		public string Title { get; set; }
		public System.DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public int Status { get; set; }
		public string Description { get; set; }
		public string TimeLimit { get; set; }
		[Required(ErrorMessage = "Kiểu không được để trống!")]
		public int Type { get; set; }
		[Required(ErrorMessage = "Thứ tự không được để trống!")]
		public int OrderIndex { get; set; }
		public int? Order { get; set; }
		public int? QuizID { get; set; }
		[Required(ErrorMessage = "Phần thi chung không được để trống!")]
		public int? ContestID { get; set; }
		public int? MinuteStart { get; set; }
		public int? MinuteEnd { get; set; }
		public int? SecoundStart { get; set; }
		public int? SecoundEnd { get; set; }
		public string QuizName { get; set; }
		public string ContestName { get; set; }
		public List<Tms.Models.QuestionModel.QuestionModel> Questions { get; set; }

    }

}
