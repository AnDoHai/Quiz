using System.Collections.Generic;
using System.Linq;
using Tms.DataAccess;
using Tms.Models.NewsModel;

namespace Tms.Services.AutoMap
{
    public static class NewsMapper
    {
        #region Mapping News
        public static NewsModel MapToModel(this News entity)
        {
            return new NewsModel
            {
				NewsId = entity.NewsId,
				Title = entity.Title,
				SortDescription = entity.SortDescription,
				Description = entity.Description,
				IsHot = entity.IsHot,
				CreatedDate = entity.CreatedDate,
				CreatedBy = entity.CreatedBy,
				UpdatedDate = entity.UpdatedDate,
				UpdatedBy = entity.UpdatedBy,
				Status = entity.Status,
				Author = entity.Author,
				CategoryNewsId = entity.CategoryNewsId,
				Image = entity.Image
            };
        }
        public static NewsModel MapToModel(this News entity, NewsModel model)
        {
			model.NewsId = entity.NewsId;
			model.Title = entity.Title;
			model.SortDescription = entity.SortDescription;
			model.Description = entity.Description;
			model.IsHot = entity.IsHot;
			model.CreatedDate = entity.CreatedDate;
			model.CreatedBy = entity.CreatedBy;
			model.UpdatedDate = entity.UpdatedDate;
			model.UpdatedBy = entity.UpdatedBy;
			model.Status = entity.Status;
			model.Author = entity.Author;
			model.CategoryNewsId = entity.CategoryNewsId;
			model.Image = entity.Image;
            return model;
        }
        public static News MapToEntity(this NewsModel model)
        {
            return new News
            {
				NewsId = model.NewsId,
				Title = model.Title,
				SortDescription = model.SortDescription,
				Description = model.Description,
				IsHot = model.IsHot,
				CreatedDate = model.CreatedDate,
				CreatedBy = model.CreatedBy,
				UpdatedDate = model.UpdatedDate,
				UpdatedBy = model.UpdatedBy,
				Status = model.Status,
				Author = model.Author,
				CategoryNewsId = model.CategoryNewsId,
				Image = model.Image
            };
        }
        public static News MapToEntity(this NewsModel model, News entity)
        {
			entity.NewsId = model.NewsId;
			entity.Title = model.Title;
			entity.SortDescription = model.SortDescription;
			entity.Description = model.Description;
			entity.IsHot = model.IsHot;
			//entity.CreatedDate = model.CreatedDate;
			//entity.CreatedBy = model.CreatedBy;
			entity.UpdatedDate = model.UpdatedDate;
			entity.UpdatedBy = model.UpdatedBy;
			entity.Status = model.Status;
			entity.Author = model.Author;
			entity.CategoryNewsId = model.CategoryNewsId;
			entity.Image = model.Image;
            return entity;
        }
        public static List<News> MapToEntities(this List<NewsModel> models)
        {
            return models.Select(x => x.MapToEntity()).ToList();
        }

        public static List<NewsModel> MapToModels(this List<News> entities)
        {
            return entities.Select(x => x.MapToModel()).ToList();
        }
        #endregion
    }
}
