﻿using System;
using System.Collections.Generic;

namespace Tms.Models.CategoryNewModel
{
    public class CategoryNewModel
    {
		public int CategoryNewsId { get; set; }
		public string Title { get; set; }
		public System.DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime? UpdatedDate { get; set; }
		public string UpdatedBy { get; set; }
		public bool Status { get; set; }
		public string Description { get; set; }
		public int? Type { get; set; }

    }

}
