using System.Collections.Generic;
using System.Linq;
using EcommerceSystem.Core;
using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.LinqExtensions;

namespace Tms.DataAccess.Repositories
{
    public interface ICategoryNewRepository: IBaseRepository<CategoryNew>
    {
        List<CategoryNew> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);
    }
    public class CategoryNewRepository : BaseRepository<CategoryNew>, ICategoryNewRepository
    {
        public CategoryNewRepository(QuizSystemEntities context) : base(context)
        {
        }

        public List<CategoryNew> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection,
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
                query = query.OrderByDescending(c => c.CategoryNewsId);

            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
