using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tms.Models.NewsModel
{
    public class NewsModel
    {
		public int NewsId { get; set; }

		public string Title { get; set; }

		public string SortDescription { get; set; }

		public string Description { get; set; }

		public bool? IsHot { get; set; }

		public System.DateTime CreatedDate { get; set; }

		public string CreatedBy { get; set; }

		public System.DateTime? UpdatedDate { get; set; }

		public string UpdatedBy { get; set; }

		public bool Status { get; set; }

		public string Author { get; set; }

		public int? CategoryNewsId { get; set; }
		public string Image { get; set; }
	}

}
