using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.Repositories;
using Tms.Models.CategoryModel;
using Tms.Services.AutoMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tms.Services
{
    public interface ICategoryService : IEntityService<Category>
    {
        List<CategoryModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);
        bool UpdateCategory(CategoryModel CategoryModel, out string message);
        bool Delete(int categoryId, out string message);
        List<CategoryModel> GetAllCategorys();
        bool CreateCategory(CategoryModel model);
		bool ChangeStatus(int categoryId, out string message);

    }
    public class CategoryService : EntityService<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IQuizRepository _quizRepository;
        public CategoryService(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IQuizRepository quizRepository)
            : base(unitOfWork, categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _quizRepository = quizRepository;
        }

        public List<CategoryModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage)
        {
			try
            {
                var categoryEntities = _categoryRepository.Search(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalPage);
				if (categoryEntities != null)
				{
					return categoryEntities.MapToModels();
				}
            }
            catch (Exception ex)
            {
                Log.Error("Search Category error", ex);
            }
            totalPage = 0;
            return null;
        }

        public bool UpdateCategory(CategoryModel categoryModel, out string message)
        {
            try
            {
                var categoryEntity = _categoryRepository.GetById(categoryModel.CategoryId);
				if (categoryEntity != null)
				{
                    categoryModel.Description = categoryEntity.Description;
                    categoryModel.Type = categoryEntity.Type;
                    categoryEntity = categoryModel.MapToEntity(categoryEntity);
					_categoryRepository.Update(categoryEntity);
					UnitOfWork.SaveChanges();

                    if (categoryModel.TimeLimit != null)
                    {
                        var quizEntities = _quizRepository.GetByCategoryId(categoryModel.CategoryId);
                        quizEntities.Select(c => c.TimeLimit = categoryModel.TimeLimit).ToList();
                        quizEntities.Select(c=> _quizRepository.Update(c));
                        UnitOfWork.SaveChanges();
                    }
					message = "Cập nhật thành công";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Update Category error", ex);
            }
            message = "Cập nhật bản ghi thất bại.";
            return false;
        }

        public bool CreateCategory(CategoryModel model)
        {
            try
            {
                var createdCategory = _categoryRepository.Insert(model.MapToEntity());
                UnitOfWork.SaveChanges();
                var errorMsg = string.Empty;
                if (createdCategory == null)
                {
                    Log.Error("Create category error");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Create Category error", ex);
                return false;
            }

        }

        public bool Delete(int categoryId, out string message)
        {
            

            try
            {
                var entity = _categoryRepository.GetById(categoryId);
				if (entity != null)
				{
					_categoryRepository.Delete(categoryId);
					UnitOfWork.SaveChanges();

					message = "Xóa bản ghi thành công";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Delete Category error", ex);
            }

            message = "Xóa bản ghi thất bại";
            return false;
        }

        public List<CategoryModel> GetAllCategorys()
        {
            //Igrone category system
            return _categoryRepository.GetAll().ToList().MapToModels();
        }

		public bool ChangeStatus(int categoryId, out string message)
        {
            try
            {
                var entity = _categoryRepository.Query(c => c.CategoryId == categoryId).FirstOrDefault();
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

					_categoryRepository.Update(entity);
					UnitOfWork.SaveChanges();

					message = "Cập nhật trạng thái thành công.";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Delete Category error", ex);
            }

            message = "Cập nhật trạng thái thất bại.";
            return false;
        }
    }
}
