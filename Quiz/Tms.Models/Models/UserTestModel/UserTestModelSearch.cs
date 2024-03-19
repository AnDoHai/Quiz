
using System.Collections.Generic;
using Tms.Models.Common;

namespace Tms.Models.UserTestModel
{
    public class UserTestSearchModel : Paging
    {
        public string TextSearch { get; set; }
		public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public List<UserTestModel> UserTests { get; set; }
    }
}