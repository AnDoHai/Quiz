using EcommerceSystem.Core.Configurations;
using Tms.Models.AnswerModel;
using Tms.Services;
using Tms.Services.AutoMap;
using Tms.Web.Framework.Authentication;
using Tms.Web.Framework.Helpers;
using System;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Tms.Web.Controllers
{
    public class AnswerController : BaseController
    {
        private readonly IAnswerService _answerService = (IAnswerService)DependencyResolver.Current.GetService(typeof(IAnswerService));
        private readonly IQuestionService _questionService = (IQuestionService)DependencyResolver.Current.GetService(typeof(IQuestionService));

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Index()
        {
            int totalRecords;
            var model = new AnswerSearchModel
            {
                Answers = _answerService.Search(1, SystemConfiguration.PageSizeDefault, null, null, null, out totalRecords),
                PageIndex = 1,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords
            };

            return View(model);
        }

        public ActionResult Search(int currentPage, string textSearch, string sortColumn, string sortDirection)
        {
            int totalRecords;
            var model = new AnswerSearchModel
            {
                Answers = _answerService.Search(currentPage, SystemConfiguration.PageSizeDefault, textSearch, sortColumn, sortDirection, out totalRecords),
				SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageIndex = currentPage,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords,
            };

            var html = RenderPartialViewToString("~/Views/Answer/Partial/_ListItems.cshtml", model);
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult CreateAnswer()
        {
            var modelQuestion = _questionService.GetAll();
            TempData["QuestionList"] = new SelectList(modelQuestion, "QuestionID", "Title");
            List<SelectListItem> type = new List<SelectListItem>();
            type.Add(new SelectListItem() { Value = "0", Text = "Hình ảnh" });
            type.Add(new SelectListItem() { Value = "1", Text = "Chữ" });
            var model = new AnswerModel();
            TempData["Type"] = type;
            var html = RenderPartialViewToString("~/Views/Answer/Create.cshtml", model);
            return Json(new { IsError = false, HTML = html });
        }

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Create()
        {
            var model = new AnswerModel();
            var modelQuestion = _questionService.GetAll();
            TempData["QuestionList"] = new SelectList(modelQuestion, "QuestionID", "Title");
            List<SelectListItem> type = new List<SelectListItem>();
            type.Add(new SelectListItem() { Value = "0", Text = "Hình ảnh" });
            type.Add(new SelectListItem() { Value = "1", Text = "Chữ" });
            TempData["Type"] = type;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AnswerModel model)
        {
			string returnUrl = Url.Action("Index", "Answer");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = CurrentUser.Email;

                if (_answerService.CreateAnswer(model))
                {
                    TempData["success"] = "Tạo mới thành công";
                    return Redirect(returnUrl);
                }

                TempData["error"] = "Tạo mới thất bại";
            }

            return Redirect(returnUrl);
        }

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult EditAnswer(int id)
        {
            var modelQuestion = _questionService.GetAll();
            TempData["QuestionList"] = new SelectList(modelQuestion, "QuestionID", "Title");
            List<SelectListItem> type = new List<SelectListItem>();
            type.Add(new SelectListItem() { Value = "0", Text = "Đúng/Sai" });
            type.Add(new SelectListItem() { Value = "1", Text = "Chọn 1 đáp án đúng" });
            type.Add(new SelectListItem() { Value = "2", Text = "Sắp xếp thứ tự đúng" });
            type.Add(new SelectListItem() { Value = "3", Text = "Nhập câu trả lời" });
            type.Add(new SelectListItem() { Value = "4", Text = "Viết đoạn văn mô tả" });
            type.Add(new SelectListItem() { Value = "5", Text = "Nhập nội dung hoặc hình ảnh hoặc âm thanh" });
            var model = _answerService.GetById(id);
            TempData["Type"] = type;
            var html = RenderPartialViewToString("~/Views/Answer/Edit.cshtml", model.MapToModel());
            return Json(new { IsError = false, HTML = html });
        }

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Edit(int id)
        {
            var answerEntity = _answerService.GetById(id);
            if(answerEntity != null)
            {
                var model = answerEntity.MapToModel();
                return View(model);
            }
            return View(new AnswerModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AnswerModel model)
        {
            string message;
			string returnUrl = Url.Action("Index", "Answer");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
				model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = CurrentUser.Email;
                var isSuccess = _answerService.UpdateAnswer(model, out message);
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
        public ActionResult Delete(int answerId)
        {
            string message;
            var result = _answerService.Delete(answerId, out message);
            if (result)
            {
                TempData["success"] = message;
                return Json(new { IsError = false });
            }
            return Json(new { IsError = true, Message = message });
        }

		[HttpPost]
        public ActionResult Invisibe(int answerId)
        {
            string message;
            var result = _answerService.ChangeStatus(answerId, out message);
            if (result)
            {
                return Json(new { IsError = false, Message = message });
            }
            return Json(new { IsError = true, Message = message });
        }
    }
}