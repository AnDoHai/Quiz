using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Tms.DataAccess.Common;

using Tms.DataAccess.LinqExtensions;

namespace Tms.DataAccess.Repositories
{
    public interface IAnswerRepository: IBaseRepository<Answer>
    {
        List<Answer> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);

        Answer GetByQuestionId( int questionId);

        List<Answer> GetListByQuestionId(int questionId);
        List<Answer> GetListByQuestionIds(List<int> questionIds);
    }
    public class AnswerRepository : BaseRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(QuizSystemEntities context) : base(context)
        {
        }

        public Answer GetByQuestionId(int questionId)
        {
            return Dbset.AsQueryable().Where(c => c.QuestionID == questionId).FirstOrDefault();
        }

        public List<Answer> GetListByQuestionId(int questionId)
        {
            return Dbset.AsQueryable().Where(c => c.QuestionID == questionId).ToList();
        }

        public List<Answer> GetListByQuestionIds(List<int> questionIds)
        {
            return Dbset.AsQueryable().Where(c => c.QuestionID.HasValue && questionIds.Contains(c.QuestionID.Value)).ToList();
        }

        public List<Answer> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection,
          out int totalPage)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            pageSize = (pageSize <= 0) ? 10 : pageSize;

            var query = Dbset.AsQueryable().Include(c=>c.Question);
            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(c => c.Title.Contains(textSearch) || c.AnswerText.Contains(textSearch) || c.Description.Contains(textSearch));
            }

            totalPage = query.Count();

			if(!string.IsNullOrEmpty(sortColumn))
            {
                query = query.OrderByField(sortColumn.Trim(), sortDirection);
            }
            else
                query = query.OrderByDescending(c => c.AnswerID);

            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
