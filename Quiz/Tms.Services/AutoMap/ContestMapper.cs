using Tms.DataAccess;
using Tms.Models.ContestModel;
using System.Collections.Generic;
using System.Linq;

namespace Tms.Services.AutoMap
{
    public static class ContestMapper
    {
        #region Mapping Contest
        public static ContestModel MapToModel(this Contest entity)
        {
            return new ContestModel
            {
				ContestID = entity.ContestID,
				ContestName = entity.ContestName,
				Title = entity.Title,
				CreatedDate = entity.CreatedDate,
				CreatedBy = entity.CreatedBy,
				UpdatedDate = entity.UpdatedDate,
				UpdatedBy = entity.UpdatedBy,
				Status = entity.Status,
				Description = entity.Description,
				TimeLimit = entity.TimeLimit,
				Type = entity.Type,
				Order = entity.Order,
				QuizID = entity.QuizID,
				QuizName = entity.Quiz != null ? entity.Quiz.QuizName : "",
            };
        }
        public static ContestModel MapToModel(this Contest entity, ContestModel model)
        {
			model.ContestID = entity.ContestID;
			model.ContestName = entity.ContestName;
			model.Title = entity.Title;
			model.CreatedDate = entity.CreatedDate;
			model.CreatedBy = entity.CreatedBy;
			model.UpdatedDate = entity.UpdatedDate;
			model.UpdatedBy = entity.UpdatedBy;
			model.Status = entity.Status;
			model.Description = entity.Description;
			model.TimeLimit = entity.TimeLimit;
			model.Type = entity.Type;
			model.Order = entity.Order;
			model.QuizID = entity.QuizID;

            return model;
        }
        public static Contest MapToEntity(this ContestModel model)
        {
            return new Contest
            {
				ContestID = model.ContestID,
				ContestName = model.ContestName,
				Title = model.Title,
				CreatedDate = model.CreatedDate,
				CreatedBy = model.CreatedBy,
				UpdatedDate = model.UpdatedDate,
				UpdatedBy = model.UpdatedBy,
				Status = model.Status,
				Description = model.Description,
				TimeLimit = model.TimeLimit,
				Type = model.Type,
				Order = model.Order,
				QuizID = model.QuizID,

            };
        }
        public static Contest MapToEntity(this ContestModel model, Contest entity)
        {
			entity.ContestID = model.ContestID;
			entity.ContestName = model.ContestName;
			entity.Title = model.Title;
			//entity.CreatedDate = model.CreatedDate;
			//entity.CreatedBy = model.CreatedBy;
			entity.UpdatedDate = model.UpdatedDate;
			entity.UpdatedBy = model.UpdatedBy;
			entity.Status = model.Status;
			entity.Description = model.Description;
			entity.TimeLimit = model.TimeLimit;
			entity.Type = model.Type;
			entity.Order = model.Order;
			entity.QuizID = model.QuizID;

            return entity;
        }
        public static List<Contest> MapToEntities(this List<ContestModel> models)
        {
            return models.Select(x => x.MapToEntity()).ToList();
        }

        public static List<ContestModel> MapToModels(this List<Contest> entities)
        {
            return entities.Select(x => x.MapToModel()).ToList();
        }
        #endregion
    }
}
