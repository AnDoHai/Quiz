using System;
using System.Collections.Generic;
using System.Linq;
using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.Repositories;
using Tms.Models.CategoryNewModel;
using Tms.Services.AutoMap;

namespace Tms.Services
{
    public interface ICategoryNewService : IEntityService<CategoryNew>
    {
        List<CategoryNewModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);
        bool UpdateCategoryNew(CategoryNewModel CategoryNewModel, out string message);
        bool Delete(int categoryNewId, out string message);
        List<CategoryNewModel> GetAllCategoryNews();
        bool CreateCategoryNew(CategoryNewModel model);
		bool ChangeStatus(int categoryNewId, out string message);

    }
    public class CategoryNewService : EntityService<CategoryNew>, ICategoryNewService
    {
        private readonly ICategoryNewRepository _categoryNewRepository;
        public CategoryNewService(IUnitOfWork unitOfWork, ICategoryNewRepository categoryNewRepository)
            : base(unitOfWork, categoryNewRepository)
        {
            _categoryNewRepository = categoryNewRepository;
        }

        public List<CategoryNewModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage)
        {
			try
            {
                var categoryNewEntities = _categoryNewRepository.Search(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalPage);
				if (categoryNewEntities != null)
				{
					return categoryNewEntities.MapToModels();
				}
            }
            catch (Exception ex)
            {
                Log.Error("Search CategoryNew error", ex);
            }
            totalPage = 0;
            return null;
        }

        public bool UpdateCategoryNew(CategoryNewModel categoryNewModel, out string message)
        {
            try
            {
                var categoryNewEntity = _categoryNewRepository.GetById(categoryNewModel.CategoryNewsId);
				if (categoryNewEntity != null)
				{
					categoryNewEntity = categoryNewModel.MapToEntity(categoryNewEntity);

					_categoryNewRepository.Update(categoryNewEntity);
					UnitOfWork.SaveChanges();

					message = "Cập nhật thành công";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Update CategoryNew error", ex);
            }
            message = "Cập nhật bản ghi thất bại.";
            return false;
        }

        public bool CreateCategoryNew(CategoryNewModel model)
        {
            try
            {
                var createdCategoryNew = _categoryNewRepository.Insert(model.MapToEntity());
                UnitOfWork.SaveChanges();
                var errorMsg = string.Empty;
                if (createdCategoryNew == null)
                {
                    Log.Error("Create categoryNew error");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Create CategoryNew error", ex);
                return false;
            }

        }

        public bool Delete(int categoryNewId, out string message)
        {
            

            try
            {
                var entity = _categoryNewRepository.GetById(categoryNewId);
				if (entity != null)
				{
					_categoryNewRepository.Delete(categoryNewId);
					UnitOfWork.SaveChanges();

					message = "Xóa bản ghi thành công";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Delete CategoryNew error", ex);
            }

            message = "Xóa bản ghi thất bại";
            return false;
        }

        public List<CategoryNewModel> GetAllCategoryNews()
        {
            //Igrone categoryNew system
            return _categoryNewRepository.GetAll().ToList().MapToModels();
        }

		public bool ChangeStatus(int categoryNewId, out string message)
        {
            try
            {
                var entity = _categoryNewRepository.Query(c => c.CategoryNewsId == categoryNewId).FirstOrDefault();
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

					_categoryNewRepository.Update(entity);
					UnitOfWork.SaveChanges();

					message = "Cập nhật trạng thái thành công.";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Delete CategoryNew error", ex);
            }

            message = "Cập nhật trạng thái thất bại.";
            return false;
        }
    }
}
