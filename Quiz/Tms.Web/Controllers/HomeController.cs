using Tms.Models;
using Tms.Models.User;
using Tms.Services;
using Tms.Services.AutoMap;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Security;
using Tms.Models.CategoryModel;

namespace Tms.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly INewsService _newsService = (INewsService)DependencyResolver.Current.GetService(typeof(INewsService));
        private readonly IUserService _useService = (IUserService)DependencyResolver.Current.GetService(typeof(IUserService));
        private readonly IContextService _contextService = (IContextService)DependencyResolver.Current.GetService(typeof(IContextService));
        private readonly IQuizService _quizService = (IQuizService)DependencyResolver.Current.GetService(typeof(IQuizService));
        private readonly ICategoryService _categoryService = (ICategoryService)DependencyResolver.Current.GetService(typeof(ICategoryService));
 
        public ActionResult Index()
        {
            // Lay danh sach to hop cac cap do thi (HSK1-HSKK Cao cap)
            var model = new CategorySearchModel
            {
                Categorys = _categoryService.GetAll().Select(x => new CategoryModel
                {
                    CategoryId = x.CategoryId,
                    Title = x.Title,
                    Type = x.Type,
                    TimeLimit = x.TimeLimit,
                    Description = x.Description
                }).ToList()
            };
      
            return View(model);
        }

        public ActionResult Header()
        {
            return PartialView("~/Views/Shared/Partial/_HeaderPartial.cshtml", CurrentUser);
        }

        public ActionResult LeftMenu()
        {
            return PartialView("~/Views/Shared/Partial/_LeftMenuPartial.cshtml", CurrentUser);
        }

        public ActionResult SwichLanguage(string cultureCode)
        {
            //LanguageUtils.SetLanguage(cultureCode);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult LockScreen()
        {
            ViewBag.Email = CurrentUser.Email;
            ViewBag.Message = "Your contact page.";
            System.Web.Security.FormsAuthentication.SignOut();
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LockScreen(string email, string pass)
        {
            var msgError = "";
            ViewBag.Message = "Your contact page.";
            if (email != null)
            {
                if (pass == "")
                {
                    return Json(new { IsError = true, Message = "Bạn chưa nhập mật khẩu!" });
                }
                var user = _useService.ValidateLogon(email, pass, out msgError);

                if (user.Status == LoginResult.Success)
                {
                    var userData = JsonConvert.SerializeObject(user);
                    var authTicket = new FormsAuthenticationTicket(1,
                        user.Email,
                        DateTime.Now,
                        DateTime.Now.AddDays(4),
                        true,
                        userData);

                    var encTicket = FormsAuthentication.Encrypt(authTicket);
                    var cookieKey = FormsAuthentication.FormsCookieName;

                    _contextService.SaveInCookie(cookieKey, encTicket, 4);
                    //var cookie = new System.Web.HttpCookie(cookieKey) { Value = encTicket.ToString() };
                    //cookie.Expires = DateTime.Now.AddDays(1);
                    //Response.Cookies.Add(cookie);

                    return Json(new { IsError = false, ckname = cookieKey, ticket = encTicket });
                }
                return Json(new { IsError = true, Message = msgError });
            }
            return Json(new { IsError = true, Message = "Tài khoản đã đăng xuất!" });
        }

    }
}