using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tms.Models.Models.QuizModel
{
    public class ExamModelSreach
    {
        public int? UserId { get; set; }
        public int? QuizId { get; set; }
        public string QuizName { get; set; }
        public string HSKName { get; set; }
        public int HSKType { get; set; }
        public int LimitTime { get; set; }

        public List<ExamModel> ExamDetails { get; set; }
    }
}
