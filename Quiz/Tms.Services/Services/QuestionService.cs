using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.Repositories;
using Tms.Models.QuestionModel;
using Tms.Services.AutoMap;
using System;
using System.Collections.Generic;
using System.Linq;
using Tms.Models.ChoiceModel;

namespace Tms.Services
{
    public interface IQuestionService : IEntityService<Question>
    {
        List<QuestionModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? hskId, int? quizId, int? contestId, int? sectionId);
        bool UpdateQuestion(QuestionAllModel QuestionModel, List<string> Image, string currenUser, out string message);
        bool Delete(int questionId, out string message);
        List<QuestionModel> GetAllQuestions();
        QuestionModel GetQuestionById(int id);
        List<QuestionModel> GetQuestionByQuizId(int id);
        bool CreateQuestion(QuestionModel model);
		bool ChangeStatus(int questionId, out string message);
        bool CreateQuestionExam(QuestionAllModel model, List<string> Image);

    }
    public class QuestionService : EntityService<Question>, IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IChoiceRepository _choiceRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IUserTestQuestionRepository _userTestQuestionRepository;
        private readonly IUserTestQuestionAnswerRepository _userTestQuestionAnswerRepository;
        public QuestionService(IUnitOfWork unitOfWork, IQuestionRepository questionRepository, IChoiceRepository choiceRepository, IAnswerRepository answerRepository, IUserTestQuestionRepository userTestQuestionRepository, IUserTestQuestionAnswerRepository userTestQuestionAnswerRepository)
            : base(unitOfWork, questionRepository)
        {
            _questionRepository = questionRepository;
            _choiceRepository = choiceRepository;
            _answerRepository = answerRepository;
            _userTestQuestionRepository = userTestQuestionRepository;
            _userTestQuestionAnswerRepository= userTestQuestionAnswerRepository;
        }

        public List<QuestionModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage,int? hskId, int? quizId,int? contestId,int? sectionId)
        {
			try
            {
                var questionEntities = _questionRepository.SearchQuestion(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalPage, hskId, quizId, contestId, sectionId);
				if (questionEntities != null)
				{
					return questionEntities.MapToModels();
				}
            }
            catch (Exception ex)
            {
                Log.Error("Search Question error", ex);
            }
            totalPage = 0;
            return null;
        }

        public bool UpdateQuestion(QuestionAllModel questionModel, List<string> Image,string currenUser, out string message)
        {
            try
            {
                var questionEntity = _questionRepository.GetById(questionModel.Question.QuestionID);
                if (questionEntity != null)
                {
                    var entityChoice = _choiceRepository.GetAllByQuestionId(questionModel.Question.QuestionID);
                    if (entityChoice != null)
                    {
                        _choiceRepository.DeleteMulti(entityChoice);
                        UnitOfWork.SaveChanges();
                    }
                    var listChoice = new List<Choice>();
                    switch (questionModel.Question.Type)
                    {
                        case 0:
                            Choice choiceModel = new Choice();
                            choiceModel.ChoiceText = "Đúng";
                            choiceModel.Title = "Đúng";
                            choiceModel.QuestionID = questionModel.Question.QuestionID;
                            choiceModel.CreatedBy = currenUser;
                            choiceModel.CreatedDate = DateTime.Now;
                            choiceModel.Status = true;
                            choiceModel.Type = 1;
                            listChoice.Add(choiceModel);
                            Choice choiceModelFalse = new Choice();
                            choiceModelFalse.ChoiceText = "Sai";
                            choiceModelFalse.Title = "Sai";
                            choiceModelFalse.QuestionID = questionModel.Question.QuestionID;
                            choiceModelFalse.CreatedBy = currenUser;
                            choiceModelFalse.CreatedDate = DateTime.Now;
                            choiceModelFalse.Status = true;
                            choiceModelFalse.Type = 1;
                            listChoice.Add(choiceModelFalse);
                            break;
                        case 1:
                        case 2:
                            if (Image == null && questionModel.Choice != null)
                            {
                                
                                foreach (var itemChoiceNew in questionModel.Choice)
                                {
                                    itemChoiceNew.QuestionID = questionEntity.QuestionID;
                                    itemChoiceNew.CreatedBy = currenUser;
                                    itemChoiceNew.CreatedDate = DateTime.Now;
                                    itemChoiceNew.Type = 1;
                                    listChoice.Add(itemChoiceNew.MapToEntity());
                                }
                                
                            }
                            else
                            {
                                for (int i = 0; i < Image.Count; i++)
                                {
                                    var numBerAlpha = i + 65;
                                    var alphabet = ((char)numBerAlpha).ToString();
                                    var choiceItem = new ChoiceModel();
                                    choiceItem.Title = alphabet;
                                    choiceItem.QuestionID = questionEntity.QuestionID;
                                    choiceItem.ChoiceText = Image[i];
                                    choiceItem.CreatedDate = DateTime.Now;
                                    choiceItem.CreatedBy = currenUser;
                                    choiceItem.Status = true;
                                    choiceItem.Type = 0;
                                    listChoice.Add(choiceItem.MapToEntity());
                                }
                            }
                            break;
                        }
                    _choiceRepository.InsertMulti(listChoice);
                    var answerEntity = _answerRepository.GetListByQuestionId(questionModel.Question.QuestionID);
                    var entityUserTestQuestions = _userTestQuestionRepository.GetAllByQuestionId(questionEntity.QuestionID);
                    _answerRepository.DeleteMulti(answerEntity);
                    UnitOfWork.SaveChanges();
                    if (questionModel.Answer != null && questionModel.Answer.Any())
                    {
                        List<Answer> listAnswer = new List<Answer>();
                        foreach (var item in questionModel.Answer)
                        {
                            if (item.AnswerText != null && item.AnswerText != "")
                            {
                                Answer dataNewAnswer = new Answer
                                {
                                    AnswerText = item.AnswerText,
                                    Code = item.AnswerText,
                                    CreatedDate = DateTime.Now,
                                    CreatedBy = currenUser,
                                    QuestionID = questionModel.Question.QuestionID,
                                };
                                listAnswer.Add(dataNewAnswer);
                            }
                        }
                        _answerRepository.InsertMulti(listAnswer);
                    }
                    // keep status
                    questionModel.Question.Status = questionEntity.Status;
                    questionEntity = questionModel.Question.MapToEntity(questionEntity);
                    _questionRepository.Update(questionEntity);
                    UnitOfWork.SaveChanges();
                    message = "Cập nhật thành công";
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update Question error", ex);
            }
            message = "Cập nhật bản ghi thất bại.";
            return false;
        }

        public bool CreateQuestion(QuestionModel model)
        {
            try
            {
                var createdQuestion = _questionRepository.Insert(model.MapToEntity());
                UnitOfWork.SaveChanges();
                var errorMsg = string.Empty;
                if (createdQuestion == null)
                {
                    Log.Error("Create question error");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Create Question error", ex);
                return false;
            }

        }

        public bool Delete(int questionId, out string message)
        {
            try
            {
                var entity = _questionRepository.GetById(questionId);
                var answerEntity = _answerRepository.GetByQuestionId(questionId);
                var choiceEntity = _choiceRepository.GetAllByQuestionId(questionId);
                var userQuestionEntity = _userTestQuestionRepository.GetByQuestionId(questionId);
                var quizExamQuestion = _userTestQuestionRepository.GetByQuestionId(questionId);

                if (entity != null)
				{
                    if (quizExamQuestion == null)
                    {
                        if (answerEntity != null)
                        {
                            _answerRepository.Delete(answerEntity.AnswerID);
                            UnitOfWork.SaveChanges();
                        }
                        if (choiceEntity != null)
                        {
                            foreach (var itemChoice in choiceEntity)
                            {
                                _choiceRepository.Delete(itemChoice.ChoiceID);
                                UnitOfWork.SaveChanges();
                            }
                        }
                        _questionRepository.Delete(questionId);
                        UnitOfWork.SaveChanges();

                        message = "Xóa bản ghi thành công";
                        return true;
                    }
                    message = "Xóa bản ghi thất bại";
                    return false;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Delete Question error", ex);
            }

            message = "Xóa bản ghi thất bại";
            return false;
        }

        public List<QuestionModel> GetAllQuestions()
        {
            //Igrone question system
            return _questionRepository.GetAll().ToList().MapToModels();
        }

		public bool ChangeStatus(int questionId, out string message)
        {
            try
            {
                var entity = _questionRepository.Query(c => c.QuestionID == questionId).FirstOrDefault();
				if (entity != null)
				{
					if (entity.Status)
					{
						entity.Status = false;
					}
					else
					{
						entity.Status = true;
					}

					_questionRepository.Update(entity);
					UnitOfWork.SaveChanges();

					message = "Cập nhật trạng thái thành công.";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Delete Question error", ex);
            }

            message = "Cập nhật trạng thái thất bại.";
            return false;
        }

        public bool CreateQuestionExam(QuestionAllModel model, List<string> Image)
        {
            try
            {
                var createdQuestion = _questionRepository.Insert(model.Question.MapToEntity());
                UnitOfWork.SaveChanges();
                List<Choice> listChoice = new List<Choice>();
                if (model.Question.Type != 5 && model.Question.Type != 6)
                {
                    if (createdQuestion != null)
                    {
                        var idQuestionChoice = createdQuestion.QuestionID;
                        if (Image != null)
                        {
                            for (int i = 0; i < Image.Count; i++)
                            {
                                var numBerAlpha = i + 65;
                                var alphabet = ((char)numBerAlpha).ToString();
                                var choiceItem = new ChoiceModel();
                                choiceItem.Title = alphabet;
                                choiceItem.QuestionID = createdQuestion.QuestionID;
                                choiceItem.ChoiceText = Image[i];
                                choiceItem.CreatedDate = createdQuestion.CreatedDate;
                                choiceItem.CreatedBy = createdQuestion.CreatedBy;
                                choiceItem.Status = true;
                                choiceItem.Type = 0;
                                listChoice.Add(choiceItem.MapToEntity());
                            }
                        }
                        else
                        {
                            if (model.Question.Type == 0)
                            {
                                Choice choiceModel = new Choice();
                                choiceModel.ChoiceText = "Đúng";
                                choiceModel.Title = "Đúng";
                                choiceModel.QuestionID = createdQuestion.QuestionID;
                                choiceModel.CreatedBy = createdQuestion.CreatedBy;
                                choiceModel.CreatedDate = createdQuestion.CreatedDate;
                                choiceModel.Status = true;
                                choiceModel.Type = 1;
                                listChoice.Add(choiceModel);
                                Choice choiceModelFalse = new Choice();
                                choiceModelFalse.ChoiceText = "Sai";
                                choiceModelFalse.Title = "Sai";
                                choiceModelFalse.QuestionID = createdQuestion.QuestionID;
                                choiceModelFalse.CreatedBy = createdQuestion.CreatedBy;
                                choiceModelFalse.CreatedDate = createdQuestion.CreatedDate;
                                choiceModelFalse.Status = true;
                                choiceModelFalse.Type = 1;
                                listChoice.Add(choiceModelFalse);
                            }
                            else if (model.Question.Type == 1 || model.Question.Type == 2)
                            {
                                if (model.Choice != null && model.Choice.Any())
                                {
                                    foreach (var choiceItem in model.Choice)
                                    {
                                        choiceItem.QuestionID = createdQuestion.QuestionID;
                                        choiceItem.CreatedDate = createdQuestion.CreatedDate;
                                        choiceItem.CreatedBy = createdQuestion.CreatedBy; 
                                        choiceItem.Status = true;
                                        if (model.Question.Type == 2)
                                        {
                                            choiceItem.Type = 1;
                                        }
                                        else
                                        {
                                            choiceItem.Type = model.Question.TypeChoice;
                                        }
                                        listChoice.Add(choiceItem.MapToEntity());
                                    }
                                }
                            }
                        }
                        var choiceAdd = _choiceRepository.InsertMulti(listChoice);
                        var listAnswer = new List<Answer>();
                        if (model.Answer != null && model.Answer.Any())
                        {
                            foreach (var item in model.Answer)
                            {
                                item.CreatedBy = createdQuestion.CreatedBy;
                                item.CreatedDate = createdQuestion.CreatedDate;
                                item.Status = true;
                                item.Code = item.AnswerText;
                                item.QuestionID = createdQuestion.QuestionID;
                                listAnswer.Add(item.MapToEntity());
                            }
                            var answerAdd = _answerRepository.InsertMulti(listAnswer);
                        }
                        UnitOfWork.SaveChanges();
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                Log.Error("Question error", ex);
            }
            return false;
        }

        public QuestionModel GetQuestionById(int id)
        {
            try
            {
                var entities = _questionRepository.GetQuestionById(id);
                if (entities != null)
                {
                    var entitiesModel = entities.MapToModel();
                    entitiesModel.Answers = entities.Answers!=null&&entities.Answers.Any()?entities.Answers.Select(c=>c.MapToModel()).ToList(): null;
                    return entitiesModel;
                }
            }
            catch(Exception ex)
            {
                Log.Error("Question error", ex);
            }
            return null;
        }

        public List<QuestionModel> GetQuestionByQuizId(int id)
        {
            try
            {
                var entities = _questionRepository.GetAllExam(id);
                if (entities != null)
                {
                    return entities.MapToModels();
                }
            }
            catch(Exception ex)
            {
                Log.Error("Question error", ex);
            }
            return null;
        }
    }
}
