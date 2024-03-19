using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Tms.Web.Framework.Security
{
    public class TmsPrincipal : IPrincipal
    {
        #region Implementation of IPrincipal

        public bool IsInRole(string roleName)
        {
            if(TmsIdentity.UserCommon != null && TmsIdentity.UserCommon.IsAdmin)
            {
                return true;
            }
            var roleModuleActions = (TmsIdentity.UserCommon != null && TmsIdentity.UserCommon.RoleModuleActions != null)
                ? TmsIdentity.UserCommon.RoleModuleActions : new Dictionary<string, string>();

            if (roleModuleActions.ContainsKey(roleName.ToLower()))
                return true;

            return false;
        }

        /// <summary>
        /// Gets the identity of the current principal.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Security.Principal.IIdentity"/> object associated with the current principal.
        /// </returns>
        public IIdentity Identity { get; private set; }

        #endregion

        public TmsIdentity TmsIdentity { get { return (TmsIdentity)Identity; } set { Identity = value; } }

        public TmsPrincipal(TmsIdentity identity)
        {
            Identity = identity;
        }
    }
}