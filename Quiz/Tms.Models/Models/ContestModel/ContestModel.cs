using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tms.Models.ContestModel
{
    public class ContestModel
    {
		public int ContestID { get; set; }
		[Required(ErrorMessage = "Tên không được để trống!")]
		public string ContestName { get; set; }
		public string Title { get; set; }
		public System.DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public int Status { get; set; }
		public string Description { get; set; }
		[Required(ErrorMessage = "Thời gian không được để trống!")]
		public int? TimeLimit { get; set; }
		[Required(ErrorMessage = "Kiểu không được để trống!")]
		public int? Type { get; set; }
		public int? Order { get; set; }
		[Required(ErrorMessage = "Mã đề thi không được để trống!")]
		public int? QuizID { get; set; }
		public string QuizName { get; set; }
		public List<Tms.Models.SectionModel.SectionModel> Sections { get; set; }

	}

}
