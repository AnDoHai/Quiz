using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.Repositories;
using Tms.Models.SectionModel;
using Tms.Services.AutoMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tms.Services
{
    public interface ISectionService : IEntityService<Section>
    {
        List<SectionModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? contestId);
        List<SectionModel> SearchDetail(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? hskId, int? quizId, int? contestId);
        bool UpdateSection(SectionModel SectionModel, out string message);
        bool Delete(int sectionId, out string message);
        List<SectionModel> GetAllSections();
        List<Section> GetAllSectionsForEdit(int id);
        List<SectionModel> GetByContestId(int id);
        bool CreateSection(SectionModel model);
		bool ChangeStatus(int sectionId, out string message);

    }
    public class SectionService : EntityService<Section>, ISectionService
    {
        private readonly ISectionRepository _sectionRepository;
        public SectionService(IUnitOfWork unitOfWork, ISectionRepository sectionRepository)
            : base(unitOfWork, sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }
        public List<SectionModel> SearchDetail(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? hskId, int? quizId, int? contestId)
        {
            try
            {
                var sectionEntities = _sectionRepository.SearchDetail(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalPage, hskId,quizId, contestId);
                if (sectionEntities != null)
                {
                    return sectionEntities.MapToModels();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Search Section error", ex);
            }
            totalPage = 0;
            return null;
        }
        public List<SectionModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? contestId)
        {
			try
            {
                var sectionEntities = _sectionRepository.Search(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalPage, contestId);
				if (sectionEntities != null)
				{
					return sectionEntities.MapToModels();
				}
            }
            catch (Exception ex)
            {
                Log.Error("Search Section error", ex);
            }
            totalPage = 0;
            return null;
        }

        public bool UpdateSection(SectionModel sectionModel, out string message)
        {
            try
            {
                sectionModel.TimeLimit = "";
                if (sectionModel.SecoundStart != null && sectionModel.MinuteStart != null && sectionModel.MinuteEnd != null && sectionModel.SecoundEnd != null)
                {
                    var timeString = sectionModel.MinuteStart + ":" + sectionModel.SecoundStart + "," + sectionModel.MinuteEnd + ":" + sectionModel.SecoundEnd;
                    sectionModel.TimeLimit = timeString;
                }
                var sectionEntity = _sectionRepository.GetById(sectionModel.SectionID);
				if (sectionEntity != null)
				{
                    if (sectionModel.Title == null || sectionModel.Title == "")
                    {
                        sectionModel.Title = sectionEntity.Title;
                    }
					sectionEntity = sectionModel.MapToEntity(sectionEntity);

					_sectionRepository.Update(sectionEntity);
					UnitOfWork.SaveChanges();

					message = "Cập nhật thành công";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Update Section error", ex);
            }
            message = "Cập nhật bản ghi thất bại.";
            return false;
        }

        public bool CreateSection(SectionModel model)
        {
            try
            {
                model.TimeLimit = "";
                if (model.SecoundStart != null && model.MinuteStart != null && model.MinuteEnd != null && model.SecoundEnd != null)
                {
                    var timeString = model.MinuteStart + ":" + model.SecoundStart + "," + model.MinuteEnd + ":" + model.SecoundEnd;
                    model.TimeLimit = timeString;
                }
                var createdSection = _sectionRepository.Insert(model.MapToEntity());
                UnitOfWork.SaveChanges();
                var errorMsg = string.Empty;
                if (createdSection == null)
                {
                    Log.Error("Create section error");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Create Section error", ex);
                return false;
            }

        }

        public bool Delete(int sectionId, out string message)
        {
            

            try
            {
                var entity = _sectionRepository.GetById(sectionId);
				if (entity != null)
				{
					_sectionRepository.Delete(sectionId);
					UnitOfWork.SaveChanges();

					message = "Xóa bản ghi thành công";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Delete Section error", ex);
            }

            message = "Xóa bản ghi thất bại";
            return false;
        }

        public List<SectionModel> GetAllSections()
        {
            //Igrone section system
            return _sectionRepository.GetAll().ToList().MapToModels();
        }

		public bool ChangeStatus(int sectionId, out string message)
        {
            try
            {
                var entity = _sectionRepository.Query(c => c.SectionID == sectionId).FirstOrDefault();
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

                    _sectionRepository.Update(entity);
                    UnitOfWork.SaveChanges();

                    message = "Cập nhật trạng thái thành công.";
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Delete Section error", ex);
            }

            message = "Cập nhật trạng thái thất bại.";
            return false;
        }

        public List<SectionModel> GetByContestId(int id)
        {
            try
            {
                var entities = _sectionRepository.GetByContestId(id);
                if (entities != null)
                {
                    return entities.MapToModels();
                }
            }
            catch(Exception ex)
            {
                Log.Error("Section error", ex);
            }
            return null;
        }

        public List<Section> GetAllSectionsForEdit(int id)
        {
            try
            {
                var entities = _sectionRepository.GetAll().Where(c=> c.ContestID == id);
                if (entities != null)
                {
                    return entities.ToList();
                }
            }catch (Exception ex)
            {
                Log.Error("Contest error", ex);
            }
            return null;
        }
    }
}
