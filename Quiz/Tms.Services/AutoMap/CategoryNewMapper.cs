using System.Collections.Generic;
using System.Linq;
using Tms.DataAccess;
using Tms.Models.CategoryNewModel;

namespace Tms.Services.AutoMap
{
    public static class CategoryNewMapper
    {
        #region Mapping CategoryNew
        public static CategoryNewModel MapToModel(this CategoryNew entity)
        {
            return new CategoryNewModel
            {
				CategoryNewsId = entity.CategoryNewsId,
				Title = entity.Title,
				CreatedDate = entity.CreatedDate,
				CreatedBy = entity.CreatedBy,
				UpdatedDate = entity.UpdatedDate,
				UpdatedBy = entity.UpdatedBy,
				Status = entity.Status,
				Description = entity.Description,
				Type = entity.Type,

            };
        }
        public static CategoryNewModel MapToModel(this CategoryNew entity, CategoryNewModel model)
        {
			model.CategoryNewsId = entity.CategoryNewsId;
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
        public static CategoryNew MapToEntity(this CategoryNewModel model)
        {
            return new CategoryNew
            {
				CategoryNewsId = model.CategoryNewsId,
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
        public static CategoryNew MapToEntity(this CategoryNewModel model, CategoryNew entity)
        {
			entity.CategoryNewsId = model.CategoryNewsId;
			entity.Title = model.Title;
			//entity.CreatedDate = model.CreatedDate;
			//entity.CreatedBy = model.CreatedBy;
			entity.UpdatedDate = model.UpdatedDate;
			entity.UpdatedBy = model.UpdatedBy;
			entity.Status = model.Status;
			entity.Description = model.Description;
			entity.Type = model.Type;

            return entity;
        }
        public static List<CategoryNew> MapToEntities(this List<CategoryNewModel> models)
        {
            return models.Select(x => x.MapToEntity()).ToList();
        }

        public static List<CategoryNewModel> MapToModels(this List<CategoryNew> entities)
        {
            return entities.Select(x => x.MapToModel()).ToList();
        }
        #endregion
    }
}
