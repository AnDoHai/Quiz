using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Tms.DataAccess.Common;

using Tms.DataAccess.LinqExtensions;

namespace Tms.DataAccess.Repositories
{
    public interface IUserTestQuestionRepository: IBaseRepository<UserTestQuestion>
    {
        List<UserTestQuestion> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);
        UserTestQuestion GetByQuestionId(int id);

        List<UserTestQuestion> GetByUserTestId(int id);

        List<UserTestQuestion> GetAllByQuestionId(int id);
    }
    public class UserTestQuestionRepository : BaseRepository<UserTestQuestion>, IUserTestQuestionRepository
    {
        public UserTestQuestionRepository(QuizSystemEntities context) : base(context)
        {
        }

        public List<UserTestQuestion> GetAllByQuestionId(int id)
        {
            var query = Dbset.AsQueryable().Include(c=>c.UserTestQuestionAnswers).Where(c => c.QuestionID == id);
            if (query.Count() > 0)
            {
                query.ToList();
            }
            return null;
        }

        public UserTestQuestion GetByQuestionId(int id)
        {
            var query = Dbset.AsQueryable().Where(c=>c.QuestionID == id);
            if (query.Count() > 0)
            {
                query.FirstOrDefault();
            }
            return null;
        }

        public List<UserTestQuestion> GetByUserTestId(int id)
        {
            var query = Dbset.AsQueryable()
                .Include(c => c.UserTestQuestionAnswers)
                .Include(c => c.Contest)
                .Where(c => c.UserTestId == id);
            return query.ToList();
        }

        public List<UserTestQuestion> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection,
          out int totalPage)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            pageSize = (pageSize <= 0) ? 10 : pageSize;

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
                query = query.OrderByDescending(c => c.UserTestQuestionId);

            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
