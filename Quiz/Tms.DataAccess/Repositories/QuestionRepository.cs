using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Tms.DataAccess.Common;

using Tms.DataAccess.LinqExtensions;

namespace Tms.DataAccess.Repositories
{
    public interface IQuestionRepository: IBaseRepository<Question>
    {
        List<Question> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);
        List<Question> SearchQuestion(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? hskId, int? quizId, int? contestId, int? sectionId);
       
        List<Question> GetAllExam(int id);
        List<Question> GetAllQuestionExam(List<int> secsionIds);

        List<Question> GetAllQuestion();
        Question GetQuestionById(int id);
        List<Question> GetAllQuestion(List<int> secsionIds);
    }
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(QuizSystemEntities context) : base(context)
        {
        }

        public List<Question> GetAllExam(int id)
        {
            var query = Dbset.AsQueryable();
            query = query.Include(c => c.Contest).Include(c => c.Section).Include(c => c.Answers).Where(c => c.QuizID == id);
            var count = query.Count();
            if (count != 0)
            {
                return query.ToList();
            }
            return null;
        }
        public List<Question> GetAllQuestionExam(List<int> secsionIds)
        {
            var query = Dbset.AsQueryable().Include(c => c.Choices).Where(c => c.SectionID.HasValue && secsionIds.Contains(c.SectionID.Value) && c.Status == true);
            return query.ToList();
        }
        public List<Question> GetAllQuestion(List<int> secsionIds)
        {
            var query = Dbset.AsQueryable().Include(c => c.Answers).Include(c=>c.Choices).Include(c=>c.UserTestQuestions.Select(g=>g.UserTestQuestionAnswers)).Where(c=> c.SectionID.HasValue && secsionIds.Contains(c.SectionID.Value) && c.Status == true);
            return query.ToList();
        }

        public List<Question> GetAllQuestion()
        {
           return Dbset.AsQueryable().Include(c => c.Contest).Include(c => c.Section).ToList();
        }

        public Question GetQuestionById(int id)
        {
            var query = Dbset.AsQueryable().Include(c => c.Answers).Include(c=>c.Section).Include(c=>c.Choices).Include(c=>c.Contest).Where(c=>c.QuestionID == id); 
            if (query.Count() > 0)
            {
                return query.FirstOrDefault();
            }
            return null;
        }

        public List<Question> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection,
          out int totalPage)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            pageSize = (pageSize <= 0) ? 10 : pageSize;

            var query = Dbset.AsQueryable().Include(c=>c.Contest).Include(c=>c.Quiz).Include(c=>c.Section);
            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(c => c.Title.Contains(textSearch) || c.QuestionText.Contains(textSearch) || c.Quiz.QuizName.Contains(textSearch) || c.Contest.ContestName.Contains(textSearch));
            }

            totalPage = query.Count();

			if(!string.IsNullOrEmpty(sortColumn))
            {
                query = query.OrderByField(sortColumn.Trim(), sortDirection);
            }
            else
                query = query.OrderByDescending(c => c.QuestionID);

            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<Question> SearchQuestion(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage, int? hskId, int? quizId, int? contestId, int? sectionId)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            pageSize = (pageSize <= 0) ? 10 : pageSize;

            var query = Dbset.AsQueryable().Include(c => c.Contest).Include(c => c.Quiz).Include(c=>c.Quiz.Category).Include(c => c.Section);
            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(c => c.Title.Contains(textSearch) || c.QuestionText.Contains(textSearch) || c.Quiz.QuizName.Contains(textSearch) || c.Contest.ContestName.Contains(textSearch) || c.QuestionID.ToString().Contains(textSearch));
            }
            if (sectionId != null)
            {
                query = query.Where(c => c.SectionID == sectionId);
            }
            if (contestId != null)
            {
                query = query.Where(c => c.ContestID == contestId);
            }
            if (quizId != null)
            {
                query = query.Where(c => c.QuizID == quizId);
            }
            if (hskId != null)
            {
                query = query.Where(c => c.Quiz.CategoryId == hskId);
            }


            totalPage = query.Count();

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = query.OrderByField(sortColumn.Trim(), sortDirection);
            }
            else
                query = query.OrderByDescending(c => c.QuestionID);

            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
