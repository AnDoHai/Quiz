using Tms.DataAccess;
using Tms.Models.UserTestModel;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Tms.Services.AutoMap
{
    public static class UserTestMapper
    {
        #region Mapping UserTest
        public static UserTestModel MapToModel(this UserTest entity)
        {
            return new UserTestModel
            {
                UserTestId = entity.UserTestId,
                UserId = entity.UserId,
                QuizID = entity.QuizID,
                Email = entity.User != null? entity.User.Email:"",
                Title = entity.Title != null ? entity.Title : "",
                PassingScore = entity.Quiz != null && entity.Quiz.Category != null && entity.Quiz.Category.PassingScore.HasValue ? entity.Quiz.Category.PassingScore.Value : 0,
                QuizName = entity.Quiz != null ? entity.Quiz.QuizName : "",
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedBy,
                UpdatedDate = entity.UpdatedDate,
                UrlAudio = entity.UserTestQuestions != null && entity.UserTestQuestions.Where(c=>c.UserTestQuestionAnswers != null && c.UserTestQuestionAnswers.Count() > 0).Any() ? entity.UserTestQuestions.SelectMany(c => c.UserTestQuestionAnswers.Select(g => g.UserTestQuestionAnswerText)).FirstOrDefault().ToString() : "",
                HskType = entity.Quiz != null && entity.Quiz.Category != null ? entity.Quiz.Category.Type:null,
                UserName = entity.User != null? entity.User.UserName : "",
                UpdatedBy = entity.UpdatedBy,
                Status = entity.Status,
                HSKId = entity.Quiz != null && entity.Quiz.Category != null ? entity.Quiz.Category.CategoryId : 0,
                Description = entity.Description,
                HSKName = (entity.Quiz != null && entity.Quiz.Category != null) ? entity.Quiz.Category.Title : "",
                TotalPoint = entity.TotalPoint != null? Math.Round(entity.TotalPoint.Value) :0,
            };
        }
        public static UserTestModel MapToModel(this UserTest entity, UserTestModel model)
        {
            model.UserTestId = entity.UserTestId;
            model.UserId = entity.UserId;
            model.QuizID = entity.QuizID;
            model.Title = entity.Title;
            model.CreatedDate = entity.CreatedDate;
            model.CreatedBy = entity.CreatedBy;
            model.UpdatedDate = entity.UpdatedDate;
            model.UpdatedBy = entity.UpdatedBy;
            model.Status = entity.Status;
            model.Description = entity.Description;

            return model;
        }
        public static UserTest MapToEntity(this UserTestModel model)
        {
            return new UserTest
            {
                UserTestId = model.UserTestId,
                UserId = model.UserId,
                QuizID = model.QuizID,
                Title = model.Title,
                CreatedDate = model.CreatedDate,
                CreatedBy = model.CreatedBy,
                UpdatedDate = model.UpdatedDate,
                UpdatedBy = model.UpdatedBy,
                Status = model.Status,
                Description = model.Description,

            };
        }
        public static UserTest MapToEntity(this UserTestModel model, UserTest entity)
        {
            entity.UserTestId = model.UserTestId;
            entity.UserId = model.UserId;
            entity.QuizID = model.QuizID;
            entity.Title = model.Title;
            //entity.CreatedDate = model.CreatedDate;
            //entity.CreatedBy = model.CreatedBy;
            entity.UpdatedDate = model.UpdatedDate;
            entity.UpdatedBy = model.UpdatedBy;
            entity.Status = model.Status;
            entity.Description = model.Description;

            return entity;
        }
        public static List<UserTest> MapToEntities(this List<UserTestModel> models)
        {
            return models.Select(x => x.MapToEntity()).ToList();
        }

        public static List<UserTestModel> MapToModels(this List<UserTest> entities)
        {
            return entities.Select(x => x.MapToModel()).ToList();
        }
        #endregion
    }
}
