using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Tms.DataAccess.Common;

using Tms.DataAccess.LinqExtensions;

namespace Tms.DataAccess.Repositories
{
    public interface IUserTestRepository : IBaseRepository<UserTest>
    {
        List<UserTest> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, DateTime? dateTime, out int totalPage);
        List<UserTest> SearchByUser(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? userId = null);

        List<UserTest> SearchAllUsers(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, DateTime? stringTime, out int totalPage);
        List<UserTest> GetByQuizId(int quizId, int UserId);
        List<UserTest> GetAlls();
        UserTest GetQuizById(int id);
        UserTest GetUserTestByQuizId(int quizId);
        List<UserTest> GetByQuizId(int quizId);

        UserTest GetUserTestById(int userTestId);
        List<UserTest> GetByUserTestId(int quizId, int questionId);
    }
    public class UserTestRepository : BaseRepository<UserTest>, IUserTestRepository
    {
        public UserTestRepository(QuizSystemEntities context) : base(context)
        {
        }

        public List<UserTest> GetByUserTestId(int quizId, int questionId)
        {
            var query = Dbset.AsQueryable().Include(c => c.UserTestQuestions).Include(c=>c.UserTestQuestions.Select(g=>g.UserTestQuestionAnswers)).Where(c => c.QuizID == quizId && c.UserTestQuestions.Where(g=>g.QuestionID == questionId).Any());
            return query.ToList();
        }
        public List<UserTest> GetByQuizId(int quizId, int UserId)
        {
            var query = Dbset.AsQueryable().Include(c => c.Quiz).Where(c => c.QuizID == quizId && c.UserId == UserId);
            return query.ToList();
        }
        // Lấy bài thi theo id bài thi
        public UserTest GetQuizById(int id)
        {
            var query = Dbset.AsQueryable().Include(c => c.Quiz).Include(c=>c.Quiz.Category).Include(c=>c.UserTestQuestions).Include(c=>c.UserTestQuestions.Select(g=>g.UserTestQuestionAnswers)).Where(c => c.UserTestId == id);
            return query.FirstOrDefault();
        }
        // Lấy đề thi theo id đề thi
        public UserTest GetUserTestByQuizId(int quizId)
        {
            var query = Dbset.AsQueryable().Include(c => c.Quiz).Where(c => c.QuizID == quizId);
            return query.FirstOrDefault();
        }

        public List<UserTest> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection,DateTime? dateTime,
          out int totalPage)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            pageSize = (pageSize <= 0) ? 10 : pageSize;

            var query = Dbset.AsQueryable().Include(c => c.Quiz).Include(c=>c.User).Include(c=>c.Quiz.Category).Include(c=>c.Quiz.Contests).Include(c=>c.UserTestQuestions).Include(c=>c.UserTestQuestions.Select(g=>g.UserTestQuestionAnswers)).Where(c => c.UserTestQuestions.Any(q => q.Question.Type == 3 || q.Question.Type == 4 || q.Question.Type == 5 || q.Question.Type == 6));

            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(c => c.Quiz.QuizName.Contains(textSearch) || c.Title.Contains(textSearch) || c.User.UserName.ToLower().Contains(textSearch.ToLower()) || c.User.FullName.ToLower().Contains(textSearch.ToLower()));
            }
            if (dateTime != null)
            {
                query = query.Where(c => c.CreatedDate.Day == dateTime.Value.Day && c.CreatedDate.Month == dateTime.Value.Month && c.CreatedDate.Year == dateTime.Value.Year);
            }

            totalPage = query.Count();

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = query.OrderByField(sortColumn.Trim(), sortDirection);
            }
            else
                query = query.OrderByDescending(c => c.UserTestId);

            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<UserTest> SearchByUser(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? userId = null)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            pageSize = (pageSize <= 0) ? 10 : pageSize;

            var query = Dbset.AsQueryable()
                .Include(c => c.Quiz)
                .Include(c => c.Quiz.Category)
                .Include(c => c.UserTestQuestions);
            if (userId != null)
            {
                query = query.Where(c => c.UserId.Value == userId);
            }
            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(c => c.Quiz.QuizName.Contains(textSearch) || c.Title.Contains(textSearch));
            }

            query = query.Where(c => Math.Round(c.TotalPoint.Value) > 0 && c.TotalPoint.HasValue || (c.Quiz.Category.Type == 8 || c.Quiz.Category.Type == 9 || c.Quiz.Category.Type == 10));

            totalPage = query.Where(c => Math.Round(c.TotalPoint.Value) > 0 && c.TotalPoint.HasValue || (c.Quiz.Category.Type == 8 || c.Quiz.Category.Type == 9 || c.Quiz.Category.Type == 10)).Count();

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = query.OrderByField(sortColumn.Trim(), sortDirection);
            }
            else
                query = query.OrderByDescending(c => c.UserTestId);

            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<UserTest> SearchAllUsers(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, DateTime? stringTime, out int totalPage)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            pageSize = (pageSize <= 0) ? 10 : pageSize;

            var query = Dbset.AsQueryable()
                .Include(c => c.Quiz)
                .Include(c => c.Quiz.Category)
                .Include(c => c.UserTestQuestions)
                .Include(c=>c.Quiz.Contests)
                .Include(c=>c.User)
                .Include(c => c.UserTestQuestions.Select(o => o.UserTestQuestionAnswers));

            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(c => c.Quiz.QuizName.Contains(textSearch) || c.Title.Contains(textSearch) || c.User.FullName.Contains(textSearch) || c.User.UserName.Contains(textSearch) || c.User.Email.Contains(textSearch));
            }
            if (stringTime != null)
            {
                query = query.Where(c => c.CreatedDate.Day == stringTime.Value.Day && c.CreatedDate.Month == stringTime.Value.Month && c.CreatedDate.Year == stringTime.Value.Year);
            }

            totalPage = query.Where(c => Math.Round(c.TotalPoint.Value) > 0 || (c.Quiz.Category.Type == 8 || c.Quiz.Category.Type == 9 || c.Quiz.Category.Type == 10)).Count();

            query = query.Where(c => Math.Round(c.TotalPoint.Value) > 0 || (c.Quiz.Category.Type == 8 || c.Quiz.Category.Type == 9 || c.Quiz.Category.Type == 10));

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = query.OrderByField(sortColumn.Trim(), sortDirection);
            }
            else
                query = query.OrderByDescending(c => c.UserTestId);

            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public UserTest GetUserTestById(int userTestId)
        {
            var query = Dbset.AsQueryable().Include(c=>c.UserTestQuestions).Include(c=>c.UserTestQuestions.Select(g=>g.UserTestQuestionAnswers)).Where(c=>c.UserTestId == userTestId);
            if (query.Count() > 0)
            {
                return query.FirstOrDefault();
            }
            return null;
        }

        public List<UserTest> GetAlls()
        {
            var query = Dbset.AsQueryable().Include(c => c.UserTestQuestions).Include(c => c.UserTestQuestions.Select(g => g.UserTestQuestionAnswers));
            if (query != null && query.Any())
            {
                return query.ToList();
            }
            return null;
        }

        public List<UserTest> GetByQuizId(int quizId)
        {
            var query = Dbset.AsQueryable().Include(c => c.UserTestQuestions).Include(c => c.UserTestQuestions.Select(g => g.UserTestQuestionAnswers)).Where(c => c.QuizID == quizId);
            if (query.Count() > 0)
            {
                return query.OrderByDescending(c=>c.UserTestId).Skip(200).Take(100).ToList();
            }
            return null;
        }
    }
}
