using Tms.DataAccess;
using Tms.Models.ChoiceModel;
using System.Collections.Generic;
using System.Linq;
using Tms.Models;
using EcommerceSystem.Core.Helpers;
using System;

namespace Tms.Services.AutoMap
{
    public static class ChoiceMapper
    {
        #region Mapping Choice
        public static ChoiceModel MapToModel(this Choice entity)
        {
			var nameType = "";
			foreach (var item in Enum.GetValues(typeof(TypeQuestion)))
			{
                if (entity.Type == (int)item)
                {
					nameType = EnumHelper<TypeQuestion>.GetDisplayValue((TypeQuestion)item);
				}
			}
			return new ChoiceModel
            {
				ChoiceID = entity.ChoiceID,
				ChoiceText = entity.ChoiceText,
				QuestionID = entity.QuestionID,
				Title = entity.Title,
				CreatedDate = entity.CreatedDate,
				CreatedBy = entity.CreatedBy,
				NameType = nameType,
				NameQuestion = entity.Question != null? entity.Question.Title:"",
				UpdatedDate = entity.UpdatedDate,
				UpdatedBy = entity.UpdatedBy,
				Status = entity.Status,
				Description = entity.Description,
				Type = entity.Type,

            };
        }
        public static ChoiceModel MapToModel(this Choice entity, ChoiceModel model)
        {
			model.ChoiceID = entity.ChoiceID;
			model.ChoiceText = entity.ChoiceText;
			model.QuestionID = entity.QuestionID;
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
        public static Choice MapToEntity(this ChoiceModel model)
        {
            return new Choice
            {
				ChoiceID = model.ChoiceID!=null?(int)model.ChoiceID:0,
				ChoiceText = model.ChoiceText,
				QuestionID = model.QuestionID!=null? model.QuestionID:null,
				Title = model.Title,
				CreatedDate = model.CreatedDate,
				CreatedBy = model.CreatedBy,
				UpdatedDate = model.UpdatedDate!=null?model.UpdatedDate:null,
				UpdatedBy = model.UpdatedBy,
				Status = model.Status,
				
				Description = model.Description,
				Type = model.Type!=null?model.Type:null,

            };
        }
        public static Choice MapToEntity(this ChoiceModel model, Choice entity)
        {
			entity.ChoiceID = (int)model.ChoiceID;
			entity.ChoiceText = model.ChoiceText;
			entity.QuestionID = model.QuestionID;
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
        public static List<Choice> MapToEntities(this List<ChoiceModel> models)
        {
            return models.Select(x => x.MapToEntity()).ToList();
        }

        public static List<ChoiceModel> MapToModels(this List<Choice> entities)
        {
            return entities.Select(x => x.MapToModel()).ToList();
        }
        #endregion
    }
}
