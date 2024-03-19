using System;
using System.Collections.Generic;
using Tms.Models.User;

namespace Tms.Models.UserTestModel
{
    public class UserTestModel
    {
		public int UserTestId { get; set; }
		public int? UserId { get; set; }
		public string UserName { get; set; }
		public string QuizName { get; set; }
		public int? QuizID { get; set; }
		public int? HskType { get; set; }
		public int? Type { get; set; }
		public string Title { get; set; }
		public double PassingScore { get; set; }
		public System.DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public bool Status { get; set; }
		public string Description { get; set; }
		public string UrlAudio { get; set; }
		public string HSKName { get; set; }
		public int HSKId { get; set; }
		public double TotalPoint { get; set; }
		public string Email { get; set; }
		public UserModel User { get; set; }
	}
	public class DegreeModel
	{
		public UserModel User { get; set; }
		public string NameHSK { get; set; }
		public int TypeHSK { get; set; }
		public string CertificateFont { get; set; }
		public string CertificateBack { get; set; }
		public double totalListening { get; set; }
		public double totalReading { get; set; }
		public double totalWriting { get; set; }
		public double totalTranslation { get; set; }
		public DateTime DateExam { get; set; }
	}

}
