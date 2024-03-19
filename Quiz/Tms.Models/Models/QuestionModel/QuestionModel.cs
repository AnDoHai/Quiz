using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Tms.Models.QuestionModel
{
	public class QuestionModel
	{
		public int QuestionID { get; set; }
		public string QuestionText { get; set; }
		[Required(ErrorMessage = "Phần thi không được để trống!")]
		public int? QuizID { get; set; }
		public int? Maxlength { get; set; }
		public string NameQuiz { get; set; }
		public string AudioUrl { get; set; }
		public string ImageUrl { get; set; }
		public string Title { get; set; }
		public bool StatusTextbox { get; set; }
		public System.DateTime CreatedDate { get; set; }
		public string NameContest { get; set; }
		public int? MinuteStart { get; set; }
		public int? SecondStart { get; set; }
		public int? MinuteEnd { get; set; }
		public int? SecondEnd { get; set; }
		public int GroupIndex { get; set; }
		public string NameSection { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public bool Status { get; set; }
		public string Description { get; set; }
		[Required(ErrorMessage = "Điểm không được để trống!")]
		public double? Point { get; set; }
		public int? Layout { get; set; }
		public int? TimeLimit { get; set; }
		public int? TypeChoice { get; set; }
		[Required(ErrorMessage = "Kiểu câu hỏi không được để trống!")]
		public int? Type { get; set; }
		[Required(ErrorMessage = "Phần thi lớn được để trống!")]
		public int? ContestID { get; set; }
		public int Order { get; set; }
		[Required(ErrorMessage = "Phần thi nhỏ không được để trống!")]
		public int SectionID { get; set; }
		public List<Tms.Models.AnswerModel.AnswerModel> Answers { get; set; }
	}

	public class QuestionAllModel {
		public QuestionModel Question { get; set; }
		public List<Tms.Models.ChoiceModel.ChoiceModel> Choice { get; set; }
		public List<Tms.Models.AnswerModel.AnswerModel> Answer { get; set; }
	}

}
