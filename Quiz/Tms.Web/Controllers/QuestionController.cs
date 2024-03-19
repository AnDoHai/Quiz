using EcommerceSystem.Core.Configurations;
using Tms.Models.QuestionModel;
using Tms.Services;
using Tms.Services.AutoMap;
using Tms.Web.Framework.Authentication;
using Tms.Web.Framework.Helpers;
using System;
using System.Web.Mvc;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Tms.Web.Controllers
{
    public class QuestionController : BaseController
    {
        private readonly IQuestionService _questionService = (IQuestionService)DependencyResolver.Current.GetService(typeof(IQuestionService));
        private readonly IQuizService _quizService = (IQuizService)DependencyResolver.Current.GetService(typeof(IQuizService));
        private readonly ISectionService _sectionService = (ISectionService)DependencyResolver.Current.GetService(typeof(ISectionService));
        private readonly IContestService _contestService = (IContestService)DependencyResolver.Current.GetService(typeof(IContestService));
        private readonly ICategoryService _categoryService = (ICategoryService)DependencyResolver.Current.GetService(typeof(ICategoryService));
        private readonly IAnswerService _answerService = (IAnswerService)DependencyResolver.Current.GetService(typeof(IAnswerService));

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Index()
        {
            int totalRecords;
            var model = new QuestionSearchModel
            {
                Questions = _questionService.Search(1, SystemConfiguration.PageSizeDefault, null, null, null, out totalRecords,null,null,null,null),
                PageIndex = 1,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords
            };
            ViewBag.HSKList = _categoryService.GetAll().ToList().MapToModels();
            return View(model);
        }
        public ActionResult SearchQuiz(int quizId)
        {
            var model = _questionService.GetQuestionByQuizId(quizId);
            QuestionSearchModel modelSreach = new QuestionSearchModel()
            {
                Questions = model
            };
            var html = RenderPartialViewToString("~/Views/Question/Partial/_ListItems.cshtml", modelSreach);
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(int currentPage, string textSearch, string sortColumn, string sortDirection,int? hskId, int? quizId,int? contestId,int? sectionId)
        {
            int totalRecords;
            var model = new QuestionSearchModel
            {
                Questions = _questionService.Search(currentPage, SystemConfiguration.PageSizeDefault, textSearch, sortColumn, sortDirection, out totalRecords, hskId, quizId, contestId, sectionId),
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageIndex = currentPage,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords,
            };

            var html = RenderPartialViewToString("~/Views/Question/Partial/_ListItems.cshtml", model);
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult FileUpload()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fname = Path.GetFileName(file.FileName);
                    /// Uploads / Images
                    fname = Path.Combine(Server.MapPath("/Uploads/Images"), fname);
                    var filePath = Path.Combine(Server.MapPath("~/Uploads/Images"), fname);
                    file.SaveAs(fname);
                    var fileTail = filePath.Split('\\');
                    var stringData = "/Uploads/Images/" + fileTail.LastOrDefault();
                    return Json(new
                    {
                        IsError = false,
                        data = stringData,
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        IsError = true,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        public string FileUpload(HttpPostedFileBase file)
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

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult CreateQuestion()
        {
            var model = new QuestionAllModel();
            var quizList = _quizService.GetAll();
            TempData["QuizList"] = new SelectList(quizList, "QuizID", "QuizName");
            var section = _sectionService.GetAll();
            TempData["SectionList"] = new SelectList(section, "SectionID", "SectionName");
            var contest = _contestService.GetAll();
            TempData["ContestList"] = new SelectList(contest, "ContestID", "ContestName");
            List<SelectListItem> type = new List<SelectListItem>();
            type.Add(new SelectListItem() { Value = "0", Text = "Hình ảnh" });
            type.Add(new SelectListItem() { Value = "1", Text = "Chữ" });
            TempData["Type"] = type;
            var html = RenderPartialViewToString("~/Views/Question/Create.cshtml", model);
            return Json(new { IsError = false, HTML = html });
        }

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Create()
        {
            var quizList = _quizService.GetAll();
            TempData["QuizList"] = new SelectList(quizList, "QuizID", "QuizName");
            var section = _sectionService.GetAll();
            TempData["SectionList"] = new SelectList(section, "SectionID", "SectionName");
            var contest = _contestService.GetAll().Select(c => new SelectListItem { Value = c.ContestID.ToString(), Text = c.ContestName + " | " + (c.Quiz != null ? c.Quiz.QuizName : "") });
            TempData["ContestList"] = contest;
            List<SelectListItem> type = new List<SelectListItem>();
            type.Add(new SelectListItem() { Value = "0", Text = "Đúng/Sai" });
            type.Add(new SelectListItem() { Value = "1", Text = "Chọn một đáp án đúng" });
            type.Add(new SelectListItem() { Value = "2", Text = "Sắp xếp thứ tự đúng" });
            type.Add(new SelectListItem() { Value = "3", Text = "Nhập câu trả lời" });
            type.Add(new SelectListItem() { Value = "4", Text = "Viết đoạn mô tả" });
            type.Add(new SelectListItem() { Value = "5", Text = "Đọc nhớ và viết lại câu" });
            type.Add(new SelectListItem() { Value = "6", Text = "Nghe và nói" });
            TempData["Type"] = type;
            List<SelectListItem> order = new List<SelectListItem>();
            order.Add(new SelectListItem() { Value = "0", Text = "Hình ảnh" });
            order.Add(new SelectListItem() { Value = "1", Text = "Chữ" });
            //order.Add(new SelectListItem() { Value = "2", Text = "Âm thanh" });
            TempData["Order"] = order;
            List<SelectListItem> layout = new List<SelectListItem>();
            layout.Add(new SelectListItem() { Value = "0", Text = "Hiển thị ngang 6 cột" });
            layout.Add(new SelectListItem() { Value = "1", Text = "Hiển thị ngang 4 cột" });
            layout.Add(new SelectListItem() { Value = "3", Text = "Hiển thị ngang 3 cột" });
            layout.Add(new SelectListItem() { Value = "2", Text = "Hiển thị ngang 2 cột" });
            layout.Add(new SelectListItem() { Value = "5", Text = "Hiển thị dọc 1 cột" });
            TempData["Layout"] = layout;
            List<SelectListItem> timeLimit = new List<SelectListItem>();
            timeLimit.Add(new SelectListItem() { Value = "0", Text = "Nghe một lần" });
            timeLimit.Add(new SelectListItem() { Value = "1", Text = "Không giới hạn" });
            TempData["TimeLimit"] = timeLimit;
            var model = new QuestionAllModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuestionAllModel model, List<string> Image, HttpPostedFileBase file, HttpPostedFileBase textFileImage)
        {
            string returnUrl = Url.Action("Index", "Question");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            model.Question.Title = "";
            if (model.Question.SecondStart != null && model.Question.MinuteStart != null && model.Question.MinuteEnd != null && model.Question.SecondEnd != null)
            {
                var timeString = model.Question.MinuteStart + ":" + model.Question.SecondStart + "," + model.Question.MinuteEnd + ":" + model.Question.SecondEnd;
                model.Question.Title = timeString;
            }
            if (ModelState.IsValid)
            {
                model.Question.CreatedDate = DateTime.Now;
                model.Question.CreatedBy = CurrentUser.Email;
                var fileName = "";
                if (textFileImage != null)
                {
                    fileName = FileUpload(textFileImage);
                    model.Question.ImageUrl = fileName;
                }
                model.Question.Status = true;
                if (file != null)
                {
                    fileName = UploadFile(file);
                    model.Question.AudioUrl = fileName;
                }
                if (_questionService.CreateQuestionExam(model, Image))
                {
                    TempData["success"] = "Tạo mới thành công";
                    return Redirect("Index");
                }
                TempData["error"] = "Tạo mới thất bại";
            }
            TempData["error"] = "Tạo mới thất bại";

            return Redirect("Index");
        }


        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult EditQuestion(int id)
        {
            var questionEntity = _questionService.GetById(id);
            var quizList = _quizService.GetAll();
            TempData["QuizList"] = new SelectList(quizList, "QuizID", "QuizName");
            TempData["SectionList"] = new SelectList(null, "SectionID", "SectionName");
            var contest = _contestService.GetAllContests() ;
            TempData["ContestList"] = new SelectList(contest, "ContestID", "ContestName");
            var model = questionEntity.MapToModel();
            List<SelectListItem> type = new List<SelectListItem>();
            type.Add(new SelectListItem() { Value = "0", Text = "Đúng/Sai" });
            type.Add(new SelectListItem() { Value = "1", Text = "Chọn một đáp án đúng" });
            type.Add(new SelectListItem() { Value = "2", Text = "Sắp xếp thứ tự đúng" });
            type.Add(new SelectListItem() { Value = "3", Text = "Nhập câu trả lời" });
            type.Add(new SelectListItem() { Value = "4", Text = "Viết đoạn mô tả" });
            type.Add(new SelectListItem() { Value = "5", Text = "Đọc nhớ và viết lại câu" });
            type.Add(new SelectListItem() { Value = "6", Text = "Nghe và nói" });
            TempData["Type"] = type;
            var listAnswer = _answerService.GetAllByQuestionId(id);
            if (listAnswer != null)
            {
                ViewBag.Answer = listAnswer;
            };
            var html = RenderPartialViewToString("~/Views/Question/Edit.cshtml", model);
            return Json(new { IsError = false, HTML = html });
        }
        public ActionResult Edit(int id)
        {
            var questionEntity = _questionService.GetQuestionById(id);
            var quizList = _quizService.GetAll();
            TempData["QuizList"] = new SelectList(quizList, "QuizID", "QuizName");
            var section = _sectionService.GetAllSectionsForEdit((int)questionEntity.ContestID);
            TempData["SectionList"] = new SelectList(section, "SectionID", "SectionName");
            var contest = _contestService.GetContestForEdit((int)questionEntity.QuizID).Select(c => new SelectListItem { Value = c.ContestID.ToString(), Text = c.ContestName + " | " + c.Quiz.QuizName });
            TempData["ContestList"] = contest;
            List<SelectListItem> type = new List<SelectListItem>();
            type.Add(new SelectListItem() { Value = "0", Text = "Đúng/Sai" });
            type.Add(new SelectListItem() { Value = "1", Text = "Chọn một đáp án đúng" });
            type.Add(new SelectListItem() { Value = "2", Text = "Sắp xếp thứ tự đúng" });
            type.Add(new SelectListItem() { Value = "3", Text = "Nhập câu trả lời" });
            type.Add(new SelectListItem() { Value = "4", Text = "Viết đoạn mô tả" });
            type.Add(new SelectListItem() { Value = "5", Text = "Đọc nhớ và viết lại câu" });
            type.Add(new SelectListItem() { Value = "6", Text = "Nghe và nói" });
            TempData["Type"] = type.ToList();
            List<SelectListItem> order = new List<SelectListItem>();
            order.Add(new SelectListItem() { Value = "0", Text = "Hình ảnh", Selected = questionEntity != null && questionEntity.TypeChoice == '0'?true:false });
            order.Add(new SelectListItem() { Value = "1", Text = "Chữ", Selected = questionEntity != null && questionEntity.TypeChoice == '1' ? true : false });
            TempData["Order"] = order;
            List<SelectListItem> layout = new List<SelectListItem>();
            layout.Add(new SelectListItem() { Value = "0", Text = "Hiển thị ngang 6 cột" });
            layout.Add(new SelectListItem() { Value = "1", Text = "Hiển thị ngang 4 cột" });
            layout.Add(new SelectListItem() { Value = "3", Text = "Hiển thị ngang 3 cột" });
            layout.Add(new SelectListItem() { Value = "2", Text = "Hiển thị ngang 2 cột" });
            layout.Add(new SelectListItem() { Value = "5", Text = "Hiển thị dọc 1 cột" });
            TempData["Layout"] = layout;
            var model = new QuestionAllModel();
            model.Question = questionEntity;
            if (questionEntity != null)
            {
                if (questionEntity.Answers != null && questionEntity.Answers.Any())
                {
                    model.Answer = questionEntity.Answers;
                }
            }
            var listAnswer = _answerService.GetAllByQuestionId(id);
            if (listAnswer != null)
            {
                ViewBag.Answer = listAnswer;
            };
            return View(model);
        }

        //change
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionAllModel model, List<string> Image, HttpPostedFileBase file, HttpPostedFileBase textFileImage)
        {
            string message;
            string returnUrl = Url.Action("Index", "Question");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
                var fileName = "";
                model.Question.CreatedBy = CurrentUser.Email;
                if (textFileImage != null)
                {
                    fileName = FileUpload(textFileImage);
                    model.Question.ImageUrl = fileName;
                }
                if (file != null)
                {
                    fileName = UploadFile(file);
                    model.Question.AudioUrl = fileName;
                }
                model.Question.Title = "";
                if (model.Question.SecondStart != null && model.Question.MinuteStart != null && model.Question.MinuteEnd != null && model.Question.SecondEnd != null)
                {
                    var timeString = model.Question.MinuteStart + ":" + model.Question.SecondStart + "," + model.Question.MinuteEnd + ":" + model.Question.SecondEnd;
                    model.Question.Title = timeString;
                }
                model.Question.UpdatedDate = DateTime.Now;
                model.Question.UpdatedBy = CurrentUser.Email;
                var curenName = CurrentUser.Email;
                var isSuccess = _questionService.UpdateQuestion(model, Image, curenName, out message);
                if (isSuccess)
                {
                    TempData["success"] = "Chỉnh sửa thành công!";
                    return RedirectToAction("Index");
                }

                TempData["error"] = "Chỉnh sửa thất bại!";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int questionId)
        {
            string message;
            var result = _questionService.Delete(questionId, out message);
            if (result)
            {
                TempData["success"] = message;
                return Json(new { IsError = false });
            }
            return Json(new { IsError = true, Message = message });
        }

        public ActionResult GetQuestion(int questionId)
        {
            var result = _questionService.GetById(questionId);
            if (result != null)
            {
                return Json(new { IsError = false,data = result });
            }
            return Json(new { IsError = true });
        }

        [HttpPost]
        public ActionResult Invisibe(int questionId)
        {
            string message;
            var result = _questionService.ChangeStatus(questionId, out message);
            if (result)
            {
                return Json(new { IsError = false, Message = message });
            }
            return Json(new { IsError = true, Message = message });
        }
    }
}