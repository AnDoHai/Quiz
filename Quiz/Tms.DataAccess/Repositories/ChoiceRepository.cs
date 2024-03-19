using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Tms.DataAccess.Common;

using Tms.DataAccess.LinqExtensions;

namespace Tms.DataAccess.Repositories
{
    public interface IChoiceRepository: IBaseRepository<Choice>
    {
        List<Choice> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);

        Choice GetByQuestionId(int id);
        List<Choice> GetAllByQuestionId(int id);
    }
    public class ChoiceRepository : BaseRepository<Choice>, IChoiceRepository
    {
        public ChoiceRepository(QuizSystemEntities context) : base(context)
        {
        }

        public List<Choice> GetAllByQuestionId(int id)
        {
            var query = Dbset.AsQueryable().Where(c=>c.QuestionID == id);
            if (query.Count() > 0)
            {
                return query.ToList();
            }
            return null;
        }

  

        public Choice GetByQuestionId(int id)
        {
            var query = Dbset.AsQueryable().Where(c => c.QuestionID == id);
            if (query.Count() > 0)
            {
                return query.FirstOrDefault();
            }
            return null;
        }

        public List<Choice> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection,
          out int totalPage)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            pageSize = (pageSize <= 0) ? 10 : pageSize;

            var query = Dbset.AsQueryable().Include(c=>c.Question);
            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(c => c.Title.Contains(textSearch) || c.ChoiceText.Contains(textSearch) || c.Description.Contains(textSearch));
            }

            totalPage = query.Count();

			if(!string.IsNullOrEmpty(sortColumn))
            {
                query = query.OrderByField(sortColumn.Trim(), sortDirection);
            }
            else
                query = query.OrderByDescending(c => c.ChoiceID);

            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
