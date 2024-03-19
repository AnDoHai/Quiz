using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tms.Models.ChoiceModel;
using Tms.Models.AnswerModel;
using Tms.Models.Models.QuizModel;

namespace Tms.Models.UserTestModel
{
    //Base on UserTestModel
    public class ExamResultModel
    {
        public int UserTestID { get; set; }
        public int? UserID { get; set; }
        public int? QuizID { get; set; }
        public QuizExamResult QuizExam { get; set; }
        public List<UserTestQuestionModel.UserTestQuestionModel> UserTestQuestions { get; set; }
    }

    public class ExamResultModelHight
    {
        public int UserTestID { get; set; }
        public int? UserID { get; set; }
        public int? QuizID { get; set; }
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public string ExamName { get; set; }
        public DateTime CraeteDate { get; set; }
        public bool Status { get; set; }
        public int TotalQuestion { get; set; }
        public double TotalPoint { get; set; }
        public int ContestID { get; set; }
        public string ContestName { get; set; }
        public string QuizName { get; set; }
        public string UserName { get; set; }
        public List<QuestionExamResultHight> UserTestExams { get; set; }
    }



    public class QuizExamResult
    {
        public int QuizID { get; set; }
        public string QuizName { get; set; }
        public string HSKName { get; set; }
        public List<ContestExamResult> ContestExams { get; set; }
    }

    public class ContestExamResult
    {
        public int ContestID { get; set; }
        public string ContestName { get; set; }
        public int ContestType { get; set; }
        public List<SectionExamResult> SectionExams { get; set; }
    }

    public class SectionExamResult
    {
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public int SectionType { get; set; }
        public int SectionOrder { get; set; }
        public string Tittle { get; set; }
        public string Description { get; set; }
        public List<QuestionExamResult> QuestionExams { get; set; }
    }

    public class QuestionExamResult
    {
        public int QuestionID { get; set; }
        public string QuestionName { get; set; }
        public double Point { get; set; }
        public string AudioUrl { get; set; }
        public string Image { get; set; }
        public int? Layout { get; set; }
        public int? TimeLimit { get; set; }
        public string Desscription { get; set; }
        public int QuestionOrder { get; set; }
        public int StatusPoint {get;set;}
        public int AnswerType { get; set; }
        public int Type { get; set; }
        public List<ChoiceModel.ChoiceModel> Choices { get; set; }
        public List<AnswerModel.AnswerModel> Answers { get; set; }
    }

     public class QuestionExamResultHight
    {
        public int QuestionID { get; set; }
        public string QuestionName { get; set; }
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public int ContestID { get; set; }
        public string ContestName { get; set; }
        public bool Status { get; set; }
        public double Point { get; set; }
        public int Type { get; set; }
    }


    public class UserTestQuestionExamResult
    {
        public int UserTestQuestionID { get; set; }
        public int QuestionID { get; set; }
        public UserTestQuestionAnswerExamResult UserTestQuestionAnswer { get; set; }
    }

    public class UserTestQuestionAnswerExamResult
    {
        public int UserTestQuestionAnswerID { get; set; }
        // ChoiceText, compare to AnswerText
        public string UserTestQuestionAnswerText { get; set; }
    }
}
