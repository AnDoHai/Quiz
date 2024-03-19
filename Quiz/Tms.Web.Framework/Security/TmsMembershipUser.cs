using System;
using System.Web.Security;
using Tms.Models.User;

namespace Tms.Web.Framework.Security
{
    public class TmsMembershipUser : MembershipUser
    {
        #region Properties

        public UserCommon UserCommon { get; set; }
        
        #endregion

        public TmsMembershipUser(UserCommon user)
            : base("TmsMembershipProvider", user.UserName, user.FullName, user.Email, string.Empty, string.Empty, true, false, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow)
        {
            UserCommon = user;
        }
    }
}