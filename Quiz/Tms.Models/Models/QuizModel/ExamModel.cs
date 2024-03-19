using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tms.Models.Models.QuizModel
{
    public class ExamModel
    {
        public int? ContestID { get; set; }
        public string ContestName { get; set; }
        public int ContestType { get; set; }
        public int LimitTime { get; set; }
        public List<SectionExamModel> Sections { get; set; }
    }
    public class SectionExamModel
    {
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public int SectionType { get; set; }
        public int SectionOrder { get; set; }
        public string Tittle { get; set; }
        public string TimeLimit { get; set; }
        public string Description { get; set; }
        public List<QuestionExamModel> QuestionModels { get; set; }
    }
    public class QuestionExamModel
    {
        public int QuestionID { get; set; }
        public string QuestionName { get; set; }
        public double Point { get; set; }
        public string AudioUrl { get; set; }
        public string Image { get; set; }
        public bool StatusTextbox { get; set; }
        public string Tittle { get; set; }
        public int? Layout { get; set; }
        public int? TimeLimit { get; set; }
        public int Maxlength { get; set; }
        public int QuestionOrder { get; set; }
        public int AnswerType { get; set; }
        public int Type { get; set; }
        public List<Tms.Models.ChoiceModel.ChoiceModel> Choices { get; set; }
    }

    public class StatisticalDetailExam
    {
        public string NameContest { get; set; }
        public int TotalNumber { get; set; }
        public int CorrectNumber { get; set; }
        public double TotalPoint { get; set; }
    }

    public class StatisticalAllExam
    {
        public string ExamName { get; set; }
        public DateTime CraeteDate { get; set; }
        public int totalQuestion { get; set; }
        public int CorrectNumberAll { get; set; }
        public double TotalPoint { get; set; }
        public List<StatisticalDetailExam> StatisticalDetails { get; set; }
    }

    public class StatisticalExamFileUpload
    {
        public string currenUserName { get; set; }
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public int questionId { get; set; }
        public int contestId { get; set; }
        public int sectionId { get; set; }
        public string textAnswer { get; set; }
    }

    public class ExamUpload {
        public int ContestID { get; set; }
        public int ContestType { get; set; }
        public int SectionID { get; set; }
        public int QuestionID { get; set; }
        public double Point { get; set; }
        public int QuestionType { get; set; }
        public int? ChoiceID { get; set; }
        public string ChoiceText { get; set; }
        public int ChoiceType { get; set; }
    }


    public class ExamHightModel
    {
        public int QuizId { get; set; }
        public string QuizName { get; set; }
        public string HSKName { get; set; }
        public string HSKType { get; set; }
        public int LimitTime { get; set; }
        public string FileMedia { get; set; }
        public List<SectionExamModel> Sections { get; set; }
        public List<QuestionExamModel> Questions { get; set; }
    }

}
