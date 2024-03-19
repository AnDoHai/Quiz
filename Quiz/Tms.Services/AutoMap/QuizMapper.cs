using Tms.DataAccess;
using Tms.Models.QuizModel;
using System.Collections.Generic;
using System.Linq;

namespace Tms.Services.AutoMap
{
    public static class QuizMapper
    {
        #region Mapping Quiz
        public static QuizModel MapToModel(this Quiz entity)
        {
            return new QuizModel
            {
				QuizID = entity.QuizID,
				QuizName = entity.QuizName,
				Title = entity.Title,
				CreatedDate = entity.CreatedDate,
				CreatedBy = entity.CreatedBy,
				UpdatedDate = entity.UpdatedDate,
				UpdatedBy = entity.UpdatedBy,
				Status = entity.Status,
				Description = entity.Description,
				CategoryId = entity.CategoryId,
				CategoryType = entity.Category!=null?entity.Category.Type:0,
				TimeLimit = entity.Category != null ? entity.Category.TimeLimit : entity.TimeLimit,
				Type = entity.Type,
				HSKName = entity.Category != null ? entity.Category.Title : "",
            };
        }
        public static QuizModel MapToModel(this Quiz entity, QuizModel model)
        {
			model.QuizID = entity.QuizID;
			model.QuizName = entity.QuizName;
			model.Title = entity.Title;
			model.CreatedDate = entity.CreatedDate;
			model.CreatedBy = entity.CreatedBy;
			model.UpdatedDate = entity.UpdatedDate;
			model.UpdatedBy = entity.UpdatedBy;
			model.Status = entity.Status;
			model.Description = entity.Description;
			model.CategoryId = entity.CategoryId;
			model.TimeLimit = entity.TimeLimit;
			model.Type = entity.Type;

            return model;
        }
        public static Quiz MapToEntity(this QuizModel model)
        {
            return new Quiz
            {
				QuizID = model.QuizID,
				QuizName = model.QuizName,
				Title = model.Title,
				CreatedDate = model.CreatedDate,
				CreatedBy = model.CreatedBy,
				UpdatedDate = model.UpdatedDate,
				UpdatedBy = model.UpdatedBy,
				Status = model.Status,
				Description = model.Description,
				CategoryId = model.CategoryId,
				TimeLimit = model.TimeLimit,
				Type = model.Type,

            };
        }
        public static Quiz MapToEntity(this QuizModel model, Quiz entity)
        {
			entity.QuizID = model.QuizID;
			entity.QuizName = model.QuizName;
			entity.Title = model.Title;
			//entity.CreatedDate = model.CreatedDate;
			//entity.CreatedBy = model.CreatedBy;
			entity.UpdatedDate = model.UpdatedDate;
			entity.UpdatedBy = model.UpdatedBy;
			entity.Status = model.Status;
			entity.Description = model.Description;
			entity.CategoryId = model.CategoryId;
			entity.TimeLimit = model.TimeLimit;
			entity.Type = model.Type;

            return entity;
        }
        public static List<Quiz> MapToEntities(this List<QuizModel> models)
        {
            return models.Select(x => x.MapToEntity()).ToList();
        }

        public static List<QuizModel> MapToModels(this List<Quiz> entities)
        {
            return entities.Select(x => x.MapToModel()).ToList();
        }
        #endregion
    }
}
