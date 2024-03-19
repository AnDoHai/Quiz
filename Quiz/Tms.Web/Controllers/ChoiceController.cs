using EcommerceSystem.Core.Configurations;
using Tms.Models.ChoiceModel;
using Tms.Services;
using Tms.Services.AutoMap;
using Tms.Web.Framework.Authentication;
using Tms.Web.Framework.Helpers;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace Tms.Web.Controllers
{
    public class ChoiceController : BaseController
    {
        private readonly IChoiceService _choiceService = (IChoiceService)DependencyResolver.Current.GetService(typeof(IChoiceService));
        private readonly IAnswerService _answerService = (IAnswerService)DependencyResolver.Current.GetService(typeof(IAnswerService));
        private readonly IQuestionService _questionService = (IQuestionService)DependencyResolver.Current.GetService(typeof(IQuestionService));

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Index()
        {
            int totalRecords;
            var model = new ChoiceSearchModel
            {
                Choices = _choiceService.Search(1, SystemConfiguration.PageSizeDefault, null, null, null, out totalRecords),
                PageIndex = 1,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords
            };

            return View(model);
        }

        public ActionResult Search(int currentPage, string textSearch, string sortColumn, string sortDirection)
        {
            int totalRecords;
            var model = new ChoiceSearchModel
            {
                Choices = _choiceService.Search(currentPage, SystemConfiguration.PageSizeDefault, textSearch, sortColumn, sortDirection, out totalRecords),
				SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageIndex = currentPage,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords,
            };

            var html = RenderPartialViewToString("~/Views/Choice/Partial/_ListItems.cshtml", model);
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetChoiceByQuestionId(int id)
        {
            var model = _choiceService.GetChoiceByQuestionId(id);
            var modelAnswer = _answerService.GetByQuestionId(id);
            if (model != null)
            {
                return Json(new { IsError = false,data = model, dataAnswer = modelAnswer });
            }
            return Json(new { IsError = true });
        }
        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Create()
        {
            var modelQuestion = _questionService.GetAll();
            TempData["QuestionList"] = new SelectList(modelQuestion, "QuestionID", "Title");
            List<SelectListItem> type = new List<SelectListItem>();
            type.Add(new SelectListItem() { Value = "0", Text = "Hình ảnh" });
            type.Add(new SelectListItem() { Value = "1", Text = "Chữ" });
            var model = new ChoiceModel();
            TempData["Type"] = type;
            return View(model);
        }

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult CreateChoice()
        {
            var modelQuestion = _questionService.GetAll();
            TempData["QuestionList"] = new SelectList(modelQuestion, "QuestionID", "Title");
            List<SelectListItem> type = new List<SelectListItem>();
            type.Add(new SelectListItem() { Value = "0", Text = "Hình ảnh" });
            type.Add(new SelectListItem() { Value = "1", Text = "Chữ" });
            var model = new ChoiceModel();
            TempData["Type"] = type;
            var html = RenderPartialViewToString("~/Views/Choice/Create.cshtml", model);
            return Json(new { IsError = false, HTML = html });
        }
        public string UploadFile(HttpPostedFileBase file)
        {
            string _path = "";
            var fileTail = file.FileName.Split('.');
            if (file.ContentLength > 0)
            {
                    string _FileName = Path.GetFileName(file.FileName);
                    _path = Path.Combine("/Uploads/Images", _FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Uploads/Images"), _FileName);
                    file.SaveAs(filePath);
            }
            return _path;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ChoiceModel model, HttpPostedFileBase fileName)
        {
			string returnUrl = Url.Action("Index", "Choice");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = "admin@tms.vn";
                if (fileName != null)
                {
                    model.ChoiceText = UploadFile(fileName);
                }
                if (_choiceService.CreateChoice(model))
                {
                    TempData["success"] = "Tạo mới thành công";
                    return Redirect(returnUrl);
                }

                TempData["error"] = "Tạo mới thất bại";
            }

            return Redirect(returnUrl);
        }

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult EditChoice(int id)
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
            var model = _choiceService.GetById(id);
            TempData["Type"] = type;
            var html = RenderPartialViewToString("~/Views/Choice/Edit.cshtml", model.MapToModel());
            return Json(new { IsError = false, HTML = html });
        }
        public ActionResult Edit(int id)
        {
            var choiceEntity = _choiceService.GetById(id);
            if(choiceEntity != null)
            {
                var model = choiceEntity.MapToModel();
                return View(model);
            }
            return View(new ChoiceModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ChoiceModel model)
        {
            string message;
			string returnUrl = Url.Action("Index", "Choice");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
				model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = CurrentUser.Email;
                var isSuccess = _choiceService.UpdateChoice(model, out message);
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
        public ActionResult Delete(int choiceId)
        {
            string message;
            var result = _choiceService.Delete(choiceId, out message);
            if (result)
            {
                TempData["success"] = message;
                return Json(new { IsError = false });
            }
            return Json(new { IsError = true, Message = message });
        }

		[HttpPost]
        public ActionResult Invisibe(int choiceId)
        {
            string message;
            var result = _choiceService.ChangeStatus(choiceId, out message);
            if (result)
            {
                return Json(new { IsError = false, Message = message });
            }
            return Json(new { IsError = true, Message = message });
        }
    }
}