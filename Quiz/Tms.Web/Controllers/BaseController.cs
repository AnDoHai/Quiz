using Tms.DataAccess;
using Tms.Models;
using Tms.Models.Common;
using Tms.Models.User;
using Tms.Services;
using Tms.Web.Framework.Security;
using Tms.Web.Hubs;
using log4net;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Tms.Web.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    public abstract class BaseController : Controller
    {
        #region Fields
        protected const string MessagePartialPath = "~/Views/Shared/MesssagePartial";
        protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        /// <summary>
        /// Returns the current user or recovers the user from the <see cref="BaseController()"/>.
        /// </summary>
        /// <remarks>
        /// Using this method requires the user to be authorized.
        /// </remarks>
        private UserCommon _currentUser;
        private readonly IUserService _userService = (IUserService)DependencyResolver.Current.GetService(typeof(IUserService));
        protected UserCommon CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User != null)
                    {
                        var customPrincipal = System.Web.HttpContext.Current.User.Identity as TmsIdentity;
                        if (customPrincipal != null)
                        {
                            _currentUser = customPrincipal.UserCommon;
                        }
                    }

                }
                if (_currentUser == null)
                {
                    return new UserCommon();
                }
                return _currentUser;
            }
            set
            {
                _currentUser = value;
            }
        }
   

        #region Helpers

        public string PartialViewResultToString(PartialViewResult partialViewResult)
        {
            return PartialViewResultToString(partialViewResult, this);
        }
        private string PartialViewResultToString(PartialViewResult partialViewResult, Controller controller)
        {
            StringWriter sw = new StringWriter();

            if (partialViewResult.View == null)
            {
                var result = partialViewResult.ViewEngineCollection.FindPartialView(controller.ControllerContext, partialViewResult.ViewName);
                if (result.View == null)
                    throw new InvalidOperationException(
                                   "Unable to find view. Searched in: " +
                                   string.Join(",", result.SearchedLocations));
                partialViewResult.View = result.View;
            }

            ViewContext viewContext = new ViewContext(controller.ControllerContext
                                        , partialViewResult.View
                                        , partialViewResult.ViewData
                                        , partialViewResult.TempData
                                        , sw);
            partialViewResult.View.Render(viewContext, sw);
            return sw.ToString();
        }
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <param name="model">Model</param>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString(string viewName, object model)
        {
            //Original source code: http://craftycodeblog.com/2010/05/15/asp-net-mvc-render-partial-view-to-string/
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public virtual void InitPaging(int pageIndex, int pageSize, int totalRecord)
        {
            ViewBag.Total = totalRecord;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
        }

        public virtual void ProcessValidatePaging<T>(T paging) where T : Paging, new()
        {
            paging = paging ?? new T();
            paging.PageIndex = paging.PageIndex <= 0 ? 1 : paging.PageIndex;
            paging.PageSize = paging.PageSize <= 0 ? 10 : paging.PageSize;
        }
        #endregion

        /// <summary>
        /// Called when [exception].
        /// </summary>
        /// <param name="exceptionContext">The exception context.</param>
        protected override void OnException(ExceptionContext exceptionContext)
        {
            base.OnException(exceptionContext);
        }

        //protected override void ExecuteCore()
        //{
        //    //var culture = LanguageUtils.GetLanguage();

        //    //Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        //    //Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;

        //    base.ExecuteCore();
        //}
    }
}
