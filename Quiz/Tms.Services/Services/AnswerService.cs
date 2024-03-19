using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.Repositories;
using Tms.Models.AnswerModel;
using Tms.Services.AutoMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tms.Services
{
    public interface IAnswerService : IEntityService<Answer>
    {
        List<AnswerModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);
        bool UpdateAnswer(AnswerModel AnswerModel, out string message);
        bool Delete(int answerId, out string message);
        List<AnswerModel> GetAllAnswers();
        List<AnswerModel> GetAllByQuestionId(int questionId);
        bool CreateAnswer(AnswerModel model);
		bool ChangeStatus(int answerId, out string message);
        AnswerModel GetByQuestionId(int id);
    }
    public class AnswerService : EntityService<Answer>, IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        public AnswerService(IUnitOfWork unitOfWork, IAnswerRepository answerRepository)
            : base(unitOfWork, answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public List<AnswerModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage)
        {
			try
            {
                var answerEntities = _answerRepository.Search(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalPage);
				if (answerEntities != null)
				{
					return answerEntities.MapToModels();
				}
            }
            catch (Exception ex)
            {
                Log.Error("Search Answer error", ex);
            }
            totalPage = 0;
            return null;
        }

        public bool UpdateAnswer(AnswerModel answerModel, out string message)
        {
            try
            {
                var answerEntity = _answerRepository.GetById(answerModel.AnswerID);
                if (answerModel.Type == null)
                {
                    answerModel.Type = answerEntity.Type;
                }
				if (answerEntity != null)
				{
					answerEntity = answerModel.MapToEntity(answerEntity);

					_answerRepository.Update(answerEntity);
					UnitOfWork.SaveChanges();

					message = "Cập nhật thành công";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Update Answer error", ex);
            }
            message = "Cập nhật bản ghi thất bại.";
            return false;
        }

        public bool CreateAnswer(AnswerModel model)
        {
            try
            {
                var createdAnswer = _answerRepository.Insert(model.MapToEntity());
                UnitOfWork.SaveChanges();
                var errorMsg = string.Empty;
                if (createdAnswer == null)
                {
                    Log.Error("Create answer error");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Create Answer error", ex);
                return false;
            }

        }

        public bool Delete(int answerId, out string message)
        {
            

            try
            {
                var entity = _answerRepository.GetById(answerId);
				if (entity != null)
				{
					_answerRepository.Delete(answerId);
					UnitOfWork.SaveChanges();

					message = "Xóa bản ghi thành công";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Delete Answer error", ex);
            }

            message = "Xóa bản ghi thất bại";
            return false;
        }

        public List<AnswerModel> GetAllAnswers()
        {
            //Igrone answer system
            return _answerRepository.GetAll().ToList().MapToModels();
        }

		public bool ChangeStatus(int answerId, out string message)
        {
            try
            {
                var entity = _answerRepository.Query(c => c.AnswerID == answerId).FirstOrDefault();
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

					_answerRepository.Update(entity);
					UnitOfWork.SaveChanges();

					message = "Cập nhật trạng thái thành công.";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Delete Answer error", ex);
            }

            message = "Cập nhật trạng thái thất bại.";
            return false;
        }

        public AnswerModel GetByQuestionId(int id)
        {
            try
            {
                var entities = _answerRepository.GetByQuestionId(id);
                if (entities != null)
                {
                    return entities.MapToModel();
                }
            }catch(Exception ex)
            {
                Log.Error("Answer error", ex);
            }
            return null;
        }

        public List<AnswerModel> GetAllByQuestionId(int questionId)
        {
            try
            {
                var entities = _answerRepository.GetListByQuestionId(questionId);
                if (entities != null)
                {
                    return entities.MapToModels();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Answer error", ex);
            }
            return null;
        }
    }
}
