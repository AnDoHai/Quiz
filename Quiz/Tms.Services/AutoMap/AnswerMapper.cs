using Tms.DataAccess;
using Tms.Models.AnswerModel;
using System.Collections.Generic;
using System.Linq;
using System;
using Tms.Models;
using EcommerceSystem.Core.Helpers;

namespace Tms.Services.AutoMap
{
    public static class AnswerMapper
    {
        #region Mapping Answer
        public static AnswerModel MapToModel(this Answer entity)
        {
			var nameType = "";
			foreach (var item in Enum.GetValues(typeof(TypeQuestion)))
			{
				if (entity.Type == (int)item)
				{
					nameType = EnumHelper<TypeQuestion>.GetDisplayValue((TypeQuestion)item);
				}
			}
			return new AnswerModel
            {
				AnswerID = entity.AnswerID,
				AnswerText = entity.AnswerText,
				QuestionID = entity.QuestionID,
				Title = entity.Title,
				CreatedDate = entity.CreatedDate,
				CreatedBy = entity.CreatedBy,
				NameQuestion = entity.Question!=null? entity.Question.QuestionText:"",
				NameType = nameType,
				UpdatedDate = entity.UpdatedDate,
				UpdatedBy = entity.UpdatedBy,
				Status = entity.Status,
				Description = entity.Description,
				Type = entity.Type,
				Code = string.IsNullOrEmpty(entity.Code)? entity.AnswerText: entity.Code,

            };
        }
        public static AnswerModel MapToModel(this Answer entity, AnswerModel model)
        {
			model.AnswerID = entity.AnswerID;
			model.AnswerText = entity.AnswerText;
			model.QuestionID = entity.QuestionID;
			model.Title = entity.Title;
			model.CreatedDate = entity.CreatedDate;
			model.CreatedBy = entity.CreatedBy;
			model.UpdatedDate = entity.UpdatedDate;
			model.UpdatedBy = entity.UpdatedBy;
			model.Status = entity.Status;
			model.Description = entity.Description;
			model.Type = entity.Type;
			model.Code = entity.Code;

            return model;
        }
        public static Answer MapToEntity(this AnswerModel model)
        {
            return new Answer
            {
				AnswerID = model.AnswerID,
				AnswerText = model.AnswerText,
				QuestionID = model.QuestionID,
				Title = model.Title,
				CreatedDate = model.CreatedDate,
				CreatedBy = model.CreatedBy,
				UpdatedDate = model.UpdatedDate,
				UpdatedBy = model.UpdatedBy,
				Status = model.Status,
				Description = model.Description,
				Type = model.Type,
				Code = model.Code,

            };
        }
        public static Answer MapToEntity(this AnswerModel model, Answer entity)
        {
			entity.AnswerID = model.AnswerID;
			entity.AnswerText = model.AnswerText;
			entity.QuestionID = model.QuestionID;
			entity.Title = model.Title;
			//entity.CreatedDate = model.CreatedDate;
			//entity.CreatedBy = model.CreatedBy;
			entity.UpdatedDate = model.UpdatedDate;
			entity.UpdatedBy = model.UpdatedBy;
			entity.Status = model.Status;
			entity.Description = model.Description;
			entity.Type = model.Type;
			entity.Code = model.Code;

            return entity;
        }
        public static List<Answer> MapToEntities(this List<AnswerModel> models)
        {
            return models.Select(x => x.MapToEntity()).ToList();
        }

        public static List<AnswerModel> MapToModels(this List<Answer> entities)
        {
            return entities.Select(x => x.MapToModel()).ToList();
        }
        #endregion
    }
}
