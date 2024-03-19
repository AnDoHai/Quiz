using Tms.DataAccess;
using Tms.Models.UserTestQuestionAnswerModel;
using System.Collections.Generic;
using System.Linq;

namespace Tms.Services.AutoMap
{
    public static class UserTestQuestionAnswerMapper
    {
        #region Mapping UserTestQuestionAnswer
        public static UserTestQuestionAnswerModel MapToModel(this UserTestQuestionAnswer entity)
        {
            return new UserTestQuestionAnswerModel
            {
				UserTestQuestionAnswerID = entity.UserTestQuestionAnswerID,
				UserTestQuestionAnswerText = entity.UserTestQuestionAnswerText,
				UserTestQuestionID = entity.UserTestQuestionID,
				Title = entity.Title,
				QuestionName = entity.UserTestQuestion!= null && entity.UserTestQuestion.Question!=null? entity.UserTestQuestion.Question.QuestionText : "",
				CreatedDate = entity.CreatedDate,
				CreatedBy = entity.CreatedBy,
				QuestionId = entity.UserTestQuestion!=null?entity.UserTestQuestion.QuestionID:null,
				UpdatedDate = entity.UpdatedDate,
				UpdatedBy = entity.UpdatedBy,
				Status = entity.Status,
				Description = entity.Description,
				Type = entity.UserTestQuestion != null && entity.UserTestQuestion.Question != null ? entity.UserTestQuestion.Question.Type : 0,
				Code = entity.Code,
				Point = entity.Point,

            };
        }
        public static UserTestQuestionAnswerModel MapToModel(this UserTestQuestionAnswer entity, UserTestQuestionAnswerModel model)
        {
			model.UserTestQuestionAnswerID = entity.UserTestQuestionAnswerID;
			model.UserTestQuestionAnswerText = entity.UserTestQuestionAnswerText;
			model.UserTestQuestionID = entity.UserTestQuestionID;
			model.Title = entity.Title;
			model.CreatedDate = entity.CreatedDate;
			model.CreatedBy = entity.CreatedBy;
			model.UpdatedDate = entity.UpdatedDate;
			model.UpdatedBy = entity.UpdatedBy;
			model.Status = entity.Status;
			model.Description = entity.Description;
			model.Type = entity.Type;
			model.Code = entity.Code;
			model.Point = entity.Point;

            return model;
        }
        public static UserTestQuestionAnswer MapToEntity(this UserTestQuestionAnswerModel model)
        {
            return new UserTestQuestionAnswer
            {
				UserTestQuestionAnswerID = model.UserTestQuestionAnswerID,
				UserTestQuestionAnswerText = model.UserTestQuestionAnswerText,
				UserTestQuestionID = model.UserTestQuestionID,
				Title = model.Title,
				CreatedDate = model.CreatedDate,
				CreatedBy = model.CreatedBy,
				UpdatedDate = model.UpdatedDate,
				UpdatedBy = model.UpdatedBy,
				Status = model.Status,
				Description = model.Description,
				Type = model.Type,
				Code = model.Code,
				Point = model.Point,

            };
        }
        public static UserTestQuestionAnswer MapToEntity(this UserTestQuestionAnswerModel model, UserTestQuestionAnswer entity)
        {
			entity.UserTestQuestionAnswerID = model.UserTestQuestionAnswerID;
			entity.UserTestQuestionAnswerText = model.UserTestQuestionAnswerText;
			entity.UserTestQuestionID = model.UserTestQuestionID;
			entity.Title = model.Title;
			//entity.CreatedDate = model.CreatedDate;
			//entity.CreatedBy = model.CreatedBy;
			entity.UpdatedDate = model.UpdatedDate;
			entity.UpdatedBy = model.UpdatedBy;
			entity.Status = model.Status;
			entity.Description = model.Description;
			entity.Type = model.Type;
			entity.Code = model.Code;
			entity.Point = model.Point;

            return entity;
        }
        public static List<UserTestQuestionAnswer> MapToEntities(this List<UserTestQuestionAnswerModel> models)
        {
            return models.Select(x => x.MapToEntity()).ToList();
        }

        public static List<UserTestQuestionAnswerModel> MapToModels(this List<UserTestQuestionAnswer> entities)
        {
            return entities.Select(x => x.MapToModel()).ToList();
        }
        #endregion
    }
}
