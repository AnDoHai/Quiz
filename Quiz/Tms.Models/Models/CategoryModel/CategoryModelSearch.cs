
using System.Collections.Generic;
using Tms.Models.Common;

namespace Tms.Models.CategoryModel
{
    public class CategorySearchModel : Paging
    {
        public string TextSearch { get; set; }
		public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public List<CategoryModel> Categorys { get; set; }
    }
}