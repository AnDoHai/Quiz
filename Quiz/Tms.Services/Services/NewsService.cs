using System;
using System.Collections.Generic;
using System.Linq;
using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.Repositories;
using Tms.Models.NewsModel;
using Tms.Services.AutoMap;

namespace Tms.Services
{
    public interface INewsService : IEntityService<News>
    {
        List<NewsModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);
        bool UpdateNews(NewsModel NewsModel, out string message);
        bool Delete(int newsId, out string message);
        List<NewsModel> GetAllNewss();
        List<NewsModel> ListFeaturedNews(int top);
        bool CreateNews(NewsModel model);
		bool ChangeStatus(int newsId, out string message);

    }
    public class NewsService : EntityService<News>, INewsService
    {
        private readonly INewsRepository _newsRepository;
        public NewsService(IUnitOfWork unitOfWork, INewsRepository newsRepository)
            : base(unitOfWork, newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public List<NewsModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage)
        {
			try
            {
                var newsEntities = _newsRepository.Search(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalPage);
				if (newsEntities != null)
				{
					return newsEntities.MapToModels();
				}
            }
            catch (Exception ex)
            {
                Log.Error("Search News error", ex);
            }
            totalPage = 0;
            return null;
        }

        public bool UpdateNews(NewsModel newsModel, out string message)
        {
            try
            {
                var newsEntity = _newsRepository.GetById(newsModel.NewsId);
				if (newsEntity != null)
				{
					newsEntity = newsModel.MapToEntity(newsEntity);

					_newsRepository.Update(newsEntity);
					UnitOfWork.SaveChanges();

					message = "Cập nhật thành công";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Update News error", ex);
            }
            message = "Cập nhật bản ghi thất bại.";
            return false;
        }

        public bool CreateNews(NewsModel model)
        {
            try
            {
                var createdNews = _newsRepository.Insert(model.MapToEntity());
                UnitOfWork.SaveChanges();
                var errorMsg = string.Empty;
                if (createdNews == null)
                {
                    Log.Error("Create news error");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Create News error", ex);
                return false;
            }

        }

        public bool Delete(int newsId, out string message)
        {
            

            try
            {
                var entity = _newsRepository.GetById(newsId);
				if (entity != null)
				{
					_newsRepository.Delete(newsId);
					UnitOfWork.SaveChanges();

					message = "Xóa bản ghi thành công";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Delete News error", ex);
            }

            message = "Xóa bản ghi thất bại";
            return false;
        }

        public List<NewsModel> GetAllNewss()
        {
            //Igrone news system
            return _newsRepository.GetAll().ToList().MapToModels();
        }

        public List<NewsModel> ListFeaturedNews(int top)
        {
            return _newsRepository.GetFeaturedNews(top).OrderByDescending(x => x.CreatedDate).ToList().MapToModels();
        }


        public bool ChangeStatus(int newsId, out string message)
        {
            try
            {
                var entity = _newsRepository.Query(c => c.NewsId == newsId).FirstOrDefault();
				if (entity != null)
				{
					if (entity.Status)
					{
						entity.Status = false;
					}
					else
					{
						entity.Status = true;
					}

					_newsRepository.Update(entity);
					UnitOfWork.SaveChanges();

					message = "Cập nhật trạng thái thành công.";
					return true;
				}
            }
            catch (Exception ex)
            {
                Log.Error("Delete News error", ex);
            }

            message = "Cập nhật trạng thái thất bại.";
            return false;
        }
    }
}
