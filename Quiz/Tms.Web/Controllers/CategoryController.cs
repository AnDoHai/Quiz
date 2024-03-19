using EcommerceSystem.Core.Configurations;
using Tms.Models.CategoryModel;
using Tms.Services;
using Tms.Services.AutoMap;
using Tms.Web.Framework.Authentication;
using Tms.Web.Framework.Helpers;
using System;
using System.Web.Mvc;
using Tms.Models.QuizModel;
using System.Linq;
using System.Web;
using System.IO;

namespace Tms.Web.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService = (ICategoryService)DependencyResolver.Current.GetService(typeof(ICategoryService));
        private readonly IQuizService _quizService = (IQuizService)DependencyResolver.Current.GetService(typeof(IQuizService));

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Index()
        {
            int totalRecords;
            var model = new CategorySearchModel
            {
                Categorys = _categoryService.Search(1, SystemConfiguration.PageSizeDefault, null, null, null, out totalRecords),
                PageIndex = 1,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords
            };

            return View(model);
        }

        public ActionResult HSKDetails(int id)
        {
            ViewBag.QuizsByCategory = _quizService.GetByCategoryIndexId(id).Select(x => new QuizModel
            {
                QuizID = x.QuizID,
                QuizName = x.QuizName,
                TimeLimit = x.TimeLimit,
                CategoryId = x.CategoryId
            });
            return View();
        }

        public ActionResult Search(int currentPage, string textSearch, string sortColumn, string sortDirection)
        {
            int totalRecords;
            var model = new CategorySearchModel
            {
                Categorys = _categoryService.Search(currentPage, SystemConfiguration.PageSizeDefault, textSearch, sortColumn, sortDirection, out totalRecords),
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageIndex = currentPage,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords,
            };

            var html = RenderPartialViewToString("~/Views/Category/Partial/_ListItems.cshtml", model);
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Create()
        {
            var model = new CategoryModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryModel model, HttpPostedFileBase file)
        {
            string returnUrl = Url.Action("Index", "Category");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
                //if (file.ContentLength > 0)
                //{
                //    string _FileName = Path.GetFileName(file.FileName);
                //    string _path = Path.Combine(Server.MapPath("/Assets/images/icon"), _FileName);
                //    file.SaveAs(_path);
                //}
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = CurrentUser.Email;

                if (_categoryService.CreateCategory(model))
                {
                    TempData["success"] = "Tạo mới thành công";
                    return Redirect(returnUrl);
                }

                TempData["error"] = "Tạo mới thất bại";
            }

            return Redirect(returnUrl);
        }

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Edit(int id)
        {
            var categoryEntity = _categoryService.GetById(id);
            if (categoryEntity != null)
            {
                var model = categoryEntity.MapToModel();
                return View(model);
            }
            return View(new CategoryModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryModel model)
        {
            string message;
            string returnUrl = Url.Action("Index", "Category");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = CurrentUser.Email;
                var isSuccess = _categoryService.UpdateCategory(model, out message);
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
        public ActionResult Delete(int categoryId)
        {
            string message;
            var result = _categoryService.Delete(categoryId, out message);
            if (result)
            {
                TempData["success"] = message;
                return Json(new { IsError = false });
            }
            return Json(new { IsError = true, Message = message });
        }

        [HttpPost]
        public ActionResult Invisibe(int categoryId)
        {
            string message;
            var result = _categoryService.ChangeStatus(categoryId, out message);
            if (result)
            {
                return Json(new { IsError = false, Message = message });
            }
            return Json(new { IsError = true, Message = message });
        }
    }
}