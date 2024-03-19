using EcommerceSystem.Core.Configurations;
using Tms.Models.SectionModel;
using Tms.Services;
using Tms.Services.AutoMap;
using Tms.Web.Framework.Authentication;
using Tms.Web.Framework.Helpers;
using System;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace Tms.Web.Controllers
{
    public class SectionController : BaseController
    {
        private readonly ISectionService _sectionService = (ISectionService)DependencyResolver.Current.GetService(typeof(ISectionService));
        private readonly IContestService _contestService = (IContestService)DependencyResolver.Current.GetService(typeof(IContestService));
        private readonly ICategoryService _categoryService = (ICategoryService)DependencyResolver.Current.GetService(typeof(ICategoryService));

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Index()
        {
            int totalRecords;
            var model = new SectionSearchModel
            {
                Sections = _sectionService.Search(1, SystemConfiguration.PageSizeDefault, null, null, null, out totalRecords, null),
                PageIndex = 1,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords
            };
            ViewBag.HSKList = _categoryService.GetAll().ToList().MapToModels();
            ViewBag.Contest = _contestService.GetAllContestsIndex().Select(c => new SelectListItem { Value = c.ContestID.ToString(), Text = c.ContestName + " || " + c.QuizName });
            return View(model);
        }

        public ActionResult SearchDetail(int currentPage, string textSearch, string sortColumn, string sortDirection,int? hskId,int? quizId, int? contestId)
        {
            int totalRecords;
            var model = new SectionSearchModel
            {
                Sections = _sectionService.SearchDetail(currentPage, SystemConfiguration.PageSizeDefault, textSearch, sortColumn, sortDirection, out totalRecords, hskId, quizId, contestId),
				SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageIndex = currentPage,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords,
            };

            var html = RenderPartialViewToString("~/Views/Section/Partial/_ListItems.cshtml", model);
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(int currentPage, string textSearch, string sortColumn, string sortDirection, int? contestId)
        {
            int totalRecords;
            var model = new SectionSearchModel
            {
                Sections = _sectionService.Search(currentPage, SystemConfiguration.PageSizeDefault, textSearch, sortColumn, sortDirection, out totalRecords, contestId),
				SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageIndex = currentPage,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords,
            };

            var html = RenderPartialViewToString("~/Views/Section/Partial/_ListItems.cshtml", model);
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Create()
        {
            ViewBag.PhanThiChungs = _contestService.GetAllContests().Select(c => new SelectListItem { Value = c.ContestID.ToString(), Text = c.ContestName + " | " + c.QuizName }).ToList();
            var model = new SectionModel();
            return View(model);
        }
        public string UploadFile(HttpPostedFileBase file)
        {
            string _path = "";
            var fileTail = file.FileName.Split('.');
            if (file.ContentLength > 0)
            {
                if (fileTail[1] == "mp3" || fileTail[1] == "mp4")
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    _path = Path.Combine("/Uploads/Audios", _FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Uploads/Audios"), _FileName);
                    file.SaveAs(filePath);
                }
            }
            return _path;
        }
        [HttpPost]
        public ActionResult GetSectionByContestId(int id)
        {
            var modelQuiz = _sectionService.GetByContestId(id);
            var data = modelQuiz;
            var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SectionModel model, HttpPostedFileBase textFile)
        {
			string returnUrl = Url.Action("Index", "Section");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = CurrentUser.Email;
                if (textFile != null)
                {
                    model.Title = UploadFile(textFile);
                }

                model.Status = 0;
                if (_sectionService.CreateSection(model))
                {
                    TempData["success"] = "Tạo mới thành công";
                    return RedirectToAction("Index", "Section");
                }

                TempData["error"] = "Tạo mới thất bại";
            }
            return Redirect(returnUrl);
        }

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Edit(int id)
        {
            ViewBag.PhanThiChungs = _contestService.GetAllContests().Select(c => new SelectListItem { Value = c.ContestID.ToString(), Text = c.ContestName }).ToList();
            var sectionEntity = _sectionService.GetById(id);
            if (sectionEntity != null)
            {
                var model = sectionEntity.MapToModel();
                return View(model);
            }
            return View(new SectionModel());
        }

        public ActionResult GetByContestID(int id)
        {
            var model = _sectionService.GetByContestId(id);
            if (model != null)
            {
                return Json(new { IsError = false, data = model });
            }
            return Json(new { IsError = true,data = model });
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SectionModel model,HttpPostedFileBase textFile)
        {
            string message;
			string returnUrl = Url.Action("Index", "Section");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
				model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = CurrentUser.Email;
                if (textFile != null)
                {
                    model.Title = UploadFile(textFile);
                }
                var isSuccess = _sectionService.UpdateSection(model, out message);
                if (isSuccess)
                {
                    TempData["success"] = message;
                    return RedirectToAction("Index", "Section");
                }

                TempData["error"] = message;
            }

            return Redirect(returnUrl);
        }

        [HttpPost]
        public ActionResult Delete(int sectionId)
        {
            string message;
            var result = _sectionService.Delete(sectionId, out message);
            if (result)
            {
                TempData["success"] = message;
                return Json(new { IsError = false });
            }
            return Json(new { IsError = true, Message = message });
        }

		[HttpPost]
        public ActionResult Invisibe(int sectionId)
        {
            string message;
            var result = _sectionService.ChangeStatus(sectionId, out message);
            if (result)
            {
                return Json(new { IsError = false, Message = message });
            }
            return Json(new { IsError = true, Message = message });
        }
    }
}