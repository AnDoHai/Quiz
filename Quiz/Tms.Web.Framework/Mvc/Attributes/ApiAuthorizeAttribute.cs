using Tms.Web.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;

namespace ShowDC.Presentation.Api.Attributes
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        public ApiAuthorizeAttribute()
        {

        }
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            IEnumerable<string> headerValues;
            ClaimsPrincipal claimsPrincipal = null;
            if (!actionContext.Request.Headers.TryGetValues("access_token", out headerValues) || headerValues == null)
            {
                return false;
            }
            if (!JwtUtil.IsValidToken(headerValues.FirstOrDefault(), out claimsPrincipal) || claimsPrincipal == null)
            {
                return false;
            }
            //Validate username
            var nameClaim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            if (nameClaim != null)
            {
                var domainUser = Membership.GetUserNameByEmail(nameClaim.Value);
                if (!string.IsNullOrEmpty(domainUser))
                {
                    HttpContext.Current.Items["Api_UserName"] = nameClaim.Value;
                    return true;
                }

            }
            return false;
        }
    }
}