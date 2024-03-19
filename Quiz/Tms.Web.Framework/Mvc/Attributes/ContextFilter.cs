using Tms.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;
using System.Web.Http;
using System.Web.Http.Controllers;
using log4net;
using System.Reflection;

namespace ShowDC.Presentation.Api.Attributes
{
    public class ContextFilter : ActionFilterAttribute
    {
        private readonly IContextService _contextService;
        protected static readonly ILog Log = LogManager.GetLogger(MethodInfo.GetCurrentMethod().DeclaringType);
        public ContextFilter()
        {
            _contextService = (IContextService)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IContextService));
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                IEnumerable<string> headerValues;
                if (actionContext.Request.Headers.TryGetValues("language", out headerValues))
                {
                    if (headerValues != null)
                    {
                        _contextService.SetCulture(headerValues.FirstOrDefault());
                    }
                }
                base.OnActionExecuting(actionContext);
            }
            catch (Exception ex)
            {
                Log.Error("Error while trying to set Language context: ", ex);
                throw;
            }
        }
    }
}