using EcommerceSystem.Core.Configurations;
using Tms.Models.QuizModel;
using Tms.Services;
using Tms.Services.AutoMap;
using Tms.Web.Framework.Authentication;
using Tms.Web.Framework.Helpers;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using Tms.Models.Models.QuizModel;
using System.Linq;
using System.Web;
using System.IO;

namespace Tms.Web.Controllers
{
    public class QuizController : BaseController
    {
        private readonly IQuizService _quizService = (IQuizService)DependencyResolver.Current.GetService(typeof(IQuizService));
        private readonly IQuestionService _questionService = (IQuestionService)DependencyResolver.Current.GetService(typeof(IQuestionService));
        private readonly IContestService _contestService = (IContestService)DependencyResolver.Current.GetService(typeof(IContestService));
        private readonly ICategoryService _categoryService = (ICategoryService)DependencyResolver.Current.GetService(typeof(ICategoryService));

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Index()
        {
            int totalRecords;
            ViewBag.Category = _categoryService.GetAllCategorys().Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Title});
            var model = new QuizSearchModel
            {
                Quizs = _quizService.Search(1, SystemConfiguration.PageSizeDefault, null, null, null, out totalRecords, null),
                PageIndex = 1,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords
            };
            return View(model);
        }
        public ActionResult SortQuizIndex(int id)
        {
            var modelQuiz = _quizService.GetById(id);
            var cateModel = _quizService.GetCategoryByQuizId(id);
            ExamModelSreach model = new ExamModelSreach()
            {
                QuizId = modelQuiz.QuizID,
                QuizName = modelQuiz.QuizName,
                HSKName = cateModel.HSKName,
                HSKType = cateModel.CategoryType != null ? cateModel.CategoryType.Value : 0,
                LimitTime = modelQuiz.TimeLimit.HasValue ? modelQuiz.TimeLimit.Value : 0,
                ExamDetails = _contestService.GetExams(id)
            };
            return View(model);
        }

        public ActionResult IndexExam()
        {
            QuizSearchModel model = new QuizSearchModel()
            {
                Quizs = _quizService.GetAllQuizs()
            };
            return View(model);
        }
        [CustomAuthorize(Roles = RoleHelper.NguoiDungDisplay)]
        public ActionResult ExamQuiz(List<ExamUpload> exam, int quizId,int ? userTestId)
        {
            var userTestIdCount = 0;
            var nameUser = CurrentUser.Email;
            var idUser = CurrentUser.UserId;
            if (userTestId != null)
            {
                var quizHandle = _quizService.ExamQuizUpdate(exam, quizId, idUser, nameUser, userTestId.Value);
                if (quizHandle != false)
                {
                    return Json(new
                    {
                        IsError = false,
                        Data = userTestIdCount,
                        Message = "nộp bài thành công!",
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var quizHandle = _quizService.ExamQuiz(exam, quizId, idUser, nameUser, out userTestIdCount);
                if (quizHandle != false)
                {
                    return Json(new
                    {
                        IsError = false,
                        Data = userTestIdCount,
                        Message = "nộp bài thành công!",
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                IsError = true,
                Message = "nộp bài thất bại!",
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCateByQuizId(int id)
        {
            var dataId = _quizService.GetByQuizId(id);
            return Json(new
            {
                IsError = true,
                data = dataId.CategoryId,
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadSortExam(ExamModel examSorts)
        {
            if (examSorts != null)
            {
                var model = _quizService.UploadQuizSorts(examSorts);
                if (model != false)
                {
                    return Json(new
                    {
                        IsError = false,
                        Message = "Cập nhật thành công!"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                IsError = true,
                Message = "Cập nhật thất bại!"
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetByExamCode(int id, string examHistoryIds)
        {
            bool resetHistory = false;
            var examHids = new List<int>();
            if(!string.IsNullOrEmpty(examHistoryIds))
            {
                int exHistoryId;
                var examHistories = examHistoryIds.Replace(" ", "").Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                examHids = examHistories.Where(x => int.TryParse(x.ToString(), out exHistoryId)).Select(x => int.Parse(x.ToString())).ToList();
            }    
            
            var dataId = _quizService.GetRandomByCodeExam(id, examHids, out resetHistory);

            return Json(new
            {
                IsError = true,
                data = dataId,
                resetHistory = resetHistory
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PostRecordedAudioVideo()
        {
            string _path = "";
            string _pathFileName = "";
            foreach (string upload in Request.Files)
            {
                var file = Request.Files[upload];
                if (file == null) continue;
                string _FileName = CurrentUser.Email + Path.GetFileName(file.FileName);
                _path = Path.Combine("/Uploads/Audios/Exam_Upload", _FileName);
                _path = _path.Replace("\\", "/");
                var filePath = Path.Combine(Server.MapPath("~/Uploads/Audios/Exam_Upload"), _FileName);
                file.SaveAs(filePath);
            }
            return Json(_path);
        }

        public ActionResult UploadExamListing(int quizId, string fileName)
        {
            StatisticalExamFileUpload model = new StatisticalExamFileUpload();
            model.currenUserName = CurrentUser.Email;
            //var nameFileString = fileName.Split('"');
            //var nameFileUrlString = nameFileString[1].Length > 0 ? nameFileString[1].Split('"'):null;
            model.textAnswer = fileName.Replace("\"", "");//nameFileUrlString!=null? string.IsNullOrEmpty(nameFileUrlString[0]) != false? nameFileUrlString[0]:"":"";
            model.QuizId = quizId;
            model.UserId = CurrentUser.UserId;
            var modelQuestion = _quizService.UploadExamFile(model);
            if (modelQuestion != false)
            {
                return Json(new
                {
                    IsError = false,
                }, JsonRequestBehavior.AllowGet);
            };
            return Json(new
            {
                IsError = true,
            }, JsonRequestBehavior.AllowGet);
        }
        // Thi khau ngu - HSKK
        public ActionResult ExamHight(int id)
        {
            var modelQuiz = _quizService.GetById(id);
            var cateModel = _quizService.GetByQuizId(id);
            var contestExamModel = _contestService.GetExams(id);
            ExamHightModel model = new ExamHightModel()
            {
                QuizId = modelQuiz.QuizID,
                QuizName = modelQuiz.QuizName, 
                HSKName = cateModel.HSKName,
                HSKType = cateModel.CategoryType != null? cateModel.CategoryType.ToString():"",
                FileMedia = contestExamModel.SelectMany(c => c.Sections).FirstOrDefault() != null? contestExamModel.SelectMany(c=>c.Sections).FirstOrDefault().Tittle:"",
                LimitTime = modelQuiz.TimeLimit.HasValue ? modelQuiz.TimeLimit.Value : 0,
                Sections = contestExamModel.SelectMany(c=>c.Sections).ToList(),
                Questions = contestExamModel.SelectMany(c=>c.Sections.SelectMany(i=>i.QuestionModels)).ToList()
            };
            return View(model);
        }
        // Thi HSK
        [CustomAuthorize(Roles = RoleHelper.NguoiDungDisplay)]
        public ActionResult Exam(int id)
        {
            var modelQuiz = _quizService.GetById(id);
            var cateModel = _quizService.GetCategoryByQuizId(id);
            ExamModelSreach model = new ExamModelSreach()
            {
                QuizId = modelQuiz.QuizID,
                QuizName = modelQuiz.QuizName,
                HSKName = cateModel.HSKName,
                HSKType = cateModel.CategoryType != null? cateModel.CategoryType.Value:0,
                LimitTime = modelQuiz.TimeLimit.HasValue ? modelQuiz.TimeLimit.Value : 0,
                ExamDetails = _contestService.GetExams(id)
            };
            return View(model);
        }

        // Index search
        //public ActionResult Search(int currentPage, string textSearch, string sortColumn, string sortDirection, int? categoryId)
        public ActionResult GetQuizByCateId(int id)
        {
            var modelQuiz = _quizService.GetAll().Where(c=>c.CategoryId == id).ToList();
            return Json(new
            {
                data = modelQuiz
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(int currentPage, string textSearch, string sortColumn, string sortDirection, int? categoryId)
        {
            int totalRecords;
           
            var model = new QuizSearchModel
            {
                Quizs = _quizService.Search(currentPage, SystemConfiguration.PageSizeDefault, textSearch, sortColumn, sortDirection, out totalRecords, categoryId),
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageIndex = currentPage,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords,
            };

            var html = RenderPartialViewToString("~/Views/Quiz/Partial/_ListItems.cshtml", model);
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }

        #region CRUD
        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]

        public ActionResult Create()
        {
            ViewBag.TrangThais = LoadTrangThai();
            ViewBag.DanhMucs = _categoryService.GetAllCategorys().Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Title });
            var model = new QuizModel();
            return View(model);
        }

        public ActionResult GetAllQuiz(int id)
        {
            var modelQuiz = _contestService.GeyByQUizId(id);
            return Json(new { IsError = false, data = modelQuiz });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuizModel model)
        {
            string returnUrl = Url.Action("Index", "Quiz");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = CurrentUser.Email;
                if (_quizService.CreateQuiz(model))
                {
                    TempData["success"] = "Tạo mới thành công";
                    return RedirectToAction("Index", "Quiz");
                }

                TempData["error"] = "Tạo mới thất bại";
            }

            return Redirect(returnUrl);
        }
        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Edit(int id)
        {
            ViewBag.TrangThais = LoadTrangThai();
            ViewBag.DanhMucs = _categoryService.GetAllCategorys().Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Title });

            var quizEntity = _quizService.GetById(id);
            if (quizEntity != null)
            {
                var model = quizEntity.MapToModel();
                return View(model);
            }
            return View(new QuizModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuizModel model)
        {
            string message;
            string returnUrl = Url.Action("Index", "Quiz");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = CurrentUser.Email;
                var isSuccess = _quizService.UpdateQuiz(model, out message);
                if (isSuccess)
                {
                    TempData["success"] = message;
                    return RedirectToAction("Index", "Quiz");
                }

                TempData["error"] = message;
                return Redirect(returnUrl);
            }

            return Redirect(returnUrl);
        }

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        [HttpPost]
        public ActionResult Delete(int quizId)
        {
            string message;
            var result = _quizService.Delete(quizId, out message);
            if (result)
            {
                TempData["success"] = message;
                return Json(new { IsError = false });
            }
            return Json(new { IsError = true, Message = message });
        }
        #endregion

        [HttpPost]
        public ActionResult Invisibe(int quizId)
        {
            string message;
            var result = _quizService.ChangeStatus(quizId, out message);
            if (result)
            {
                return Json(new { IsError = false, Message = message });
            }
            return Json(new { IsError = true, Message = message });
        }

        public dynamic LoadTrangThai()
        {
            List<SelectListItem> TrangThais = new List<SelectListItem>();
            TrangThais.Add(new SelectListItem { Value = "1", Text = "Khởi tạo" });
            TrangThais.Add(new SelectListItem { Value = "2", Text = "Đang thi" });
            TrangThais.Add(new SelectListItem { Value = "3", Text = "Kết thúc" });
            return TrangThais;
        }
    }
}