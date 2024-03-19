using Tms.Models.Common;
using System.Collections.Generic;

namespace Tms.Models.User
{
    public class UserSearchModel : Paging
    {
        public string UserName { get; set; }
        public string TextSearch { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public List<UserModel> Users { get; set; }
    }
}
