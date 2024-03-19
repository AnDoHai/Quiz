using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Tms.DataAccess.Common;

using Tms.DataAccess.LinqExtensions;

namespace Tms.DataAccess.Repositories
{
    public interface ISectionRepository: IBaseRepository<Section>
    {
        List<Section> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? contestId);
        List<Section> SearchDetail(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? hskId, int? quizId, int? contestId);

        List<Section> GetByContestId(int id);
    }
    public class SectionRepository : BaseRepository<Section>, ISectionRepository
    {
        public SectionRepository(QuizSystemEntities context) : base(context)
        {
        }

        public List<Section> GetByContestId(int id)
        {
            var query = Dbset.AsQueryable().Include(c => c.Contest).Where(c => c.ContestID == id);
            if (query.Count() > 0)
            {
                return query.ToList();
            }
            return null;
        }
        
        public List<Section> SearchDetail(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? hskId, int? quizId, int? contestId)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            pageSize = (pageSize <= 0) ? 10 : pageSize;

            var query = Dbset.AsQueryable().Include(c => c.Contest).Include(c => c.Contest.Quiz);
            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(c => c.Title.Contains(textSearch) || c.SectionName.Contains(textSearch) || c.SectionID.ToString().Contains(textSearch));
            }
            if (hskId != null)
            {
                query = query.Where(c => c.Contest.Quiz.Category.CategoryId == hskId);
            }
            if (quizId != null)
            {
                query = query.Where(c => c.Contest.Quiz.QuizID == quizId);
            }
            if (contestId != null)
            {
                query = query.Where(c => c.ContestID == contestId);
            }
            totalPage = query.Count();

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = query.OrderByField(sortColumn.Trim(), sortDirection);
            }
            else
                query = query.OrderByDescending(c => c.SectionID);

            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<Section> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection,
          out int totalPage, int? contestId)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            pageSize = (pageSize <= 0) ? 10 : pageSize;

            var query = Dbset.AsQueryable().Include(c => c.Contest).Include(c => c.Contest.Quiz);
            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(c => c.Title.Contains(textSearch) || c.SectionName.Contains(textSearch));
            }
            if(contestId != null)
            {
                query = query.Where(c => c.ContestID == contestId);
            }
            totalPage = query.Count();

			if(!string.IsNullOrEmpty(sortColumn))
            {
                query = query.OrderByField(sortColumn.Trim(), sortDirection);
            }
            else
                query = query.OrderByDescending(c => c.SectionID);

            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
