using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Tms.DataAccess.Common;

using Tms.DataAccess.LinqExtensions;

namespace Tms.DataAccess.Repositories
{
    public interface IContestRepository: IBaseRepository<Contest>
    {
        List<Contest> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? quizId);

        List<Contest> GetAllContest(int id);
        List<Contest> GetAllContestsIndex();

        List<Contest> GetAllContests();
    }
    public class ContestRepository : BaseRepository<Contest>, IContestRepository
    {
        public ContestRepository(QuizSystemEntities context) : base(context)
        {
        }

        public List<Contest> GetAllContest(int id)
        {
            var query = Dbset.AsQueryable().Include(c => c.Sections).Where(c => c.QuizID == id && c.Status == 0);
            return query.ToList();
        }

        public List<Contest> GetAllContests()
        {
            var query = Dbset.AsQueryable().Include(c => c.Quiz).Include(c => c.Quiz.Category);
            if (query.Count() > 0)
            {
                return query.OrderBy(c => c.Quiz.Category.Title).ThenBy(c => c.Quiz.QuizName).ToList();
            }
            return null;
        }
        public List<Contest> GetAllContestsIndex()
        {
            var query = Dbset.AsQueryable().Include(c => c.Quiz);
            if (query.Count() > 0)
            {
                return query.OrderBy(c => c.Quiz.Category.Title).ThenBy(c => c.Quiz.QuizName).ToList();
            }
            return null;
        }

        public List<Contest> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection,
          out int totalPage, int? quizId)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            pageSize = (pageSize <= 0) ? 10 : pageSize;

            var query = Dbset.AsQueryable().Include(c => c.Quiz);
            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(c => c.Title.Contains(textSearch) || c.ContestName.Contains(textSearch) || c.ContestID.ToString().Contains(textSearch));
            }

            if(quizId != null)
            {
                query = query.Where(c => c.QuizID == quizId);
            }

            totalPage = query.Count();

			if(!string.IsNullOrEmpty(sortColumn))
            {
                query = query.OrderByField(sortColumn.Trim(), sortDirection);
            }
            else
                query = query.OrderByDescending(c => c.ContestID);

            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
