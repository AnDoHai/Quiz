using EcommerceSystem.Core.Configurations;
using Tms.Models.UserTestModel;
using Tms.Services;
using Tms.Services.AutoMap;
using Tms.Web.Framework.Authentication;
using Tms.Web.Framework.Helpers;
using System;
using System.Web.Mvc;
using Tms.Models.UserTestQuestionAnswerModel;
using Tms.Models.Models.QuizModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tms.Web.Controllers
{
    public class UserTestController : BaseController
    {
        private readonly IUserTestService _userTestService = (IUserTestService)DependencyResolver.Current.GetService(typeof(IUserTestService));
        private readonly IUserService _userService = (IUserService)DependencyResolver.Current.GetService(typeof(IUserService));
        private readonly IUserTestQuestionService _userTestQuestionService = (IUserTestQuestionService)DependencyResolver.Current.GetService(typeof(IUserTestQuestionService));
        private readonly IUserTestQuestionAnswerService _userTestQuestionAnswerService = (IUserTestQuestionAnswerService)DependencyResolver.Current.GetService(typeof(IUserTestQuestionAnswerService));
        private readonly IQuizService _quizService = (IQuizService)DependencyResolver.Current.GetService(typeof(IQuizService));
        private readonly IQuestionService _questionService = (IQuestionService)DependencyResolver.Current.GetService(typeof(IQuestionService));
        private readonly IContestService _contestService = (IContestService)DependencyResolver.Current.GetService(typeof(IContestService));
        private readonly ICategoryService _categoryService = (ICategoryService)DependencyResolver.Current.GetService(typeof(ICategoryService));

        // Module chấm thi
        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Index()
        {
            int totalRecords;
            var model = new UserTestSearchModel
            {
                UserTests = _userTestService.Search(1, SystemConfiguration.PageSizeDefault, null, null, null,null, out totalRecords),
                PageIndex = 1,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords
            };

            return View(model);
        }

        //public ActionResult UpdatePointExamAll()
        //{
        //    var model = _userTestService.
        //    return View("Index");
        //}

        [HttpPost]
        public ActionResult DeleteRecordedAudioVideo(int userTestId)
        {
            var modelUserTest = _userTestService.GetAudioForDelete(userTestId);
            var message = "";
            var stringNameFile = modelUserTest.Split('/').Last();
            if (!String.IsNullOrEmpty(stringNameFile))
            {
                if (System.IO.File.Exists(Path.Combine("/Uploads/Audios/", stringNameFile)))
                {
                    // If file found, delete it   
                    System.IO.File.Delete(Path.Combine("/Uploads/Audios/", stringNameFile));
                }
            }
            return Json(new
            {
                IsError = false,
                Message = "xóa thành công!",
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateTotalPoint(int userTestId)
        {
            var model = _userTestService.UpdatePoint(userTestId);
            return Json(new
            {
                IsError = false,
                data = model,
                Message = "cập nhật thành công!",
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateQuiz(int quizId,int questionId)
        {
            var model = _userTestService.UpdateExam(questionId, quizId);
            return Redirect("/Home/Index");
        }

        public ActionResult UpdateQuizPoint(int quizId)
        {
            var model = _userTestService.UpdateAllExam(quizId);
            return Redirect("/Home/Index");
        }

        // Danh sách bài thi
        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult ExamList()
        {
            int totalRecords;
            var model = new UserTestSearchModel
            {
                UserTests = _userTestService.SearchAllUsers(1, SystemConfiguration.PageSizeDefault, null, null, null,null, out totalRecords),
                PageIndex = 1,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords,
            };

            return View(model);
        }
        [CustomAuthorize(Roles = RoleHelper.NguoiDungDisplay)]
        public ActionResult ExamHistoryCertificate()
        {
            int totalRecords;
            int userId = CurrentUser.UserId;
            var model = new UserTestSearchModel
            {
                UserTests = _userTestService.ExamHistoryCertificate(1, SystemConfiguration.PageSizeDefault, null, null, null, out totalRecords, userId),
                PageIndex = 1,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords
            };
            return View(model);
        }
        // Lịch sử thi (by userId)
        [CustomAuthorize(Roles = RoleHelper.NguoiDungDisplay)]
        public ActionResult ExamHistory()
        {
            int totalRecords;
            int userId = CurrentUser.UserId;
            var model = new UserTestSearchModel
            {
                UserTests = _userTestService.SearchByUser(1, SystemConfiguration.PageSizeDefault, null, null, null, out totalRecords, userId),
                PageIndex = 1,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords
            };
            return View(model);
        }
        //[HttpGet]
        //public ActionResult UpdateAllExam(int quizId)
        //{
        //    var model = _userTestService.UpdateUserTestAnswer(questionId, quizId);
        //    return RedirectToAction("Index");
        //}

        [HttpGet]
        public ActionResult InsertMutilRowUserTest(int questionId,int quizId)
        {
            var model = _userTestService.UpdateUserTestAnswer(questionId, quizId);
            return RedirectToAction("Index");
        }

        // Kết quả thi (by userTestId)
        [CustomAuthorize(Roles = RoleHelper.NguoiDungDisplay)]
        public ActionResult ExamResult(int id)
        {
            var examModel = _userTestService.GetQuizById(id);
            if (examModel == null || examModel.Quiz == null)
            {
                return View(new ExamResultModel());
            }
            ExamResultModel model = new ExamResultModel
            {
                UserTestID = examModel.UserTestId,
                UserID = examModel.UserId,
                QuizID = examModel.QuizID,
                QuizExam = new QuizExamResult()
                {
                    QuizID = examModel.Quiz.QuizID,
                    QuizName = examModel.Quiz.Category.Title,
                    ContestExams = _contestService.GetExamsResult(examModel.QuizID.Value,id)
                },
                UserTestQuestions = _userTestQuestionService.GetUserTestQuestion(examModel.UserTestId)
            };
            ViewBag.ModelStatistical = _userTestService.StatisticalAllExam(id);
            return View(model);
        }

        [CustomAuthorize(Roles = RoleHelper.NguoiDungDisplay)]
        public ActionResult ExamResultHight(int id)
        {
            var examModel = _userTestService.GetQuizById(id);
            if (examModel == null || examModel.Quiz == null)
            {
                return View(new ExamResultModelHight());
            }
            var userTetsHightModel = _userTestService.GetExamsResultHight(id);
            ExamResultModelHight model = new ExamResultModelHight
            {
                UserTestID = examModel.UserTestId,
                ExamName = examModel.Quiz.QuizName,
                CraeteDate = examModel.CreatedDate,
                TotalQuestion = examModel.Quiz.Questions.Count(),
                Status = userTetsHightModel.FirstOrDefault().Status,
                TotalPoint = userTetsHightModel.Sum(c=>c.Point),
                SectionID = userTetsHightModel.FirstOrDefault().SectionID,
                SectionName = userTetsHightModel.FirstOrDefault().SectionName,
                ContestID = userTetsHightModel.FirstOrDefault().ContestID,
                ContestName = userTetsHightModel.FirstOrDefault().ContestName,
                UserID = examModel.UserId,
                QuizID = examModel.QuizID,
                UserTestExams = userTetsHightModel

            };
            return View(model);
        }

        public ActionResult Search(int currentPage, string textSearch, string sortColumn, string sortDirection,string dateTime)
        {
            int totalRecords;
            var model = new UserTestSearchModel
            {
                UserTests = _userTestService.Search(currentPage, SystemConfiguration.PageSizeDefault, textSearch, sortColumn, sortDirection, dateTime, out totalRecords),
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageIndex = currentPage,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords,
            };

            var html = RenderPartialViewToString("~/Views/UserTest/Partial/_ListItems.cshtml", model);
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }
        // Tìm kiếm danh sách bài thi
        public ActionResult SearchAllUsers(int currentPage, string textSearch, string sortColumn, string sortDirection,string stringTime)
        {
            int totalRecords;
            var model = new UserTestSearchModel
            {
                UserTests = _userTestService.SearchAllUsers(currentPage, SystemConfiguration.PageSizeDefault, textSearch, sortColumn, sortDirection, stringTime, out totalRecords),
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageIndex = currentPage,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords,
            };

            var html = RenderPartialViewToString("~/Views/UserTest/Partial/_ListItemsAllExams.cshtml", model);
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }

        // Tìm kiếm lịch sử thi (current user)
        public ActionResult SearchByUser(int currentPage, string textSearch, string sortColumn, string sortDirection)
        {
            int totalRecords;
            int userId = CurrentUser.UserId;
            var model = new UserTestSearchModel
            {
                UserTests = _userTestService.SearchByUser(currentPage, SystemConfiguration.PageSizeDefault, textSearch, sortColumn, sortDirection, out totalRecords, userId),
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageIndex = currentPage,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords,
            };

            var html = RenderPartialViewToString("~/Views/UserTest/Partial/_ListItemsExamHistory.cshtml", model);
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeletePDF(int userTestId)
        {
            var userTestModel = _userTestService.DeleteFilePDF(userTestId);
            var stringFilePDF = userTestModel;
            if (!String.IsNullOrEmpty(stringFilePDF))
            {
                var filePath = Server.MapPath(stringFilePDF);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            return Json(new
            {
                IsError = false,
                Message = "cập nhật thành công!",
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MarkTestExam(UserTestQuestionAnswerSearchModel model)
        {
            if (model != null)
            {
                var updateModel = _userTestQuestionAnswerService.MarkExamTest(model.UserTestQuestionAnswers,model.UserTestId);
            }
            return RedirectToAction("Index");
        }

        public ActionResult PrintDegree(int id)
        {
            string message = "";
            string printFilePath = "";
            try
            {
                var userTestEntity = _userTestService.GetById(id);
                if (userTestEntity != null)
                {
                    if (string.IsNullOrEmpty(userTestEntity.Description))
                    {
                        UserTestModel utModel = userTestEntity.MapToModel();
                        var model = _userTestQuestionAnswerService.Degree(id);
                        if (model != null)
                        {
                            var domainUrl = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);//Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port
                            model.CertificateFont = domainUrl + model.CertificateFont;
                            model.CertificateBack = domainUrl + model.CertificateBack;
                            if (!string.IsNullOrEmpty(model.User.Thumbnail))
                            {
                                model.User.Thumbnail = domainUrl + model.User.Thumbnail;
                            }
                            var html = RenderPartialViewToString("~/Views/UserTest/Partial/_degreePartial.cshtml", model);
                            string fileName = model.User != null && !string.IsNullOrEmpty(model.User.EmployerCode) ? "Degree" + model.User.EmployerCode + DateTime.Now.ToString("mmss") : "Degree" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                            string filePath = Path.Combine(Server.MapPath("~/Uploads/DegreeFiles/"), fileName + ".pdf");
                            System.IO.File.WriteAllBytes(filePath, PdfSharpConvert(html));
                            printFilePath = "/Uploads/DegreeFiles/" + fileName + ".pdf";
                            utModel.Description = printFilePath;
                            _userTestService.UpdateUserTest(utModel, out message);
                        }
                        else
                        {
                            message = "Cập nhật thất bại!";
                            return Json(new { IsError = true, PrintFilePath = printFilePath, Message = message });
                        }
                    }
                    else
                    {
                        printFilePath = userTestEntity.Description;
                    }
                    return Json(new { IsError = false, PrintFilePath = printFilePath, Message = message });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
            return Json(new { IsError = true, PrintFilePath = printFilePath, Message = message });


        }
        [HttpGet]
        public FileContentResult DegreeExport(int id)
        {
            var model = _userTestQuestionAnswerService.Degree(id);
            var domainUrl = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);//Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port
            model.CertificateFont = domainUrl + model.CertificateFont;
            model.CertificateBack = domainUrl + model.CertificateBack;
            if (!string.IsNullOrEmpty(model.User.Thumbnail))
            {
                model.User.Thumbnail = domainUrl + model.User.Thumbnail;
            }
            var html = RenderPartialViewToString("~/Views/UserTest/Partial/_degreePartial.cshtml", model);
            string fileName = model.User != null && !string.IsNullOrEmpty(model.User.EmployerCode) ? "Degree" + model.User.EmployerCode : "Degree" + DateTime.Now.ToString("ddMMyyyy");
            return File(PdfSharpConvert(html), "application/pdf", fileName + ".pdf");
        }
        public ActionResult MarkTest(int id)
        {
            var userModel = _userService.GetById((int)_userTestService.GetById(id).UserId);
            var quizModel = _userTestService.GetQuizById(id);
            var userTestAnswerModel = _userTestQuestionAnswerService.GetAllUserTestQuestionAnswersById(id);
            var model = new UserTestQuestionAnswerSearchModel()
            {
                QuizName = quizModel.Quiz.QuizName,
                UserName = userModel.UserName,
                UserTestId = id,
                TypeCategory = (int)quizModel.Quiz.Category.Type,
                UrlAudio = userTestAnswerModel != null? userTestAnswerModel[0].UserTestQuestionAnswerText:"",
                TimeLimit = quizModel.Quiz.TimeLimit != null? (int)quizModel.Quiz.TimeLimit: 0,
                CreatedDate = quizModel.Quiz.CreatedDate,
                UserTestQuestionAnswers = userTestAnswerModel != null && userTestAnswerModel.Any()? userTestAnswerModel:null
            };
            return View(model);
        }


        public ActionResult Create()
        {
            var model = new UserTestModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserTestModel model)
        {
            string returnUrl = Url.Action("Index", "UserTest");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = CurrentUser.Email;

                if (_userTestService.CreateUserTest(model))
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
            var userTestEntity = _userTestService.GetById(id);
            if (userTestEntity != null)
            {
                var model = userTestEntity.MapToModel();
                return View(model);
            }
            return View(new UserTestModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserTestModel model)
        {
            string message;
            string returnUrl = Url.Action("Index", "UserTest");
            if (Request.UrlReferrer != null && !string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                returnUrl = Request.UrlReferrer.ToString();
            }
            if (ModelState.IsValid)
            {
                model.UpdatedDate = DateTime.Now;
                model.UpdatedBy = CurrentUser.Email;
                var isSuccess = _userTestService.UpdateUserTest(model, out message);
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
        public ActionResult Delete(int userTestId)
        {
            string message;
            var result = _userTestService.Delete(userTestId, out message);
            if (result)
            {
                TempData["success"] = message;
                return Json(new { IsError = false });
            }
            return Json(new { IsError = true, Message = message });
        }

        [HttpPost]
        public ActionResult Invisibe(int userTestId)
        {
            string message;
            string printFilePath = "";
            UserTestModel utModel = null;
            var result = _userTestService.ChangeStatus(userTestId, out message, out utModel);
            if (result)
            {
                if(utModel != null && utModel.Status && string.IsNullOrEmpty(utModel.Description))
                {
                    var model = _userTestQuestionAnswerService.Degree(userTestId);
                    if (model != null)
                    {
                        var domainUrl = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);//Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port
                        model.CertificateFont = domainUrl + model.CertificateFont;
                        model.CertificateBack = domainUrl + model.CertificateBack;
                        if (!string.IsNullOrEmpty(model.User.Thumbnail))
                        {
                            model.User.Thumbnail = domainUrl + model.User.Thumbnail;
                        }
                        var html = RenderPartialViewToString("~/Views/UserTest/Partial/_degreePartial.cshtml", model);
                        string fileName = model.User != null && !string.IsNullOrEmpty(model.User.EmployerCode) ? "Degree" + model.User.EmployerCode + DateTime.Now.ToString("mmss") : "Degree" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                        string filePath = Path.Combine(Server.MapPath("~/Uploads/DegreeFiles/"), fileName + ".pdf");
                        System.IO.File.WriteAllBytes(filePath, PdfSharpConvert(html));
                        printFilePath = "/Uploads/DegreeFiles/" + fileName + ".pdf";
                        utModel.Description = printFilePath;
                        _userTestService.UpdateUserTest(utModel, out message);
                    }
                    else
                    {
                        message = "Cập nhật thất bại!";
                        return Json(new { IsError = true, PrintFilePath = printFilePath, Message = message });
                    }
                }    
                return Json(new { IsError = false, PrintFilePath = printFilePath, Message = message });
            }
            return Json(new { IsError = true, PrintFilePath = printFilePath, Message = message });
        }

        private Byte[] PdfSharpConvert(string html)
        {
            var pageMargins = new NReco.PdfGenerator.PageMargins { Bottom = 0, Left = 0, Right = 0, Top = 0 };
            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            htmlToPdf.Size = NReco.PdfGenerator.PageSize.A4;
            htmlToPdf.Margins = pageMargins;
            return htmlToPdf.GeneratePdf(html);
        }
    }
}