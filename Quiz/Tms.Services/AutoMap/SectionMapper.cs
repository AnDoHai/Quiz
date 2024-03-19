using Tms.DataAccess;
using Tms.Models.SectionModel;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Tms.Services.AutoMap
{
    public static class SectionMapper
    {
        #region Mapping Section
        public static SectionModel MapToModel(this Section entity)
        {
            int? minuteStart = null;
            int? secoundStart = null;
            int? minuteEnd = null;
            int? secoundEnd = null;
            if (!string.IsNullOrEmpty(entity.TimeLimit))
            {
                var stringTime = entity.TimeLimit.Split(',');
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
            return new SectionModel
            {
                SectionID = entity.SectionID,
                SectionName = entity.SectionName,
                Title = entity.Title,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedBy,
                UpdatedDate = entity.UpdatedDate,
                UpdatedBy = entity.UpdatedBy,
                Status = entity.Status,
                MinuteStart = minuteStart,
                SecoundStart = secoundStart,
                MinuteEnd = minuteEnd,
                SecoundEnd = secoundEnd,
                OrderIndex = entity.OrderIndex.HasValue? entity.OrderIndex.Value:0,
                Description = entity.Description,
                Type = (int)entity.Type,
                Order = entity.Order,
                QuizID = entity.QuizID,
                ContestID = entity.ContestID,
                QuizName = (entity.Contest != null && entity.Contest.Quiz != null) ? entity.Contest.Quiz.QuizName : "",
                ContestName = entity.Contest != null ? entity.Contest.ContestName : "",

            };
        }
        public static SectionModel MapToModel(this Section entity, SectionModel model)
        {
            model.SectionID = entity.SectionID;
            model.SectionName = entity.SectionName;
            model.Title = entity.Title;
            model.CreatedDate = entity.CreatedDate;
            model.CreatedBy = entity.CreatedBy;
            model.UpdatedDate = entity.UpdatedDate;
            model.UpdatedBy = entity.UpdatedBy;
            model.Status = entity.Status;
            model.Description = entity.Description;
            model.Type = (int)entity.Type;
            model.Order = entity.Order;
            model.QuizID = entity.QuizID;
            model.ContestID = entity.ContestID;

            return model;
        }
        public static Section MapToEntity(this SectionModel model)
        {
            return new Section
            {
                SectionID = model.SectionID,
                SectionName = model.SectionName,
                Title = model.Title,
                CreatedDate = model.CreatedDate,
                CreatedBy = model.CreatedBy,
                UpdatedDate = model.UpdatedDate,
                UpdatedBy = model.UpdatedBy,
                OrderIndex = model.OrderIndex,
                Status = model.Status,
                Description = model.Description,
                Type = model.Type,
                TimeLimit = model.TimeLimit,
                Order = model.Order,
                QuizID = model.QuizID,
                ContestID = model.ContestID,

            };
        }
        public static Section MapToEntity(this SectionModel model, Section entity)
        {
            entity.SectionID = model.SectionID;
            entity.SectionName = model.SectionName;
            entity.Title = model.Title;
            //entity.CreatedDate = model.CreatedDate;
            //entity.CreatedBy = model.CreatedBy;
            entity.UpdatedDate = model.UpdatedDate;
            entity.UpdatedBy = model.UpdatedBy;
            entity.Status = model.Status;
            entity.TimeLimit = model.TimeLimit;
            entity.OrderIndex = model.OrderIndex;
            entity.Description = model.Description;
            entity.Type = model.Type;
            entity.Order = model.Order;
            entity.QuizID = model.QuizID;
            entity.ContestID = model.ContestID;

            return entity;
        }
        public static List<Section> MapToEntities(this List<SectionModel> models)
        {
            return models.Select(x => x.MapToEntity()).ToList();
        }

        public static List<SectionModel> MapToModels(this List<Section> entities)
        {
            return entities.Select(x => x.MapToModel()).ToList();
        }
        #endregion
    }
}
