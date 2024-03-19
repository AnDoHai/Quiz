using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.Repositories;
using Tms.Models.ChoiceModel;
using Tms.Services.AutoMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tms.Services
{
    public interface IChoiceService : IEntityService<Choice>
    {
        List<ChoiceModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);
        bool UpdateChoice(ChoiceModel ChoiceModel, out string message);
        bool Delete(int choiceId, out string message);
        List<ChoiceModel> GetAllChoices();
        List<ChoiceModel> GetChoiceByQuestionId(int id);
        bool CreateChoice(ChoiceModel model);
		bool ChangeStatus(int choiceId, out string message);

    }
    public class ChoiceService : EntityService<Choice>, IChoiceService
    {
        private readonly IChoiceRepository _choiceRepository;
        public ChoiceService(IUnitOfWork unitOfWork, IChoiceRepository choiceRepository)
            : base(unitOfWork, choiceRepository)
        {
            _choiceRepository = choiceRepository;
        }

        public List<ChoiceModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage)
        {
			try
            {
                var choiceEntities = _choiceRepository.Search(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalPage);
				if (choiceEntities != null)
				{
					return choiceEntities.MapToModels();
				}
            }
            catch (Exception ex)
            {
                Log.Error("Search Choice error", ex);
            }
            totalPage = 0;
            return null;
        }

        public bool UpdateChoice(ChoiceModel choiceModel, out string message)
        {
            try
            {
                var choiceEntity = _choiceRepository.GetById((int)choiceModel.ChoiceID);
                if (choiceModel.Type == null)
                {
                    choiceModel.Type = choiceEntity.Type;
                }
				if (choiceEntity != null)
				{
					choiceEntity = choiceModel.MapToEntity(choiceEntity);

					_choiceRepository.Update(choiceEntity);
					UnitOfWork.SaveChanges();

					message = "Cập nhật thành công";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Update Choice error", ex);
            }
            message = "Cập nhật bản ghi thất bại.";
            return false;
        }

        public bool CreateChoice(ChoiceModel model)
        {
            try
            {
                var createdChoice = _choiceRepository.Insert(model.MapToEntity());
                UnitOfWork.SaveChanges();
                var errorMsg = string.Empty;
                if (createdChoice == null)
                {
                    Log.Error("Create choice error");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Create Choice error", ex);
                return false;
            }

        }

        public bool Delete(int choiceId, out string message)
        {
            

            try
            {
                var entity = _choiceRepository.GetById(choiceId);
				if (entity != null)
				{
					_choiceRepository.Delete(choiceId);
					UnitOfWork.SaveChanges();

					message = "Xóa bản ghi thành công";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Delete Choice error", ex);
            }

            message = "Xóa bản ghi thất bại";
            return false;
        }

        public List<ChoiceModel> GetAllChoices()
        {
            //Igrone choice system
            return _choiceRepository.GetAll().ToList().MapToModels();
        }

		public bool ChangeStatus(int choiceId, out string message)
        {
            try
            {
                var entity = _choiceRepository.Query(c => c.ChoiceID == choiceId).FirstOrDefault();
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

					_choiceRepository.Update(entity);
					UnitOfWork.SaveChanges();

					message = "Cập nhật trạng thái thành công.";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Delete Choice error", ex);
            }

            message = "Cập nhật trạng thái thất bại.";
            return false;
        }

        public List<ChoiceModel> GetChoiceByQuestionId(int id)
        {
            try
            {
                var entities = _choiceRepository.GetAllByQuestionId(id);
                if (entities != null)
                {
                    return entities.MapToModels();
                }
            }catch(Exception ex)
            {
                Log.Error("Choice error", ex);
            }
            return null;
        }
    }
}
