using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Tms.DataAccess.Common;

using Tms.DataAccess.LinqExtensions;

namespace Tms.DataAccess.Repositories
{
    public interface IUserTestQuestionAnswerRepository: IBaseRepository<UserTestQuestionAnswer>
    {
        List<UserTestQuestionAnswer> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);

        List<UserTestQuestionAnswer> GetByUserTestId(int id);
        List<UserTestQuestionAnswer> GetByTestHightId(int id);
        UserTestQuestionAnswer GetAnswerByUserTestId(int userTestId);
        List<UserTestQuestionAnswer> GetByTestId(int id);
        List<UserTestQuestionAnswer> GetAllByUserTestId(int userTestId);

    }
    public class UserTestQuestionAnswerRepository : BaseRepository<UserTestQuestionAnswer>, IUserTestQuestionAnswerRepository
    {
        public UserTestQuestionAnswerRepository(QuizSystemEntities context) : base(context)
        {
        }

        public List<UserTestQuestionAnswer> GetAllByUserTestId(int userTestId)
        {
            var query = Dbset.AsQueryable().Include(c => c.UserTestQuestion).Include(c => c.UserTestQuestion.UserTest).Where(c=>c.UserTestQuestion.UserTest.UserTestId == userTestId);
            if (query.Count() > 0)
            {
                return query.ToList();
            }
            return null;
        }

        public UserTestQuestionAnswer GetAnswerByUserTestId(int userTestId)
        {
            var query = Dbset.AsQueryable().Include(c=>c.UserTestQuestion.UserTest).Where(c => c.UserTestQuestion.UserTest.UserTestId == userTestId);
            if (query.Count() > 0)
            {
                return query.FirstOrDefault();
            }
            return null;
        }
        public List<UserTestQuestionAnswer> GetByTestHightId(int id)
        {
            var query = Dbset.AsQueryable().Include(c => c.UserTestQuestion).Include(c => c.UserTestQuestion.UserTest).Include(c => c.UserTestQuestion.UserTest.Quiz).Include(c => c.UserTestQuestion.UserTest.Quiz.Category);
            return query.Where(c => c.UserTestQuestion.UserTest.UserTestId == id).ToList();
        }
        public List<UserTestQuestionAnswer> GetByTestId(int id)
        {
            var query = Dbset.AsQueryable().Include(c => c.UserTestQuestion).Include(c => c.UserTestQuestion.UserTest).Include(c=>c.UserTestQuestion.UserTest.Quiz).Include(c=>c.UserTestQuestion.UserTest.Quiz.Category);
            return query.Where(c => c.UserTestQuestion.UserTest.UserTestId == id && c.Point != null).ToList();
        }

        public List<UserTestQuestionAnswer> GetByUserTestId(int id)
        {
            var query = Dbset.AsQueryable().Include(c => c.UserTestQuestion).Include(c => c.UserTestQuestion.UserTest).Include(c=>c.UserTestQuestion.Question).Include(c => c.UserTestQuestion.Question).Include(c=> c.UserTestQuestion.Question.Answers);
            return query.Where(c=>c.UserTestQuestion.Question.Answers.Any() == false).Where(c => c.UserTestQuestion.UserTest.UserTestId == id).ToList();
        }

        public List<UserTestQuestionAnswer> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection,
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
                query = query.OrderByDescending(c => c.UserTestQuestionAnswerID);

            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
