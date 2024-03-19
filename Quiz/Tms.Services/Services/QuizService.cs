using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.Repositories;
using Tms.Models.QuizModel;
using Tms.Services.AutoMap;
using System;
using System.Collections.Generic;
using System.Linq;
using Tms.Models.Models.QuizModel;
using Tms.Models.ChoiceModel;

namespace Tms.Services
{
    public interface IQuizService : IEntityService<Quiz>
    {
        List<QuizModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? categoryId);
        bool UpdateQuiz(QuizModel QuizModel, out string message);
        int GetRandomByCodeExam(int cateId, List<int> examHistoryIds, out bool resetHistory);
        bool UploadExamFile(StatisticalExamFileUpload exam);
        QuizModel GetCategoryByQuizId(int? id);
        List<QuizModel> GetByCategoryIndexId(int cateId);
        bool Delete(int quizId, out string message);
        List<QuizModel> GetAllQuizs();
        List<QuizModel> GetAllQuizsIndex();
        List<QuizModel> GetByCategoryId(int cateId);
        List<QuizModel> ListFeaturedQuizs(int top);
        bool CreateQuiz(QuizModel model);
        bool ChangeStatus(int quizId, out string message);
        QuizModel GetByQuizId(int? id);
        bool ExamQuiz(List<ExamUpload> exam, int quizId, int userId, string currenUser, out int userTestId);
        bool ExamQuizUpdate(List<ExamUpload> exam, int quizId, int userId, string currenUser, int userTestId);
        bool UploadQuizSorts(ExamModel examSorts);

    }
    public class QuizService : EntityService<Quiz>, IQuizService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserTestRepository _userTestRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IUserTestQuestionRepository _userTestQuestionRepository;
        private readonly IUserTestQuestionAnswerRepository _userTestQuestionAnswerRepository;
        public QuizService(IUnitOfWork unitOfWork, ISectionRepository sectionRepository, IQuizRepository quizRepository, IUserTestRepository userTestRepository, IUserTestQuestionRepository userTestQuestionRepository, IUserTestQuestionAnswerRepository userTestQuestionAnswerRepository, IAnswerRepository answerRepository, IQuestionRepository questionRepository, ICategoryRepository categoryRepository)
            : base(unitOfWork, quizRepository)
        {
            _quizRepository = quizRepository;
            _userTestRepository = userTestRepository;
            _sectionRepository = sectionRepository;
            _userTestQuestionRepository = userTestQuestionRepository;
            _userTestQuestionAnswerRepository = userTestQuestionAnswerRepository;
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _categoryRepository = categoryRepository;
        }

        public List<QuizModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? categoryId)
        {
            try
            {
                var quizEntities = _quizRepository.Search(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalPage, categoryId);
                if (quizEntities != null)
                {
                    return quizEntities.MapToModels();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Search Quiz error", ex);
            }
            totalPage = 0;
            return null;
        }
        public bool UpdateQuiz(QuizModel quizModel, out string message)
        {
            try
            {
                var quizEntity = _quizRepository.GetById(quizModel.QuizID);
                quizModel.TimeLimit = quizEntity.TimeLimit;
                if (quizEntity != null)
                {
                    quizEntity = quizModel.MapToEntity(quizEntity);

                    _quizRepository.Update(quizEntity);
                    UnitOfWork.SaveChanges();

                    message = "Cập nhật thành công";
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update Quiz error", ex);
            }
            message = "Cập nhật bản ghi thất bại.";
            return false;
        }

        public bool CreateQuiz(QuizModel model)
        {
            try
            {
                if (model.CategoryId != null)
                {
                    var cateEntity = _categoryRepository.GetById(model.CategoryId.Value);
                    if (cateEntity != null)
                    {
                        model.TimeLimit = cateEntity.TimeLimit;
                        var createdQuiz = _quizRepository.Insert(model.MapToEntity());
                        UnitOfWork.SaveChanges();
                        var errorMsg = string.Empty;
                        if (createdQuiz == null)
                        {
                            Log.Error("Create quiz error");
                            return false;
                        }
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error("Create Quiz error", ex);
                return false;
            }

        }

        public bool Delete(int quizId, out string message)
        {


            try
            {
                var entity = _quizRepository.GetById(quizId);
                if (entity != null)
                {
                    _quizRepository.Delete(quizId);
                    UnitOfWork.SaveChanges();

                    message = "Xóa bản ghi thành công";
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Delete Quiz error", ex);
            }

            message = "Xóa bản ghi thất bại";
            return false;
        }

        public List<QuizModel> GetAllQuizs()
        {
            //Igrone quiz system
            return _quizRepository.GetAllQuiz().ToList().MapToModels();
        }

        public QuizModel GetCategoryByQuizId(int? id)
        {
            return _quizRepository.GetCategoryByQuizId(id).MapToModel();
        }
        public QuizModel GetByQuizId(int? id)
        {
            return _quizRepository.GetByQuizId(id).MapToModel();
        }

        public List<QuizModel> GetByCategoryIndexId(int cateId)
        {
            return _quizRepository.GetByCategoryIndexId(cateId).ToList().MapToModels();
        }
        public List<QuizModel> GetByCategoryId(int cateId)
        {
            return _quizRepository.GetByCategoryId(cateId).ToList().MapToModels();
        }
        public List<QuizModel> ListFeaturedQuizs(int top)
        {
            return _quizRepository.GetFeaturedQuiz(top).OrderByDescending(x => x.CreatedDate).ToList().MapToModels();
        }


        public bool ChangeStatus(int quizId, out string message)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.Error("Delete Quiz error", ex);
            }

            message = "Cập nhật trạng thái thất bại.";
            return false;
        }
        public bool ExamQuizUpdate(List<ExamUpload> exam, int quizId, int userId, string currenUser, int userTestId)
        {
            var status = false;
            if (exam != null)
            {
                var userTestQuestionEntities = exam.Select(c => new UserTestQuestion()
                {
                    QuestionID = c.QuestionID,
                    UserTestId = userTestId,
                    ContestID = c.ContestID,
                    SectionID = c.SectionID,
                    CreatedDate = DateTime.Now,
                    CreatedBy = currenUser
                }).ToList();
                var userTestQuestions = _userTestQuestionRepository.InsertMulti(userTestQuestionEntities);
                UnitOfWork.SaveChanges();

                var questionIds = exam.Select(c => c.QuestionID).ToList();
                var checkAnswers = _answerRepository.GetListByQuestionIds(questionIds);
                var userTestQuestionAnswerEntities = new List<UserTestQuestionAnswer>();

                foreach (var contestModel in exam)
                {
                    var usTest = userTestQuestions.FirstOrDefault(c => c.QuestionID == contestModel.QuestionID);
                    var userTestQuestionAnswerEntity = new UserTestQuestionAnswer()
                    {
                        CreatedDate = DateTime.Now,
                        CreatedBy = currenUser,
                        UserTestQuestionID = usTest.UserTestQuestionId,
                        UserTestQuestionAnswerText = contestModel.ChoiceText,
                        Code = contestModel.ChoiceText,
                        Type = contestModel.ContestType
                    };
                    var checkAnswer = checkAnswers.Where(c => c.QuestionID == usTest.QuestionID).ToList();
                    if (checkAnswer != null && checkAnswer.Any())
                    {
                        string answerText = contestModel.ChoiceText.ToLower().Replace(" ", "").Trim();
                        if (checkAnswer.Where(c => c.AnswerText != null).Any(c => c.AnswerText.ToLower().Replace(" ", "").Trim().Equals(answerText)))
                        {
                            userTestQuestionAnswerEntity.Point = contestModel.Point;
                        }
                        else
                        {
                            userTestQuestionAnswerEntity.Point = 0;
                        }
                    }
                    else
                    {
                        userTestQuestionAnswerEntity.Point = null;
                    }
                    userTestQuestionAnswerEntities.Add(userTestQuestionAnswerEntity);

                }
                var answerAdd = _userTestQuestionAnswerRepository.InsertMulti(userTestQuestionAnswerEntities);
                UnitOfWork.SaveChanges();
                var listPointContest = answerAdd.GroupBy(c => c.Type);
                var pointTotal = listPointContest.Select(c => c.ToList().Sum(g => g.Point)).Select(i => Math.Round(i.Value)).Sum();
                var entitiyUpdate = _userTestRepository.GetById(userTestId);
                entitiyUpdate.TotalPoint += pointTotal;
                entitiyUpdate.CreatedDate = DateTime.Now;
                _userTestRepository.Update(entitiyUpdate);
                UnitOfWork.SaveChanges();
                status = true;
                return status;

            }
            status = true;
            return status;
        }
        public bool ExamQuiz(List<ExamUpload> exam, int quizId, int userId, string currenUser, out int userTestId)
        {
            var status = false;
            if (exam != null)
            {
                var userTestEntity = new UserTest()
                {
                    UserId = userId,
                    QuizID = quizId,
                    CreatedDate = DateTime.Now,
                    CreatedBy = currenUser
                };
                var userTestAdd = _userTestRepository.Insert(userTestEntity);
                UnitOfWork.SaveChanges();
                if (userTestAdd != null)
                {
                    userTestId = userTestAdd.UserTestId;
                    var userTestQuestionEntities = exam.Select(c => new UserTestQuestion()
                    {
                        QuestionID = c.QuestionID,
                        UserTestId = userTestAdd.UserTestId,
                        ContestID = c.ContestID,
                        SectionID = c.SectionID,
                        CreatedDate = DateTime.Now,
                        CreatedBy = currenUser
                    }).ToList();
                    var userTestQuestions = _userTestQuestionRepository.InsertMulti(userTestQuestionEntities);
                    UnitOfWork.SaveChanges();

                    var questionIds = exam.Select(c => c.QuestionID).ToList();
                    var checkAnswers = _answerRepository.GetListByQuestionIds(questionIds);
                    var userTestQuestionAnswerEntities = new List<UserTestQuestionAnswer>();

                    foreach (var contestModel in exam)
                    {
                        var usTest = userTestQuestions.FirstOrDefault(c => c.QuestionID == contestModel.QuestionID);
                        var userTestQuestionAnswerEntity = new UserTestQuestionAnswer()
                        {
                            CreatedDate = DateTime.Now,
                            CreatedBy = currenUser,
                            UserTestQuestionID = usTest.UserTestQuestionId,
                            UserTestQuestionAnswerText = contestModel.ChoiceText,
                            Code = contestModel.ChoiceText,
                            Type = contestModel.ContestType
                        };
                        var checkAnswer = checkAnswers.Where(c => c.QuestionID == usTest.QuestionID).ToList();
                        if (checkAnswer != null && checkAnswer.Any())
                        {
                            string answerText = contestModel.ChoiceText.ToLower().Replace(" ", "").Trim();
                            if (checkAnswer.Where(c => c.AnswerText != null).Any(c => c.AnswerText.ToLower().Replace(" ", "").Trim().Equals(answerText)))
                            {
                                userTestQuestionAnswerEntity.Point = contestModel.Point;
                            }
                            else
                            {
                                userTestQuestionAnswerEntity.Point = 0;
                            }
                        }
                        else
                        {
                            userTestQuestionAnswerEntity.Point = null;
                        }
                        userTestQuestionAnswerEntities.Add(userTestQuestionAnswerEntity);

                    }
                    var answerAdd = _userTestQuestionAnswerRepository.InsertMulti(userTestQuestionAnswerEntities);
                    var listPointContest = answerAdd.GroupBy(c => c.Type);
                    var pointTotal = listPointContest.Select(c => c.ToList().Sum(g => g.Point)).Select(i => Math.Round(i.Value)).Sum();
                    userTestAdd.TotalPoint = pointTotal;
                    _userTestRepository.Update(userTestAdd);
                    UnitOfWork.SaveChanges();
                    status = true;
                    return status;
                }
            }
            else
            {
                var userTestEntity = new UserTest()
                {
                    UserId = userId,
                    QuizID = quizId,
                    CreatedDate = DateTime.Now,
                    CreatedBy = currenUser
                };
                var userTestAdd = _userTestRepository.Insert(userTestEntity);
                UnitOfWork.SaveChanges();
                userTestId = userTestAdd.UserTestId;
                status = true;
                return status;
            }
            userTestId = 0;
            return status;
        }

        public int GetRandomByCodeExam(int cateId, List<int> examHistoryIds, out bool resetHistory)
        {
            resetHistory = false;
            var entities = _quizRepository.GetAll().Where(c => c.CategoryId == cateId && c.Status != 3);
            if (entities != null)
            {
                var currentQuizs = entities;
                if (examHistoryIds != null && examHistoryIds.Any())
                {
                    currentQuizs = entities.Where(c => !examHistoryIds.Contains(c.QuizID));
                    if (currentQuizs == null || !currentQuizs.Any())
                    {
                        currentQuizs = entities;
                    }
                    else if (currentQuizs.Count() == 1)
                    {
                        resetHistory = true;
                        return currentQuizs.FirstOrDefault().QuizID;
                    }
                }
                List<int> arrCode = new List<int>();
                foreach (var item in currentQuizs)
                {
                    arrCode.Add(item.QuizID);
                }
                var maxCount = arrCode.Count();
                Random ranDomCode = new Random();
                var code = ranDomCode.Next(0, maxCount);
                return arrCode[code];
            }
            return 0;
        }

        public bool UploadExamFile(StatisticalExamFileUpload exam)
        {
            try
            {
                var entityUserTest = new UserTest()
                {
                    UserId = exam.UserId,
                    QuizID = exam.QuizId,
                    CreatedDate = DateTime.Now,
                    CreatedBy = exam.currenUserName,
                    Status = false,
                    TotalPoint = 0
                };
                var userTest = _userTestRepository.Insert(entityUserTest);
                UnitOfWork.SaveChanges();
                var questionEntities = _questionRepository.GetAllExam(exam.QuizId).Where(c => c.Status == true).ToList();
                List<UserTestQuestion> listUserTestQuestionEntities = new List<UserTestQuestion>();
                if (questionEntities != null)
                {
                    foreach (var itemQuestion in questionEntities)
                    {
                        if (userTest != null)
                        {
                            var entityQuestion = new UserTestQuestion()
                            {
                                UserTestId = userTest.UserTestId,
                                QuestionID = itemQuestion.QuestionID,
                                CreatedDate = DateTime.Now,
                                CreatedBy = exam.currenUserName,
                                Status = true,
                                ContestID = itemQuestion.ContestID,
                                SectionID = itemQuestion.SectionID,
                            };
                            listUserTestQuestionEntities.Add(entityQuestion);
                        }
                    }
                    var userTestQuestion = _userTestQuestionRepository.InsertMulti(listUserTestQuestionEntities);
                    UnitOfWork.SaveChanges();
                    if (userTestQuestion != null)
                    {
                        List<UserTestQuestionAnswer> listAnswerQuestionEntities = new List<UserTestQuestionAnswer>();
                        foreach (var itemAnswer in userTestQuestion)
                        {
                            var entityQuestionAnswer = new UserTestQuestionAnswer()
                            {
                                UserTestQuestionAnswerText = exam.textAnswer,
                                UserTestQuestionID = itemAnswer.UserTestQuestionId,
                                CreatedDate = DateTime.Now,
                                CreatedBy = exam.currenUserName,
                                Status = false,
                                Type = 6,
                                Code = exam.textAnswer,
                                Point = null
                            };
                            listAnswerQuestionEntities.Add(entityQuestionAnswer);
                        }

                        _userTestQuestionAnswerRepository.InsertMulti(listAnswerQuestionEntities);
                        UnitOfWork.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Delete Quiz error", ex);
            }
            return false;
        }

        public List<QuizModel> GetAllQuizsIndex()
        {
            return _quizRepository.GetAllQuizIndex().MapToModels();
        }

        public bool UploadQuizSorts(ExamModel examSorts)
        {
            try
            {
                if (examSorts != null)
                {
                    //loop contest
                        var secionCount = 0;
                        if (examSorts.Sections!= null && examSorts.Sections.Any())
                        {
                            //loop section
                            foreach (var itemSection in examSorts.Sections)
                            {
                                var questionCount = 0;
                                //update groupIndex Secctions
                                var entitySection = _sectionRepository.GetById(itemSection.SectionID);
                                if (entitySection != null)
                                {
                                    secionCount++;
                                    entitySection.OrderIndex = secionCount;
                                    UnitOfWork.SaveChanges();
                                }

                                if (itemSection.QuestionModels != null && itemSection.QuestionModels.Any())
                                {
                                    //loop question
                                    foreach (var itemQuestion in itemSection.QuestionModels)
                                    {
                                        var questionEntity = _questionRepository.GetById(itemQuestion.QuestionID);
                                        if (questionEntity != null)
                                        {
                                            questionCount++;
                                            questionEntity.GroupIndex = questionCount;
                                            UnitOfWork.SaveChanges();
                                        }
                                    }
                                }
                            }
                        }
                
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Quiz error", ex);
            }
            return false;
        }
    }
}
