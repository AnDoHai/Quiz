using EcommerceSystem.Core.Configurations;
using System;
using System.Linq;

using System.Web.Mvc;
using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.Repositories;
using Tms.Models.NewsModel;
using Tms.Services;
using Tms.Services.AutoMap;
using System.Web;
using System.IO;
using Tms.Models.CategoryNewModel;

namespace Tms.Web.Controllers
{
    public class NewsController : BaseController
    { 
        private readonly INewsService _newsService = (INewsService)DependencyResolver.Current.GetService(typeof(INewsService));
        private readonly ICategoryNewService _categoryNewService = (ICategoryNewService)DependencyResolver.Current.GetService(typeof(ICategoryNewService));

        public ActionResult Index(int? cateId)
        {
            var GetCateData = _categoryNewService.GetAll().Select(x => new CategoryNewModel {CategoryNewsId= x.CategoryNewsId,Title= x.Title }).ToList();

            ViewData["CategoryList"] = GetCateData;

            //int totalRecords;
            //var model = new NewsSearchModel
            //{
            //    Newss = _newsService.Search(1, SystemConfiguration.PageSizeDefault, null, null, null, out totalRecords),
            //    PageIndex = 1,
            //    PageSize = SystemConfiguration.PageSizeDefault,
            //    TotalRecords = totalRecords
            //};

            if (cateId == null)
            {
                var listNews = new NewsSearchModel();
                listNews.Newss = _newsService.GetAllNewss();
                return View(listNews);
            }
            else
            {
                var listNews = new NewsSearchModel();
                ViewBag.CateId = cateId;
                listNews.Newss = _newsService.GetAllNewss().Where(m => (m.CategoryNewsId) == cateId).ToList();              
                return View(listNews);
            }

        }

        public ActionResult Search(int currentPage, string textSearch, string sortColumn, string sortDirection)
        {
            int totalRecords;
            var model = new NewsSearchModel
            {
                Newss = _newsService.Search(currentPage, SystemConfiguration.PageSizeDefault, textSearch, sortColumn, sortDirection, out totalRecords),
				SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageIndex = currentPage,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords,
            };

            var html = RenderPartialViewToString("~/Views/News/Partial/_ListItems.cshtml", model);
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var GetCateData = _categoryNewService.GetAll().Select(x => new { x.CategoryNewsId, x.Title }).ToList();

            ViewData["CategoryList"] = GetCateData;

            var model = new NewsModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewsModel model, HttpPostedFileBase Image)
        {
			string returnUrl = Url.Action("Index", "News");

            var GetCateData = _categoryNewService.GetAll().Select(x => new { x.CategoryNewsId, x.Title }).ToList();

            ViewData["CategoryList"] = GetCateData;

            if (Image != null && Image.ContentLength > 0)
            {

                string fileName = Path.GetFileName(Image.FileName);

                var urlImage = Path.Combine(Server.MapPath("~/Uploads/NewsImages/"), fileName);

                Image.SaveAs(urlImage);

                ViewData["FileName"] = Image.FileName;

                model.Image = fileName;
            }

            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }

            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = CurrentUser.Email;

                if (_newsService.CreateNews(model))
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

            var GetCateData = _categoryNewService.GetAll().Select(x => new { x.CategoryNewsId, x.Title }).ToList();

            ViewData["CategoryList"] = GetCateData;

            var newsEntity = _newsService.GetById(id);
            if(newsEntity != null)
            {
                var model = newsEntity.MapToModel();
                return View(model);
            }
            return View(new NewsModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsModel model , HttpPostedFileBase Image)
        {
            string message;
			string returnUrl = Url.Action("Index", "News");

            var GetCateData = _categoryNewService.GetAll().Select(x => new { x.CategoryNewsId, x.Title }).ToList();

            ViewData["CategoryList"] = GetCateData;

            if (Image != null && Image.ContentLength > 0)
            {

                string fileName = Path.GetFileName(Image.FileName);

                var urlImage = Path.Combine(Server.MapPath("~/Uploads/NewsImages/"), fileName);

                Image.SaveAs(urlImage);

                ViewData["FileName"] = Image.FileName;

                model.Image = fileName;
            }

            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
				model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = CurrentUser.Email;
                var isSuccess = _newsService.UpdateNews(model, out message);
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
        public ActionResult Delete(int newsId)
        {
            string message;
            var result = _newsService.Delete(newsId, out message);
            if (result)
            {
                TempData["success"] = message;
                return Json(new { IsError = false });
            }
            return Json(new { IsError = true, Message = message });
        }

		[HttpPost]
        public ActionResult Invisibe(int newsId)
        {
            string message;
            var result = _newsService.ChangeStatus(newsId, out message);
            if (result)
            {
                return Json(new { IsError = false, Message = message });
            }
            return Json(new { IsError = true, Message = message });
        }

        public ActionResult NewsDetails(int id)
        {
            var newsEntity = _newsService.GetById(id);
            if(newsEntity != null)
            {
                var model = newsEntity.MapToModel();
                return View(model);
            }
            return View(new NewsModel());
        }
    }
}