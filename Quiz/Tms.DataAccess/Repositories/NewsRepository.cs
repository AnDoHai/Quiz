using EcommerceSystem.Core;
using System.Collections.Generic;
using System.Linq;
using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.LinqExtensions;

namespace Tms.DataAccess.Repositories
{
    public interface INewsRepository: IBaseRepository<News>
    {
        List<News> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);
        List<News> GetFeaturedNews(int top);
    }
    public class NewsRepository : BaseRepository<News>, INewsRepository
    {
        public NewsRepository(QuizSystemEntities context) : base(context)
        {
        }

        public List<News> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection,
          out int totalPage)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            pageSize = (pageSize <= 0) ? Constants.DefaultPageSize : pageSize;

            var query = Dbset.AsQueryable();
            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(c => c.Title.Contains(textSearch));
            }

            totalPage = query.Count();

			if(!string.IsNullOrEmpty(sortColumn))
            {
                query = query.OrderByField(sortColumn.Trim(), sortDirection);
            }
            else
                query = query.OrderByDescending(c => c.NewsId);

            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<News> GetFeaturedNews(int top)
        {
            return Dbset.AsQueryable().Take(top).ToList();
        }
    }
}
