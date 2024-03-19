using System;
using System.Collections.Generic;

namespace Tms.Models.CategoryModel
{
    public class CategoryModel
    {
		public int CategoryId { get; set; }
		public string Title { get; set; }
		public System.DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public bool Status { get; set; }
		public string Description { get; set; }
		public int? Type { get; set; }
		public int? TimeLimit { get; set; }
		public string CertificateImageFont { get; set; }
		public string CertificateImageBack { get; set; }
		public int? PassingScore { get; set; }
	}

}
