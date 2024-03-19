using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.Repositories;
using Tms.Models.UserTestQuestionModel;
using Tms.Services.AutoMap;
using System;
using System.Collections.Generic;
using System.Linq;
using Tms.Models.UserTestModel;
using Tms.Models.Models.QuizModel;

namespace Tms.Services
{
    public interface IUserTestQuestionService : IEntityService<UserTestQuestion>
    {
        List<UserTestQuestionModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);
        bool UpdateUserTestQuestion(UserTestQuestionModel UserTestQuestionModel, out string message);
        bool Delete(int userTestQuestionId, out string message);
        List<UserTestQuestionModel> GetAllUserTestQuestions();
        bool CreateUserTestQuestion(UserTestQuestionModel model);
		bool ChangeStatus(int userTestQuestionId, out string message);
        List<UserTestQuestionModel> GetUserTestQuestion(int id);
    }
    public class UserTestQuestionService : EntityService<UserTestQuestion>, IUserTestQuestionService
    {
        private readonly IUserTestQuestionRepository _userTestQuestionRepository;
        private readonly IUserTestQuestionAnswerRepository _userTestQuestionAnswerRepository;
        private readonly IContestRepository _contestRepository;
        public UserTestQuestionService(IUnitOfWork unitOfWork, IUserTestQuestionRepository userTestQuestionRepository, IUserTestQuestionAnswerRepository userTestQuestionAnswerRepository, IContestRepository contestRepository)
            : base(unitOfWork, userTestQuestionRepository)
        {
            _userTestQuestionRepository = userTestQuestionRepository;
            _userTestQuestionAnswerRepository = userTestQuestionAnswerRepository;
            _contestRepository = contestRepository;
        }

        public List<UserTestQuestionModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage)
        {
			try
            {
                var userTestQuestionEntities = _userTestQuestionRepository.Search(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalPage);
				if (userTestQuestionEntities != null)
				{
					return userTestQuestionEntities.MapToModels();
				}
            }
            catch (Exception ex)
            {
                Log.Error("Search UserTestQuestion error", ex);
            }
            totalPage = 0;
            return null;
        }

        public bool UpdateUserTestQuestion(UserTestQuestionModel userTestQuestionModel, out string message)
        {
            try
            {
                var userTestQuestionEntity = _userTestQuestionRepository.GetById(userTestQuestionModel.UserTestQuestionId);
				if (userTestQuestionEntity != null)
				{
					userTestQuestionEntity = userTestQuestionModel.MapToEntity(userTestQuestionEntity);

					_userTestQuestionRepository.Update(userTestQuestionEntity);
					UnitOfWork.SaveChanges();

					message = "Cập nhật thành công";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Update UserTestQuestion error", ex);
            }
            message = "Cập nhật bản ghi thất bại.";
            return false;
        }

        public bool CreateUserTestQuestion(UserTestQuestionModel model)
        {
            try
            {
                var createdUserTestQuestion = _userTestQuestionRepository.Insert(model.MapToEntity());
                UnitOfWork.SaveChanges();
                var errorMsg = string.Empty;
                if (createdUserTestQuestion == null)
                {
                    Log.Error("Create userTestQuestion error");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Create UserTestQuestion error", ex);
                return false;
            }

        }

        public bool Delete(int userTestQuestionId, out string message)
        {
            

            try
            {
                var entity = _userTestQuestionRepository.GetById(userTestQuestionId);
				if (entity != null)
				{
					_userTestQuestionRepository.Delete(userTestQuestionId);
					UnitOfWork.SaveChanges();

					message = "Xóa bản ghi thành công";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Delete UserTestQuestion error", ex);
            }

            message = "Xóa bản ghi thất bại";
            return false;
        }

        public List<UserTestQuestionModel> GetAllUserTestQuestions()
        {
            //Igrone userTestQuestion system
            return _userTestQuestionRepository.GetAll().ToList().MapToModels();
        }

        public List<UserTestQuestionModel> GetUserTestQuestion(int id)
        {
            try
            {
                var entities = _userTestQuestionRepository.GetByUserTestId(id);
                if(entities != null)
                {
                    return entities.MapToModels();
                }
            }
            catch(Exception ex)
            {
                Log.Error("User Test Question error", ex);
            }
            return null;
        }


        public bool ChangeStatus(int userTestQuestionId, out string message)
        {
            try
            {
                var entity = _userTestQuestionRepository.Query(c => c.UserTestQuestionId == userTestQuestionId).FirstOrDefault();
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

					_userTestQuestionRepository.Update(entity);
					UnitOfWork.SaveChanges();

					message = "Cập nhật trạng thái thành công.";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Delete UserTestQuestion error", ex);
            }

            message = "Cập nhật trạng thái thất bại.";
            return false;
        }

        public StatisticalAllExam StatisticalAllExam(int userTestId)
        {
            try
            {
                var entities = _userTestQuestionAnswerRepository.GetByTestId(userTestId).Where(c=>c.Point != null).ToList();
                StatisticalAllExam modelEntity = new StatisticalAllExam();
                if (entities != null)
                {

                }
                modelEntity.ExamName = entities.FirstOrDefault()!=null? entities.FirstOrDefault().UserTestQuestion.UserTest.Quiz.QuizName:"";
                modelEntity.CraeteDate = entities.FirstOrDefault().UserTestQuestion.UserTest.CreatedDate;
                modelEntity.totalQuestion = entities.FirstOrDefault().UserTestQuestion.UserTest.Quiz.Questions.Count();
                modelEntity.CorrectNumberAll = entities.Count();
                modelEntity.TotalPoint = (double)entities.Sum(c => c.Point);
                var contestEntities = _contestRepository.GetAllContest(entities.FirstOrDefault().UserTestQuestion.UserTest.Quiz.QuizID);
                List<StatisticalDetailExam> listStatisticalDetails = new List<StatisticalDetailExam>();
                foreach (var itemContest in contestEntities)
                {
                    StatisticalDetailExam itemEntity = new StatisticalDetailExam();
                    itemEntity.NameContest = itemContest.ContestName;
                    itemEntity.TotalNumber = entities.FirstOrDefault().UserTestQuestion.UserTest.Quiz.Questions.Where(c=>c.ContestID == itemContest.ContestID).Count();
                    itemEntity.CorrectNumber = entities.Where(c=>c.UserTestQuestion.ContestID == itemContest.ContestID).Count();
                    listStatisticalDetails.Add(itemEntity);
                }
                modelEntity.StatisticalDetails = listStatisticalDetails;
                return modelEntity;
            }
            catch(Exception ex)
            {
                Log.Error("UserTestQuestion error", ex);
            }
            return null;
        }
    }
}
