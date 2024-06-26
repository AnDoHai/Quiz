﻿using EcommerceSystem.Core.Configurations;
using Tms.Models.UserTestQuestionModel;
using Tms.Services;
using Tms.Services.AutoMap;
using Tms.Web.Framework.Authentication;
using Tms.Web.Framework.Helpers;
using System;
using System.Web.Mvc;

namespace Tms.Web.Controllers
{
    public class UserTestQuestionController : BaseController
    {
        private readonly IUserTestQuestionService _userTestQuestionService = (IUserTestQuestionService)DependencyResolver.Current.GetService(typeof(IUserTestQuestionService));
        
        public ActionResult Index()
        {
            int totalRecords;
            var model = new UserTestQuestionSearchModel
            {
                UserTestQuestions = _userTestQuestionService.Search(1, SystemConfiguration.PageSizeDefault, null, null, null, out totalRecords),
                PageIndex = 1,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords
            };

            return View(model);
        }

        public ActionResult Search(int currentPage, string textSearch, string sortColumn, string sortDirection)
        {
            int totalRecords;
            var model = new UserTestQuestionSearchModel
            {
                UserTestQuestions = _userTestQuestionService.Search(currentPage, SystemConfiguration.PageSizeDefault, textSearch, sortColumn, sortDirection, out totalRecords),
				SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageIndex = currentPage,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords,
            };

            var html = RenderPartialViewToString("~/Views/UserTestQuestion/Partial/_ListItems.cshtml", model);
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var model = new UserTestQuestionModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserTestQuestionModel model)
        {
			string returnUrl = Url.Action("Index", "UserTestQuestion");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = CurrentUser.Email;

                if (_userTestQuestionService.CreateUserTestQuestion(model))
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
            var userTestQuestionEntity = _userTestQuestionService.GetById(id);
            if(userTestQuestionEntity != null)
            {
                var model = userTestQuestionEntity.MapToModel();
                return View(model);
            }
            return View(new UserTestQuestionModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserTestQuestionModel model)
        {
            string message;
			string returnUrl = Url.Action("Index", "UserTestQuestion");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
				model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = CurrentUser.Email;
                var isSuccess = _userTestQuestionService.UpdateUserTestQuestion(model, out message);
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
        public ActionResult Delete(int userTestQuestionId)
        {
            string message;
            var result = _userTestQuestionService.Delete(userTestQuestionId, out message);
            if (result)
            {
                TempData["success"] = message;
                return Json(new { IsError = false });
            }
            return Json(new { IsError = true, Message = message });
        }

		[HttpPost]
        public ActionResult Invisibe(int userTestQuestionId)
        {
            string message;
            var result = _userTestQuestionService.ChangeStatus(userTestQuestionId, out message);
            if (result)
            {
                return Json(new { IsError = false, Message = message });
            }
            return Json(new { IsError = true, Message = message });
        }
    }
}