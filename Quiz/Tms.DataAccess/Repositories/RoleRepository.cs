using System.Collections.Generic;
using System.Linq;
using Tms.DataAccess.Common;
using Tms.DataAccess.LinqExtensions;

namespace Tms.DataAccess.Repositories
{
    public interface IRoleRepository: IBaseRepository<Role>
    {
        List<Role> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);
    }
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(QuizSystemEntities context) : base(context)
        {
        }

        public List<Role> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection,
          out int totalPage)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            var query = Dbset.AsQueryable();
            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(c => c.Name.Contains(textSearch) || c.Code.Equals(textSearch));
            }

            totalPage = query.Count();

            if(!string.IsNullOrEmpty(sortColumn))
            {
                query = query.OrderByField(sortColumn.Trim().Replace("\t" , ""), sortDirection);
            }
            else
                query = query.OrderByDescending(c => c.RoleId);

            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
