
using System.Collections.Generic;
using Tms.Models.Common;

namespace Tms.Models.ChoiceModel
{
    public class ChoiceSearchModel : Paging
    {
        public string TextSearch { get; set; }
		public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public List<ChoiceModel> Choices { get; set; }
    }
}