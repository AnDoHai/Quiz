using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.Repositories;
using Tms.Models.UserTestModel;
using Tms.Services.AutoMap;
using System;
using System.Collections.Generic;
using System.Linq;
using Tms.Models.Models.QuizModel;
using System.Globalization;

namespace Tms.Services
{
    public interface IUserTestService : IEntityService<UserTest>
    {
        List<UserTestModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection,string dateTime, out int totalPage);
        List<UserTestModel> SearchByUser(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? userId = null);
        List<UserTestModel> SearchAllUsers(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, string stringTime, out int totalPage);
        List<UserTestModel> ExamHistoryCertificate(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? userId = null);
        bool UpdateUserTest(UserTestModel UserTestModel, out string message);
        bool Delete(int userTestId, out string message);
        double UpdatePoint(int userTestId);
        string DeleteFilePDF(int userTestId);
        List<UserTestModel> GetAllUserTests();
        string GetAudioForDelete(int userTestId);
        List<QuestionExamResultHight> GetExamsResultHight(int id);
        bool CreateUserTest(UserTestModel model);
		bool ChangeStatus(int userTestId, out string message, out UserTestModel userTestModel);
        List<UserTestModel> GetAllTestExam(int userId, int quizId);
        UserTest GetQuizById(int id);
        UserTest GetUserTestByQuizId(int quizId);
        StatisticalAllExam StatisticalAllExam(int userTestId);

        bool UpdateUserTestAnswer(int questionId,int quizId);
        bool UpdateExam(int questionId, int quizId);
        bool UpdateAllExam(int quizId);

    }
    public class UserTestService : EntityService<UserTest>, IUserTestService
    {
        private readonly IUserTestRepository _userTestRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IUserTestQuestionRepository _userTestQuestionRepository;
        private readonly IContestRepository _contestRepository;
        private readonly IUserTestQuestionAnswerRepository _userTestQuestionAnswerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IQuestionRepository _questionRepository;
        public UserTestService(IUnitOfWork unitOfWork, IUserTestRepository userTestRepository, IUserRepository userRepository, IUserTestQuestionAnswerRepository userTestQuestionAnswerRepository, IContestRepository contestRepository, IQuestionRepository questionRepository, IUserTestQuestionRepository userTestQuestionRepository, IAnswerRepository answerRepository)
            : base(unitOfWork, userTestRepository)
        {
            _userTestRepository = userTestRepository;
            _userTestQuestionRepository = userTestQuestionRepository;
            _answerRepository = answerRepository;
            _userRepository = userRepository;
            _userTestQuestionAnswerRepository = userTestQuestionAnswerRepository;
            _contestRepository = contestRepository;
            _questionRepository= questionRepository;
        }
        public StatisticalAllExam StatisticalAllExam(int userTestId)
        {
            try
            {
                var entities = _userTestRepository.GetQuizById(userTestId);
                var contestEntities = _contestRepository.GetAllContest((int)entities.QuizID);
                StatisticalAllExam modelEntity = new StatisticalAllExam();
                modelEntity.CraeteDate = entities.CreatedDate;
                modelEntity.totalQuestion = entities.Quiz.Questions.Where(c=>c.Status == true).Count();
                modelEntity.TotalPoint = entities.TotalPoint != null ? entities.TotalPoint.Value : 0;
                if (entities != null)
                {
                    if (entities.UserTestQuestions != null)
                    { 
                        var answerEntities = entities.UserTestQuestions.SelectMany(c => c.UserTestQuestionAnswers);
                        modelEntity.CorrectNumberAll = answerEntities.Where(c=>c.Point.HasValue && c.Point != 0).Count();
                    }
                    else
                    {
                        modelEntity.CorrectNumberAll = 0;
                    }
                    List<StatisticalDetailExam> listStatisticalDetails = new List<StatisticalDetailExam>();
                    foreach (var itemContest in contestEntities)
                    {
                        StatisticalDetailExam itemEntity = new StatisticalDetailExam();
                        itemEntity.NameContest = itemContest.ContestName;
                        itemEntity.TotalNumber = itemContest.Questions.Where(c => c.Status == true).Count();
                        itemEntity.TotalPoint = entities.UserTestQuestions != null && entities.UserTestQuestions.Any() ? Math.Round((double)entities.UserTestQuestions.Where(c => c.ContestID == itemContest.ContestID).SelectMany(c => c.UserTestQuestionAnswers.Where(g => g.Point.HasValue && g.Point > 0)).Sum(c => c.Point)) : 0;
                        itemEntity.CorrectNumber = entities.UserTestQuestions != null ? entities.UserTestQuestions.Where(c => c.ContestID == itemContest.ContestID).SelectMany(c => c.UserTestQuestionAnswers.Where(g => g.Point.HasValue && g.Point > 0)).Count() : 0;
                        listStatisticalDetails.Add(itemEntity);
                    }
                    modelEntity.StatisticalDetails = listStatisticalDetails;
                    return modelEntity;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UserTestQuestion error", ex);
            }
            return null;
        }

        public List<UserTestModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, string dateTime, out int totalPage)
        {
			try
            {
                if (!string.IsNullOrEmpty(dateTime))
                {
                    DateTime dateTimer = DateTime.Parse(dateTime, CultureInfo.GetCultureInfo("vi-VN")).Date.Add(DateTime.Now.TimeOfDay);
                    var userTestEntities = _userTestRepository.Search(currentPage, pageSize, textSearch, sortColumn, sortDirection, dateTimer, out totalPage);
                    if (userTestEntities != null)
                    {
                        var entities = userTestEntities.MapToModels();
                        return entities;
                    }
                }
                else
                {
                    var userTestEntities = _userTestRepository.Search(currentPage, pageSize, textSearch, sortColumn, sortDirection,null, out totalPage);
                    if (userTestEntities != null)
                    {
                        var entities = userTestEntities.MapToModels();
                        return entities;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Search UserTest error", ex);
            }
            totalPage = 0;
            return null;
        }
        public List<UserTestModel> ExamHistoryCertificate(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? userId = null)
        {
            try
            {
                var userTestEntities = _userTestRepository.SearchByUser(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalPage, userId);
                if (userTestEntities != null)
                {
                    var entities = userTestEntities.MapToModels().Where(c => c.TotalPoint > c.PassingScore && c.Status == true).ToList();
                    //var entities = userTestEntities.MapToModels();
                    return entities;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Search UserTest error", ex);
            }
            totalPage = 0;
            return null;
        }

        public List<UserTestModel> SearchByUser(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? userId = null)
        {
            try
            {
                var userTestEntities = _userTestRepository.SearchByUser(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalPage, userId);
                if (userTestEntities != null)
                {
                    return userTestEntities.MapToModels().ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Search UserTest error", ex);
            }
            totalPage = 0;
            return null;
        }

        public List<UserTestModel> SearchAllUsers(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, string stringTime, out int totalPage)
        {
            try
            {
                if (!string.IsNullOrEmpty(stringTime))
                {
                    DateTime dateTime = DateTime.Parse(stringTime, CultureInfo.GetCultureInfo("vi-VN")).Date.Add(DateTime.Now.TimeOfDay);
                    var userTestEntities = _userTestRepository.SearchAllUsers(currentPage, pageSize, textSearch, sortColumn, sortDirection, dateTime, out totalPage);
                    if (userTestEntities != null)
                    {
                        var entities = userTestEntities.MapToModels().ToList();
                        return entities;
                    }
                }
                else
                {
                    var userTestEntities = _userTestRepository.SearchAllUsers(currentPage, pageSize, textSearch, sortColumn, sortDirection, null, out totalPage);
                    if (userTestEntities != null)
                    {
                        var entities = userTestEntities.MapToModels().ToList();
                        return entities;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Search UserTest error", ex);
            }
            totalPage = 0;
            return null;
        }

        public bool UpdateUserTest(UserTestModel userTestModel, out string message)
        {
            try
            {
                var userTestEntity = _userTestRepository.GetById(userTestModel.UserTestId);
				if (userTestEntity != null)
				{
					userTestEntity = userTestModel.MapToEntity(userTestEntity);

					_userTestRepository.Update(userTestEntity);
					UnitOfWork.SaveChanges();

					message = "Cập nhật thành công";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Update UserTest error", ex);
            }
            message = "Cập nhật bản ghi thất bại.";
            return false;
        }

        public bool CreateUserTest(UserTestModel model)
        {
            try
            {
                var createdUserTest = _userTestRepository.Insert(model.MapToEntity());
                UnitOfWork.SaveChanges();
                var errorMsg = string.Empty;
                if (createdUserTest == null)
                {
                    Log.Error("Create userTest error");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Create UserTest error", ex);
                return false;
            }

        }

        public bool Delete(int userTestId, out string message)
        {
            

            try
            {
                var entity = _userTestRepository.GetById(userTestId);
				if (entity != null)
				{
					_userTestRepository.Delete(userTestId);
					UnitOfWork.SaveChanges();

					message = "Xóa bản ghi thành công";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Delete UserTest error", ex);
            }

            message = "Xóa bản ghi thất bại";
            return false;
        }

        public List<UserTestModel> GetAllUserTests()
        {
            //Igrone userTest system
            return _userTestRepository.GetAll().ToList().MapToModels();
        }

		public bool ChangeStatus(int userTestId, out string message, out UserTestModel userTestModel)
        {
            userTestModel = null;
            try
            {
                var entity = _userTestRepository.Query(c => c.UserTestId == userTestId).FirstOrDefault();
				if (entity != null)
				{
					if (entity.Status)
					{
						entity.Status = false;
					}
					else
					{
						entity.Status = true;
                        var entities = _userTestRepository.GetQuizById(userTestId);
                        if (entities != null)
                        {
                            //lấy mã
                            var preFix = entities.Quiz.Category.PreFix;
                            var dateNumber = entities.CreatedDate.ToString("dd/MM");
                            var arrayDateTime = dateNumber.Split('/');
                            var stringDateTime = "";
                            for (int i = 0; i < arrayDateTime.Count(); i++)
                            {
                                stringDateTime += arrayDateTime[i];
                            }
                            var countDegree = _userTestRepository.GetAll().Where(c => c.Status == true).Count();
                            var degreeCode = (countDegree + 1).ToString().PadLeft(6, '0');
                            var codeDegree = preFix + stringDateTime + degreeCode;
                            //end lấy mã
                            entity.Title = codeDegree;
                        }
                    }

                    _userTestRepository.Update(entity);
					UnitOfWork.SaveChanges();
                    userTestModel = entity.MapToModel();

                    message = "Cập nhật trạng thái thành công.";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Delete UserTest error", ex);
            }

            message = "Cập nhật trạng thái thất bại.";
            return false;
        }

        public List<UserTestModel> GetAllTestExam()
        {
            throw new NotImplementedException();
        }

        public List<UserTestModel> GetAllTestExam(int userId,int quizId)
        {
            var entities = _userTestRepository.GetByQuizId(quizId, userId);
            if (entities != null)
            {
                return entities.MapToModels();
            }
            return null;
        }
        public List<QuestionExamResultHight> GetExamsResultHight(int id)
        {
            List<QuestionExamResultHight> listQuiz = new List<QuestionExamResultHight>();
            try
            {
                var entities = _userTestQuestionAnswerRepository.GetByTestHightId(id);
                if (entities != null)
                {
                    foreach (var item in entities)
                    {
                        QuestionExamResultHight itemDetail = new QuestionExamResultHight();
                        itemDetail.QuestionID = (int)item.UserTestQuestion.QuestionID;
                        var questionEntity = _questionRepository.GetAllQuestion();
                        itemDetail.QuestionName = questionEntity.Where(c => c.QuestionID == item.UserTestQuestion.QuestionID).FirstOrDefault().QuestionText;
                        itemDetail.SectionID = (int)questionEntity.Where(c => c.QuestionID == item.UserTestQuestion.QuestionID).FirstOrDefault().SectionID;
                        itemDetail.SectionName = questionEntity.Where(c => c.QuestionID == item.UserTestQuestion.QuestionID).FirstOrDefault().Section.SectionName;
                        itemDetail.ContestID = (int)questionEntity.Where(c => c.QuestionID == item.UserTestQuestion.QuestionID).FirstOrDefault().ContestID;
                        itemDetail.ContestName = questionEntity.Where(c => c.QuestionID == item.UserTestQuestion.QuestionID).FirstOrDefault().Contest.ContestName;
                        if (item.Point == null)
                        {
                            itemDetail.Status = false;
                        }
                        else
                        {
                            itemDetail.Status = true;
                        }
                        itemDetail.Point = (int)item.Point;
                        listQuiz.Add(itemDetail);
                    }
                }
                return listQuiz;
            }
            catch (Exception ex)
            {
                Log.Error("Question error", ex);
            }
            return null;
        }
        public UserTest GetQuizById(int id)
        {
            try
            {
                var entity = _userTestRepository.GetQuizById(id);
                if (entity != null)
                {
                    return entity;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UserTest error", ex);
            }
            return null;
        }

        public UserTest GetUserTestByQuizId(int quizId)
        {
            try
            {
                var entity = _userTestRepository.GetUserTestByQuizId(quizId);
                if (entity != null)
                {
                    return entity;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UserTest error", ex);
            }
            return null;
        }

        public string GetAudioForDelete(int userTestId)
        {
            try
            {
                var entity = _userTestQuestionAnswerRepository.GetAnswerByUserTestId(userTestId);
                if (entity != null)
                {
                    var urlLink = entity.UserTestQuestionAnswerText;
                    entity.Code = "";
                    entity.UserTestQuestionAnswerText = "";
                    _userTestQuestionAnswerRepository.Update(entity);
                    UnitOfWork.SaveChanges();
                    return urlLink;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UserTest error", ex);
            }
            return "";
        }

        public string DeleteFilePDF(int userTestId)
        {
            try
            {
                var entity = _userTestRepository.GetById(userTestId);
                var stringPDF = "";
                if (entity != null)
                {
                    stringPDF = entity.Description;
                    entity.Description = "";
                    _userTestRepository.Update(entity);
                    UnitOfWork.SaveChanges();
                    return stringPDF;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UserTest error", ex);
            }
            return "";
        }

        public double UpdatePoint(int userTestId)
        {
            try
            {
                var entity = _userTestQuestionAnswerRepository.GetAllByUserTestId(userTestId);
                if (entity != null)
                {
                    double point  = entity.GroupBy(c => c.Type).Select(c => c.ToList().Sum(g => g.Point)).Select(i => Math.Round(i.Value)).Sum();
                    var entityUpdate = _userTestRepository.GetById(userTestId);
                    entityUpdate.TotalPoint = point;
                    _userTestRepository.Update(entityUpdate);
                    UnitOfWork.SaveChanges();
                    return point;
                }

            }
            catch (Exception ex)
            {
                Log.Error("UserTest error", ex);
            }
            return 0;
        }

        public bool UpdateUserTestAnswer(int questionId, int quizId)
        {
            try
            {
                var entity = _userTestRepository.GetByUserTestId(quizId, questionId);
                if (entity != null)
                {
                    var listQuestion = new List<UserTestQuestion>();
                    foreach (var item in entity)
                    {
                        var userTestQuestion = new UserTestQuestion
                        {
                            ContestID = /*1059*/2239,
                            SectionID = /*1147*/ 2777,
                            QuestionID = /*1567*/ 5904,
                            UserTestId = item.UserTestId,
                            CreatedDate = DateTime.Now,
                            CreatedBy = "admin"
                        };
                        listQuestion.Add(userTestQuestion);
                    }
                    var userTestQuestionList = _userTestQuestionRepository.InsertMulti(listQuestion);
                    UnitOfWork.SaveChanges();
                    var listAnswer = new List<UserTestQuestionAnswer>();
                    foreach (var itemDetail in userTestQuestionList)
                    {
                        var answer = new UserTestQuestionAnswer
                        {
                            UserTestQuestionID = itemDetail.UserTestQuestionId,
                            CreatedDate = DateTime.Now,
                            CreatedBy = "admin",
                            Point = 5,
                            UserTestQuestionAnswerText = "",
                            Code = "",
                            Type = 3,
                            Status = true
                        };
                        listAnswer.Add(answer);
                    }
                    _userTestQuestionAnswerRepository.InsertMulti(listAnswer);
                    UnitOfWork.SaveChanges();
                    return true;
                }

            }
            catch (Exception ex)
            {
                Log.Error("UserTest error", ex);
            }
            return false;
        }

        public bool UpdateAllExam(int quizId)
        {
            try
            {
                var entity = _userTestRepository.GetByQuizId(quizId).ToList();
                double point = 0;
                if (entity != null)
                {
                    foreach (var itemDetail in entity)
                    {
                        point = 0;
                        var check = itemDetail.UserTestQuestions.GroupBy(c => c.ContestID);
                        foreach (var itemDetailQuestion in check)
                        {
                            double countPoint1 = 0;
                            double pointCheck = 0;
                            foreach (var answer in itemDetailQuestion)
                            {
                                double countPoint = (double)answer.UserTestQuestionAnswers.Sum(c => c.Point);
                                pointCheck += countPoint;
                            }
                            point += Math.Round(pointCheck);

                        }
                        itemDetail.TotalPoint = point;
                        _userTestRepository.Update(itemDetail);
                        UnitOfWork.SaveChanges();
                    }
                    return true;
                }

            }
            catch (Exception ex)
            {
                Log.Error("UserTest error", ex);
            }
            return false;
        }

        public bool UpdateExam(int questionId, int quizId)
        {
            try
            {
                var entities = _userTestRepository.GetByUserTestId(quizId, questionId);
                var entityAnswer = _answerRepository.GetByQuestionId(questionId);
                var entityQuestion = _questionRepository.GetById(questionId);
                if (entities != null && entities.Any())
                {
                    foreach (var item in entities)
                    {
                        var entityQuestions = item.UserTestQuestions.Where(c => c.QuestionID == questionId).FirstOrDefault();
                        if (entityQuestions != null)
                        {
                            foreach (var itemAnswer in entityQuestions.UserTestQuestionAnswers)
                            {
                                if (itemAnswer.Code.Equals(entityAnswer.Code))
                                {
                                    itemAnswer.Point = entityQuestion.Point;
                                    _userTestQuestionAnswerRepository.Update(itemAnswer);
                                    UnitOfWork.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("UserTest error", ex);
            }
            return false;
        }
    }
}
