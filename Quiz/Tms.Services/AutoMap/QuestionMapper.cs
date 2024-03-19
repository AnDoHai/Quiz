using Tms.DataAccess;
using Tms.Models.QuestionModel;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Tms.Services.AutoMap
{
    public static class QuestionMapper
    {
        #region Mapping Question
        public static QuestionModel MapToModel(this Question entity)
        {
			int? minuteStart = null;
			int? secoundStart = null;
			int? minuteEnd = null;
			int? secoundEnd = null;
			if (!string.IsNullOrEmpty(entity.Title))
			{
				var stringTime = entity.Title.Split(',');
				var stringStartTime = stringTime[0];
				var stringEndTime = stringTime[1];
				if (!string.IsNullOrEmpty(stringStartTime))
				{
					var timeStartDetail = stringStartTime.Split(':');
					minuteStart = Int32.Parse(timeStartDetail[0]);
					secoundStart = Int32.Parse(timeStartDetail[1]);
				}
				if (!string.IsNullOrEmpty(stringEndTime))
				{
					var timeEndDetail = stringEndTime.Split(':');
					minuteEnd = Int32.Parse(timeEndDetail[0]);
					secoundEnd = Int32.Parse(timeEndDetail[1]);
				}
			}
			return new QuestionModel
			{
				QuestionID = entity.QuestionID,
				QuestionText = entity.QuestionText,
				QuizID = entity.QuizID,
				AudioUrl = entity.AudioUrl,
				Title = entity.Title,
				GroupIndex = entity.GroupIndex.HasValue ? entity.GroupIndex.Value:0,
				NameContest = entity.Contest != null ? entity.Contest.ContestName : "",
				NameSection = entity.Section != null ? entity.Section.SectionName : "",
				NameQuiz = entity.Quiz != null ? entity.Quiz.QuizName : "",
				CreatedDate = entity.CreatedDate,
				CreatedBy = entity.CreatedBy,
				UpdatedDate = entity.UpdatedDate,
				UpdatedBy = entity.UpdatedBy,
				MinuteStart = minuteStart,
				SecondStart = secoundStart,
				MinuteEnd = minuteEnd,
				SecondEnd = secoundEnd,
				Status = entity.Status,
				StatusTextbox = entity.StatusTextbox!=null?entity.StatusTextbox.Value:true,
				Maxlength = entity.MaxLengthText,
				Order = (int)entity.Order,
				TypeChoice = entity.Choices != null && entity.Choices.Any() ? entity.Choices.FirstOrDefault().Type : 0,
				Description = entity.Description,
				Point = entity.Point,
				Layout = entity.Layout,
				Type = entity.Type,
				ContestID = entity.ContestID,
				SectionID = (int)entity.SectionID,

            };
        }
        public static QuestionModel MapToModel(this Question entity, QuestionModel model)
        {
			model.QuestionID = entity.QuestionID;
			model.QuestionText = entity.QuestionText;
			model.QuizID = entity.QuizID;
			model.AudioUrl = entity.AudioUrl;
			model.Title = entity.Title;
			model.CreatedDate = entity.CreatedDate;
			model.CreatedBy = entity.CreatedBy;
			model.UpdatedDate = entity.UpdatedDate;
			model.UpdatedBy = entity.UpdatedBy;
			model.Status = entity.Status;
			model.Description = entity.Description;
			model.Point = entity.Point;
			model.Type = entity.Type;
			model.ContestID = entity.ContestID;
			model.SectionID = (int)entity.SectionID;

            return model;
        }
        public static Question MapToEntity(this QuestionModel model)
        {
			return new Question
			{
				QuestionID = model.QuestionID,
				QuestionText = model.QuestionText,
				QuizID = model.QuizID,
				AudioUrl = model.AudioUrl,
				Title = model.Title,
				GroupIndex = model.GroupIndex,
				CreatedDate = model.CreatedDate,
				CreatedBy = model.CreatedBy,
				UpdatedDate = model.UpdatedDate,
				UpdatedBy = model.UpdatedBy,
				Status = model.Status,
				StatusTextbox = model.StatusTextbox,
				Description = model.Description,
				ImageUrl = model.ImageUrl,
				Point = model.Point,
				Type = model.Type,
				MaxLengthText = model.Maxlength,
				TimeLimit = model.TimeLimit,
				Layout = model.Layout,
				ContestID = model.ContestID,
				SectionID = model.SectionID,
				Order = model.Order

            };
        }
        public static Question MapToEntity(this QuestionModel model, Question entity)
        {
			entity.QuestionID = model.QuestionID;
			entity.QuestionText = model.QuestionText;
			entity.QuizID = model.QuizID;
			entity.AudioUrl = model.AudioUrl;
			entity.Title = model.Title;
			entity.GroupIndex = model.GroupIndex;
			//entity.CreatedDate = model.CreatedDate;
			//entity.CreatedBy = model.CreatedBy;
			entity.UpdatedDate = model.UpdatedDate;
			entity.UpdatedBy = model.UpdatedBy;
			entity.Status = model.Status;
			entity.ImageUrl = model.ImageUrl;
			entity.StatusTextbox = model.StatusTextbox;
			entity.Layout = model.Layout;
			entity.MaxLengthText = model.Maxlength;
			entity.Description = model.Description;
			entity.Point = model.Point;
			entity.Type = model.Type;
			entity.ContestID = model.ContestID != 0 ? model.ContestID : entity.ContestID;
			entity.SectionID = model.SectionID != 0 ? model.SectionID : entity.SectionID;

            return entity;
        }
        public static List<Question> MapToEntities(this List<QuestionModel> models)
        {
            return models.Select(x => x.MapToEntity()).ToList();
        }

        public static List<QuestionModel> MapToModels(this List<Question> entities)
        {
            return entities.Select(x => x.MapToModel()).ToList();
        }
        #endregion
    }
}
