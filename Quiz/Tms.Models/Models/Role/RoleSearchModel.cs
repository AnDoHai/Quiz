using Tms.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tms.Models.Role
{
    public class RoleSearchModel : Paging
    {
        public string TextSearch { get; set; }

        public List<RoleModel> Roles { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
    }
}
