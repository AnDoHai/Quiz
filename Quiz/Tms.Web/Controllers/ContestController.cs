using EcommerceSystem.Core.Configurations;
using Tms.Models.ContestModel;
using Tms.Services;
using Tms.Services.AutoMap;
using Tms.Web.Framework.Authentication;
using Tms.Web.Framework.Helpers;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Tms.Web.Controllers
{
    public class ContestController : BaseController
    {
        private readonly IContestService _contestService = (IContestService)DependencyResolver.Current.GetService(typeof(IContestService));
        private readonly IQuizService _quizService = (IQuizService)DependencyResolver.Current.GetService(typeof(IQuizService));

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Index()
        {
            int totalRecords;
            var model = new ContestSearchModel
            {
                Contests = _contestService.Search(1, SystemConfiguration.PageSizeDefault, null, null, null, out totalRecords, null),
                PageIndex = 1,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords
            };
            ViewBag.Quiz = _quizService.GetAllQuizsIndex().Select(c => new SelectListItem { Value = c.QuizID.ToString(), Text = c.QuizName + " || " + c.HSKName});

            return View(model);
        }
        public ActionResult GetContestByQuizId(int id)
        {
            var modelQuiz = _contestService.GetAll().Where(c => c.QuizID == id).ToList();
            return Json(new
            {
                data = modelQuiz
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(int currentPage, string textSearch, string sortColumn, string sortDirection, int? quizId)
        {
            int totalRecords;
            var model = new ContestSearchModel
            {
                Contests = _contestService.Search(currentPage, SystemConfiguration.PageSizeDefault, textSearch, sortColumn, sortDirection, out totalRecords, quizId),
				SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageIndex = currentPage,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords,
            };

            var html = RenderPartialViewToString("~/Views/Contest/Partial/_ListItems.cshtml", model);
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Create()
        {
            ViewBag.PhanThis = LoadPhanThi();
            ViewBag.BaiThis = _quizService.GetAllQuizsIndex().Select(c => new SelectListItem { Value = c.QuizID.ToString(), Text = c.QuizName});
            var model = new ContestModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContestModel model)
        {
			string returnUrl = Url.Action("Index", "Contest");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = CurrentUser.Email;
                model.Status = 0;

                if (_contestService.CreateContest(model))
                {
                    TempData["success"] = "Tạo mới thành công";
                    return RedirectToAction("Index", "Contest");
                }

                TempData["error"] = "Tạo mới thất bại";
            }

            return Redirect(returnUrl);
        }

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Edit(int id)
        {
            ViewBag.PhanThis = LoadPhanThi();
            ViewBag.BaiThis = _quizService.GetAllQuizs().Select(c => new SelectListItem { Value = c.QuizID.ToString(), Text = c.QuizName });
            var contestEntity = _contestService.GetById(id);
            if(contestEntity != null)
            {
                var model = contestEntity.MapToModel();
                return View(model);
            }
            return View(new ContestModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContestModel model)
        {
            string message;
			string returnUrl = Url.Action("Index", "Contest");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
				model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = CurrentUser.Email;
                var isSuccess = _contestService.UpdateContest(model, out message);
                if (isSuccess)
                {
                    TempData["success"] = message;
                    return RedirectToAction("Index", "Contest");
                }

                TempData["error"] = message;
            }

            return Redirect(returnUrl);
        }

        [HttpPost]
        public ActionResult Delete(int contestId)
        {
            string message;
            var result = _contestService.Delete(contestId, out message);
            if (result)
            {
                TempData["success"] = message;
                return Json(new { IsError = false });
            }
            return Json(new { IsError = true, Message = message });
        }

		[HttpPost]
        public ActionResult Invisibe(int contestId)
        {
            string message;
            var result = _contestService.ChangeStatus(contestId, out message);
            if (result)
            {
                return Json(new { IsError = false, Message = message });
            }
            return Json(new { IsError = true, Message = message });
        }

        public dynamic LoadPhanThi()
        {
            List<SelectListItem> PhanThis = new List<SelectListItem>();
            PhanThis.Add(new SelectListItem { Value = "1", Text = "Phần thi nghe" });
            PhanThis.Add(new SelectListItem { Value = "2", Text = "Phần thi đọc" });
            PhanThis.Add(new SelectListItem { Value = "3", Text = "Phần thi viết" });
            PhanThis.Add(new SelectListItem { Value = "4", Text = "Phần thi dịch" });
            return PhanThis;
        }

    }
}