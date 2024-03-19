using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Tms.DataAccess.Common;

using Tms.DataAccess.LinqExtensions;

namespace Tms.DataAccess.Repositories
{
    public interface IQuizRepository: IBaseRepository<Quiz>
    {
        List<Quiz> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? categoryId);
        List<Quiz> SearchByCategory(int currentPage, int pageSize, string sortColumn, string sortDirection, out int totalPage, int categoryId);
        List<Quiz> GetAllQuiz(int id);
        List<Quiz> GetAllQuiz();
        Quiz GetCategoryByQuizId(int? id);
        List<Quiz> GetAllQuizIndex();
        List<Quiz> GetByCategoryIndexId(int id);
        List<Quiz> GetFeaturedQuiz(int top);

        List<Quiz> GetByCategoryId(int id);

        Quiz GetByQuizId(int? id);
    }
    public class QuizRepository : BaseRepository<Quiz>, IQuizRepository
    {
        public QuizRepository(QuizSystemEntities context) : base(context)
        {
        }

        public List<Quiz> GetAllQuiz(int id)
        {
            var query = Dbset.AsQueryable()
                .Include(c=>c.Contests)
                .Include(c=>c.Questions)
                .Include(c=>c.Questions.Select(o=>o.Section))
                .Include(c=>c.Questions.Select(g=>g.Answers))
                .Where(c=>c.QuizID == id);
            return query.ToList(); 
        }

        public List<Quiz> GetAllQuiz()
        {
            var query = Dbset.AsQueryable()
                .Include(c => c.Category)
                .Include(c => c.Contests)
                .Include(c => c.Questions)
                .Include(c => c.Questions.Select(o => o.Section))
                .Include(c => c.Questions.Select(g => g.Answers));
            return query.OrderBy(c => c.Category.Title).ThenBy(c => c.QuizName).ToList();
        }

        public List<Quiz> GetAllQuizIndex()
        {
            var query = Dbset.AsQueryable()
                .Include(c => c.Category)
                .Include(c => c.Contests);
            return query.OrderBy(c => c.Category.Title).ThenBy(c => c.QuizName).ToList();
        }

        public List<Quiz> GetByCategoryIndexId(int id)
        {
            var query = Dbset.AsQueryable()
                .Include(c => c.Contests)
                .Where(c => c.CategoryId.Value == id);
            return query.ToList();
        }
        public List<Quiz> GetByCategoryId(int id)
        {
            var query = Dbset.AsQueryable()
                .Include(c => c.Contests)
                .Include(c => c.Questions)
                .Include(c => c.Questions.Select(o => o.Section))
                .Include(c => c.Questions.Select(g => g.Answers))
                .Where(c => c.CategoryId.Value == id);
            return query.ToList();
        }
        public List<Quiz> GetFeaturedQuiz(int top)
        {
            var query = Dbset.AsQueryable()
                .Include(c => c.Contests)
                .Include(c => c.Questions)
                .Include(c => c.Questions.Select(o => o.Section))
                .Include(c => c.Questions.Select(g => g.Answers));
            return query.Take(top).ToList();
        }
        public Quiz GetCategoryByQuizId(int? id)
        {
            var query = Dbset.AsQueryable()
               .Include(c => c.Category)
               .Where(c => c.QuizID == id);
            return query.FirstOrDefault();
        }
        public Quiz GetByQuizId(int? id)
        {
            var query = Dbset.AsQueryable()
               .Include(c => c.Contests)
               .Include(c=>c.Category)
               .Include(c => c.Questions)
               .Include(c => c.Questions.Select(o => o.Section))
               .Include(c => c.Questions.Select(g => g.Answers))
               .Where(c => c.QuizID == id);
            return query.FirstOrDefault();
        }
        public List<Quiz> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection,
          out int totalPage, int? categoryId)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            pageSize = (pageSize <= 0) ? 10 : pageSize;

            var query = Dbset.AsQueryable().Include(c => c.Category);
            if(categoryId != null)
            {
                query = query.Where(c => c.CategoryId.Value == categoryId);
            }
            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(c => c.Title.Contains(textSearch) || c.QuizName.Contains(textSearch));
            }

            totalPage = query.Count();

			if(!string.IsNullOrEmpty(sortColumn))
            {
                query = query.OrderByField(sortColumn.Trim(), sortDirection);
            }
            else
                query = query.OrderByDescending(c => c.QuizID);

            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<Quiz> SearchByCategory(int currentPage, int pageSize, string sortColumn, string sortDirection, out int totalPage, int categoryId)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            pageSize = (pageSize <= 0) ? 10 : pageSize;

            var query = Dbset.AsQueryable().Include(c => c.Category);
            query = query.Where(c => c.CategoryId == categoryId);

            totalPage = query.Count();

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = query.OrderByField(sortColumn.Trim(), sortDirection);
            }
            else
                query = query.OrderByDescending(c => c.QuizID);

            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
