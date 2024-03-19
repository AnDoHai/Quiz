using System;

namespace Tms.Models.Common
{
    public class SearchModel : Paging
    {
        public Filter OrderField { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public string Code { get; set; }
        public string KeyWord { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } 
    }
}
