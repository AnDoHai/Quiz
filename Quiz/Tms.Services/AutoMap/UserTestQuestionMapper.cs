using Tms.DataAccess;
using Tms.Models.UserTestQuestionModel;
using System.Collections.Generic;
using System.Linq;

namespace Tms.Services.AutoMap
{
    public static class UserTestQuestionMapper
    {
        #region Mapping UserTestQuestion
        public static UserTestQuestionModel MapToModel(this UserTestQuestion entity)
        {
			return new UserTestQuestionModel
            {
				UserTestQuestionId = entity.UserTestQuestionId,
				UserTestId = entity.UserTestId,
				QuestionID = entity.QuestionID,
				Title = entity.Title,
				CreatedDate = entity.CreatedDate,
				CreatedBy = entity.CreatedBy,
				UpdatedDate = entity.UpdatedDate,
				UpdatedBy = entity.UpdatedBy,
				Status = entity.Status,
				Description = entity.Description,
				ContestID = entity.ContestID,
				SectionID = entity.SectionID,
				UserTestQuestionAnswerModels = entity.UserTestQuestionAnswers != null && entity.UserTestQuestionAnswers.Any() ? entity.UserTestQuestionAnswers.ToList().MapToModels() : null
			};
        }
        public static UserTestQuestionModel MapToModel(this UserTestQuestion entity, UserTestQuestionModel model)
        {
			model.UserTestQuestionId = entity.UserTestQuestionId;
			model.UserTestId = entity.UserTestId;
			model.QuestionID = entity.QuestionID;
			model.Title = entity.Title;
			model.CreatedDate = entity.CreatedDate;
			model.CreatedBy = entity.CreatedBy;
			model.UpdatedDate = entity.UpdatedDate;
			model.UpdatedBy = entity.UpdatedBy;
			model.Status = entity.Status;
			model.Description = entity.Description;
			model.ContestID = entity.ContestID;
			model.SectionID = entity.SectionID;

            return model;
        }
        public static UserTestQuestion MapToEntity(this UserTestQuestionModel model)
        {
            return new UserTestQuestion
            {
				UserTestQuestionId = model.UserTestQuestionId,
				UserTestId = model.UserTestId,
				QuestionID = model.QuestionID,
				Title = model.Title,
				CreatedDate = model.CreatedDate,
				CreatedBy = model.CreatedBy,
				UpdatedDate = model.UpdatedDate,
				UpdatedBy = model.UpdatedBy,
				Status = model.Status,
				Description = model.Description,
				ContestID = model.ContestID,
				SectionID = model.SectionID,

            };
        }
        public static UserTestQuestion MapToEntity(this UserTestQuestionModel model, UserTestQuestion entity)
        {
			entity.UserTestQuestionId = model.UserTestQuestionId;
			entity.UserTestId = model.UserTestId;
			entity.QuestionID = model.QuestionID;
			entity.Title = model.Title;
			//entity.CreatedDate = model.CreatedDate;
			//entity.CreatedBy = model.CreatedBy;
			entity.UpdatedDate = model.UpdatedDate;
			entity.UpdatedBy = model.UpdatedBy;
			entity.Status = model.Status;
			entity.Description = model.Description;
			entity.ContestID = model.ContestID;
			entity.SectionID = model.SectionID;

            return entity;
        }
        public static List<UserTestQuestion> MapToEntities(this List<UserTestQuestionModel> models)
        {
            return models.Select(x => x.MapToEntity()).ToList();
        }

        public static List<UserTestQuestionModel> MapToModels(this List<UserTestQuestion> entities)
        {
            return entities.Select(x => x.MapToModel()).ToList();
        }
        #endregion
    }
}
