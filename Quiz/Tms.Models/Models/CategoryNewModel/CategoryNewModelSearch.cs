
using System.Collections.Generic;
using Tms.Models.Common;

namespace Tms.Models.CategoryNewModel
{
    public class CategoryNewSearchModel : Paging
    {
        public string TextSearch { get; set; }
		public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public List<CategoryNewModel> CategoryNews { get; set; }
    }
}