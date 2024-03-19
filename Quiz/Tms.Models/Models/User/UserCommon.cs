using System.Collections.Generic;
using Tms.Models.Common;

namespace Tms.Models.User
{
    public class UserCommon
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string UserName { get; set; }

        public string Tel { get; set; }

        public string AddressLine1 { get; set; }
        public string[] Roles { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPhuTrachDonvi { get; set; }
        public LoginResult Status { get; set; }
        public Dictionary<string, string> RoleModuleActions { get; set; }
        public string Avatar { get; set; }
        public int XuongId { get; set; }
        public List<int> NhomXuongIds { get; set; }
        public string XuongName { get; set; }
        public int MenuType { get; set; }
        public int DonViId { get; set; }
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
    }
}
