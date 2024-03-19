using Tms.DataAccess;
using Tms.Models.CategoryModel;
using System.Collections.Generic;
using System.Linq;

namespace Tms.Services.AutoMap
{
    public static class CategoryMapper
    {
        #region Mapping Category
        public static CategoryModel MapToModel(this Category entity)
        {
            return new CategoryModel
            {
				CategoryId = entity.CategoryId,
				Title = entity.Title,
				CreatedDate = entity.CreatedDate,
				CreatedBy = entity.CreatedBy,
				UpdatedDate = entity.UpdatedDate,
				UpdatedBy = entity.UpdatedBy,
				Status = entity.Status,
				Description = entity.Description,
				Type = entity.Type,
				TimeLimit = entity.TimeLimit,
				PassingScore = entity.PassingScore,
				CertificateImageBack = entity.CertificateImageBack,
				CertificateImageFont = entity.CertificateImageFont
            };
        }
        public static CategoryModel MapToModel(this Category entity, CategoryModel model)
        {
			model.CategoryId = entity.CategoryId;
			model.Title = entity.Title;
			model.CreatedDate = entity.CreatedDate;
			model.CreatedBy = entity.CreatedBy;
			model.UpdatedDate = entity.UpdatedDate;
			model.UpdatedBy = entity.UpdatedBy;
			model.Status = entity.Status;
			model.Description = entity.Description;
			model.Type = entity.Type;

            return model;
        }
        public static Category MapToEntity(this CategoryModel model)
        {
            return new Category
            {
				CategoryId = model.CategoryId,
				Title = model.Title,
				CreatedDate = model.CreatedDate,
				CreatedBy = model.CreatedBy,
				UpdatedDate = model.UpdatedDate,
				UpdatedBy = model.UpdatedBy,
				Status = model.Status,
				Description = model.Description,
				Type = model.Type,

            };
        }
        public static Category MapToEntity(this CategoryModel model, Category entity)
        {
			entity.CategoryId = model.CategoryId;
			entity.Title = model.Title;
			//entity.CreatedDate = model.CreatedDate;
			//entity.CreatedBy = model.CreatedBy;
			entity.TimeLimit = model.TimeLimit;
			entity.UpdatedDate = model.UpdatedDate;
			entity.UpdatedBy = model.UpdatedBy;
			entity.Status = model.Status;
			entity.Description = model.Description;
			entity.Type = model.Type;

            return entity;
        }
        public static List<Category> MapToEntities(this List<CategoryModel> models)
        {
            return models.Select(x => x.MapToEntity()).ToList();
        }

        public static List<CategoryModel> MapToModels(this List<Category> entities)
        {
            return entities.Select(x => x.MapToModel()).ToList();
        }
        #endregion
    }
}
