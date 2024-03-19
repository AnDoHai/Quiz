using EcommerceSystem.Core.Configurations;
using Tms.Models.UserTestQuestionAnswerModel;
using Tms.Services;
using Tms.Services.AutoMap;
using Tms.Web.Framework.Authentication;
using Tms.Web.Framework.Helpers;
using System;
using System.Web.Mvc;

namespace Tms.Web.Controllers
{
    public class UserTestQuestionAnswerController : BaseController
    {
        private readonly IUserTestQuestionAnswerService _userTestQuestionAnswerService = (IUserTestQuestionAnswerService)DependencyResolver.Current.GetService(typeof(IUserTestQuestionAnswerService));
        
        public ActionResult Index()
        {
            int totalRecords;
            var model = new UserTestQuestionAnswerSearchModel
            {
                UserTestQuestionAnswers = _userTestQuestionAnswerService.Search(1, SystemConfiguration.PageSizeDefault, null, null, null, out totalRecords),
                PageIndex = 1,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords
            };

            return View(model);
        }

        public ActionResult Search(int currentPage, string textSearch, string sortColumn, string sortDirection)
        {
            int totalRecords;
            var model = new UserTestQuestionAnswerSearchModel
            {
                UserTestQuestionAnswers = _userTestQuestionAnswerService.Search(currentPage, SystemConfiguration.PageSizeDefault, textSearch, sortColumn, sortDirection, out totalRecords),
				SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageIndex = currentPage,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords,
            };

            var html = RenderPartialViewToString("~/Views/UserTestQuestionAnswer/Partial/_ListItems.cshtml", model);
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var model = new UserTestQuestionAnswerModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserTestQuestionAnswerModel model)
        {
			string returnUrl = Url.Action("Index", "UserTestQuestionAnswer");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = CurrentUser.Email;

                if (_userTestQuestionAnswerService.CreateUserTestQuestionAnswer(model))
                {
                    TempData["success"] = "Tạo mới thành công";
                    return Redirect(returnUrl);
                }

                TempData["error"] = "Tạo mới thất bại";
            }

            return Redirect(returnUrl);
        }

        public ActionResult Edit(int id)
        {
            var userTestQuestionAnswerEntity = _userTestQuestionAnswerService.GetById(id);
            if(userTestQuestionAnswerEntity != null)
            {
                var model = userTestQuestionAnswerEntity.MapToModel();
                return View(model);
            }
            return View(new UserTestQuestionAnswerModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserTestQuestionAnswerModel model)
        {
            string message;
			string returnUrl = Url.Action("Index", "UserTestQuestionAnswer");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
				model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = CurrentUser.Email;
                var isSuccess = _userTestQuestionAnswerService.UpdateUserTestQuestionAnswer(model, out message);
                if (isSuccess)
                {
                    TempData["success"] = message;
                    return Redirect(returnUrl);
                }

                TempData["error"] = message;
            }

            return Redirect(returnUrl);
        }

        [HttpPost]
        public ActionResult Delete(int userTestQuestionAnswerId)
        {
            string message;
            var result = _userTestQuestionAnswerService.Delete(userTestQuestionAnswerId, out message);
            if (result)
            {
                TempData["success"] = message;
                return Json(new { IsError = false });
            }
            return Json(new { IsError = true, Message = message });
        }

		[HttpPost]
        public ActionResult Invisibe(int userTestQuestionAnswerId)
        {
            string message;
            var result = _userTestQuestionAnswerService.ChangeStatus(userTestQuestionAnswerId, out message);
            if (result)
            {
                return Json(new { IsError = false, Message = message });
            }
            return Json(new { IsError = true, Message = message });
        }
    }
}