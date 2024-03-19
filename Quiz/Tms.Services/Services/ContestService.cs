using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.Repositories;
using Tms.Models.ContestModel;
using Tms.Services.AutoMap;
using System;
using System.Collections.Generic;
using System.Linq;
using Tms.Models.Models.QuizModel;
using Tms.Models.UserTestModel;
using System.Data.Entity;
using Tms.Models.ChoiceModel;

namespace Tms.Services
{
    public interface IContestService : IEntityService<Contest>
    {
        List<ContestModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? quizId);
        bool UpdateContest(ContestModel ContestModel, out string message);
        bool Delete(int contestId, out string message);
        List<ContestModel> GetAllContests();
        List<ContestModel> GeyByQUizId(int id);
        List<Contest> GetContestForEdit(int id);
        List<ContestModel> GetAllContestsIndex();
        bool CreateContest(ContestModel model);
        bool ChangeStatus(int contestId, out string message);
        List<ExamModel> GetExams(int id);

        List<ContestExamResult> GetExamsResult(int id, int userTestId);
    }
    public class ContestService : EntityService<Contest>, IContestService
    {
        private readonly IContestRepository _contestRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserTestRepository _userTestRepository;
        public ContestService(IUnitOfWork unitOfWork, IContestRepository contestRepository, IQuestionRepository questionRepository, IUserTestRepository userTestRepository)
            : base(unitOfWork, contestRepository)
        {
            _contestRepository = contestRepository;
            _questionRepository = questionRepository;
            _userTestRepository = userTestRepository;
        }

        public List<ContestModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? quizId)
        {
            try
            {
                var contestEntities = _contestRepository.Search(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalPage, quizId);
                if (contestEntities != null)
                {
                    return contestEntities.MapToModels();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Search Contest error", ex);
            }
            totalPage = 0;
            return null;
        }

        public bool UpdateContest(ContestModel contestModel, out string message)
        {
            try
            {
                var contestEntity = _contestRepository.GetById(contestModel.ContestID);
                if (contestEntity != null)
                {
                    contestEntity = contestModel.MapToEntity(contestEntity);

                    _contestRepository.Update(contestEntity);
                    UnitOfWork.SaveChanges();

                    message = "Cập nhật thành công";
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update Contest error", ex);
            }
            message = "Cập nhật bản ghi thất bại.";
            return false;
        }
        public List<ExamModel> GetExams(int id)
        {
            List<ExamModel> listQuiz = new List<ExamModel>();
            try
            {
                var entities = _contestRepository.GetAllContest(id);
                if (entities != null)
                {
                    var secsionIds = entities.SelectMany(c => c.Sections.Select(s => s.SectionID)).ToList();
                    var questions = _questionRepository.GetAllQuestionExam(secsionIds);
                    foreach (var item in entities)
                    {
                        ExamModel model = new ExamModel();

                        SectionExamModel sectionModel = new SectionExamModel();
                        model.ContestID = (int)item.ContestID;
                        model.ContestName = item.ContestName;
                        model.ContestType = item.Type.HasValue && item.Type != null? item.Type.Value:0;
                        model.LimitTime = (int)item.TimeLimit;
                        model.Sections = item.Sections.Where(c => c.ContestID == model.ContestID && c.Type != null && c.Status == 0).OrderBy(c=>c.OrderIndex).Select(c => new SectionExamModel()
                        {
                            SectionID = c.SectionID,
                            SectionName = c.SectionName != null ? c.SectionName : "",
                            Description = c.Description != null ? c.Description : "",
                            SectionType = (int)c.Type,
                            TimeLimit = c.TimeLimit,
                            Tittle = c.Title,
                        }).ToList();
                        foreach (var modelSecsion in model.Sections)
                        {
                            var questionSelections = questions.Where(c => c.SectionID == modelSecsion.SectionID && c.Section.Status == 0 && c.Status == true);
                            var questionModels = questionSelections.OrderBy(c=>c.GroupIndex).Select(c => new QuestionExamModel()
                            {
                                QuestionID = c.QuestionID,
                                QuestionName = c.QuestionText,
                                Point = c.Point.HasValue ? (double)c.Point : 0,
                                Type = c.Type.HasValue ? c.Type.Value : 0,
                                QuestionOrder = c.Order.HasValue ? c.Order.Value : 0,
                                Image = c.ImageUrl,
                                Maxlength = c.MaxLengthText != null?(int)c.MaxLengthText:0,
                                Tittle = c.Title != null? c.Title:"0",
                                Layout = c.Layout.HasValue ? c.Layout.Value : 2,
                                TimeLimit = c.TimeLimit,
                                StatusTextbox = c.StatusTextbox != null?c.StatusTextbox.Value:true,
                                AnswerType = c.Choices != null && c.Choices.Any() && c.Choices.FirstOrDefault().Type.HasValue ? c.Choices.FirstOrDefault().Type.Value : 3,
                                AudioUrl = c.AudioUrl,
                                Choices = c.Choices != null && c.Choices.Any() ? c.Choices.ToList().MapToModels() : null,
                            }).ToList();
                            modelSecsion.QuestionModels = questionModels;
                        }
                        listQuiz.Add(model);
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

        public int CheckStatusPoint(UserTest userTests,int questionId)
        {
            if (userTests != null && userTests.UserTestQuestions != null && userTests.UserTestQuestions.Select(c=>c.UserTestQuestionAnswers).Count() > 0)
            {
                var poinQuestion = userTests.UserTestQuestions.Where(c=>c.QuestionID == questionId).SelectMany(c=>c.UserTestQuestionAnswers).Select(g=>g.Point).ToList();
                if (poinQuestion != null && poinQuestion.Count() > 0)
                {
                    if (poinQuestion.FirstOrDefault() == null)
                    {
                        return 3;
                    }else if (poinQuestion.FirstOrDefault() > 0)
                    {
                        return 1;
                    }else if (poinQuestion.FirstOrDefault() == 0)
                    {
                        return 2;
                    }
                }
            }
            return 2;
        }
        public List<ContestExamResult> GetExamsResult(int id,int userTestId)
        {
            List<ContestExamResult> listQuiz = new List<ContestExamResult>();
            try
            {
                var entities = _contestRepository.GetAllContest(id);
                var userTestEntity = _userTestRepository.GetUserTestById(userTestId);
                if (entities != null)
                {
                    var secsionIds = entities.SelectMany(c => c.Sections.Select(s => s.SectionID)).ToList();
                    var questions = _questionRepository.GetAllQuestion(secsionIds);
                    // Load phần thi chung
                    foreach (var item in entities)
                    {
                        ContestExamResult model = new ContestExamResult();

                        SectionExamResult sectionModel = new SectionExamResult();
                        model.ContestID = (int)item.ContestID;
                        model.ContestName = item.ContestName;
                        model.ContestType = (int)item.Type;
                        model.SectionExams = item.Sections.Where(c => c.ContestID == c.ContestID && c.Type != null && c.Status == 0).OrderBy(c => c.OrderIndex).Select(c => new SectionExamResult()
                        {
                            SectionID = c.SectionID,
                            SectionName = c.SectionName != null ? c.SectionName : "",
                            Description = c.Description != null ? c.Description : "",
                            SectionType = (int)c.Type,
                            Tittle = c.Title,
                        }).ToList();
                        // Load phần thi nhỏ
                        foreach (var modelSecsion in model.SectionExams)
                        {
                            var questionSelections = questions.Where(c => c.SectionID == modelSecsion.SectionID && c.Section.Status == 0 && c.Status == true);
                            var questionModels = questionSelections.OrderBy(c=>c.GroupIndex).Where(c=>c.Status != false).Select(c => new QuestionExamResult()
                            {
                                QuestionID = c.QuestionID,
                                QuestionName = c.QuestionText,
                                Point = userTestEntity != null && userTestEntity.UserTestQuestions != null && userTestEntity.UserTestQuestions.Select(i=>i.UserTestQuestionAnswers) != null ? userTestEntity.UserTestQuestions.Where(g=>g.QuestionID == c.QuestionID).SelectMany(o=>o.UserTestQuestionAnswers).Select(h=>h.Point).Sum().Value:0,
                                StatusPoint = CheckStatusPoint(userTestEntity,c.QuestionID),
                                Type = c.Type.HasValue ? c.Type.Value : 0,
                                QuestionOrder = c.Order.HasValue ? c.Order.Value : 0,
                                Image = c.ImageUrl,
                                Layout = c.Layout.HasValue ? c.Layout.Value : 2,
                                TimeLimit = c.TimeLimit,
                                Desscription = c.Description,
                                AnswerType = c.Choices != null && c.Choices.Any() && c.Choices.FirstOrDefault().Type.HasValue ? c.Choices.FirstOrDefault().Type.Value : 3,
                                AudioUrl = c.AudioUrl,
                                Choices = c.Choices != null && c.Choices.Any() ? c.Choices.ToList().MapToModels() : null,
                                Answers = c.Answers != null && c.Answers.Any() ? c.Answers.ToList().MapToModels() : null,
                            }).ToList();
                            modelSecsion.QuestionExams = questionModels;
                        }
                        listQuiz.Add(model);
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
        public bool CreateContest(ContestModel model)
        {
            try
            {
                var createdContest = _contestRepository.Insert(model.MapToEntity());
                UnitOfWork.SaveChanges();
                var errorMsg = string.Empty;
                if (createdContest == null)
                {
                    Log.Error("Create contest error");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Create Contest error", ex);
                return false;
            }

        }

        public bool Delete(int contestId, out string message)
        {


            try
            {
                var entity = _contestRepository.GetById(contestId);
                if (entity != null)
                {
                    _contestRepository.Delete(contestId);
                    UnitOfWork.SaveChanges();

                    message = "Xóa bản ghi thành công";
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Delete Contest error", ex);
            }

            message = "Xóa bản ghi thất bại";
            return false;
        }

        public List<ContestModel> GetAllContests()
        {
            //Igrone contest system
            return _contestRepository.GetAllContests().ToList().MapToModels();
        }

        public List<ContestModel> GetAllContestsIndex()
        {
            //Igrone contest system
            return _contestRepository.GetAllContestsIndex().ToList().MapToModels();
        }

        public bool ChangeStatus(int contestId, out string message)
        {
            try
            {
                var entity = _contestRepository.Query(c => c.ContestID == contestId).FirstOrDefault();
                if (entity != null)
                {
                    if (entity.Status == 0)
                    {
                        entity.Status = 1;
                    }
                    else
                    {
                        entity.Status = 0;
                    }

                    _contestRepository.Update(entity);
                    UnitOfWork.SaveChanges();

                    message = "Cập nhật trạng thái thành công.";
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Delete Contest error", ex);
            }

            message = "Cập nhật trạng thái thất bại.";
            return false;
        }

        public List<ContestModel> GeyByQUizId(int id)
        {
            try
            {
                var entities = _contestRepository.GetAllContest(id);
                if (entities != null)
                {
                    return entities.MapToModels();
                }

            }
            catch (Exception ex)
            {
                Log.Error("Contest error", ex);
            }
            return null;
        }

        public List<Contest> GetContestForEdit(int id)
        {
            try
            {
                var entities = _contestRepository.GetAll().Where(c => c.Type != null && c.QuizID != null && c.QuizID == id);
                if (entities != null)
                {
                    return entities.ToList();
                }
            }catch(Exception ex)
            {
                Log.Error("Contest error", ex);
            }
            return null;
        }
    }
}
