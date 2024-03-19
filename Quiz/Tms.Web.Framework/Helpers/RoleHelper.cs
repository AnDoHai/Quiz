using Tms.Models;
using Tms.Web.Framework.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Tms.Web.Framework.Helpers
{
    public class RoleHelper
    {
        #region NguoiDung
        public const string NguoiDungDisplay = "nguoidung-displaynguoidung";

        public static bool IsNguoiDungDisplay()
        {
            if (HttpContext.Current.User.IsInRole(NguoiDungDisplay))
                return true;
            return false;
        }
        #endregion

        #region QuanTri
        //ADMIN
        public const string QuanTriDisplay = "quantri-displayquantri";

        public static bool IsQuanTriDisplay()
        {
            if (HttpContext.Current.User.IsInRole(QuanTriDisplay))
                return true;
            return false;
        }

        // Đề thi
        public const string QuizListing = "Quiz-Listing";
        public const string QuizCreate = "Quiz-Create";
        public const string QuizEdit = "Quiz-Edit";
        public const string QuizDelete = "Quiz-Delete";

        public static bool IsQuizListing()
        {
            if (HttpContext.Current.User.IsInRole(QuizListing))
                return true;
            return false;
        }

        public static bool IsQuizCreate()
        {
            if (HttpContext.Current.User.IsInRole(QuizCreate))
                return true;
            return false;
        }

        public static bool IsQuizEdit()
        {
            if (HttpContext.Current.User.IsInRole(QuizEdit))
                return true;
            return false;
        }

        public static bool IsQuizDelete()
        {
            if (HttpContext.Current.User.IsInRole(QuizDelete))
                return true;
            return false;
        }

        // Phần thi chung
        public const string ContestListing = "Contest-Listing";
        public const string ContestCreate = "Contest-Create";
        public const string ContestEdit = "Contest-Edit";
        public const string ContestDelete = "Contest-Delete";

        public static bool IsContestListing()
        {
            if (HttpContext.Current.User.IsInRole(ContestListing))
                return true;
            return false;
        }

        public static bool IsContestCreate()
        {
            if (HttpContext.Current.User.IsInRole(ContestCreate))
                return true;
            return false;
        }

        public static bool IsContestEdit()
        {
            if (HttpContext.Current.User.IsInRole(ContestEdit))
                return true;
            return false;
        }

        public static bool IsContestDelete()
        {
            if (HttpContext.Current.User.IsInRole(ContestDelete))
                return true;
            return false;
        }

        // Phần thi nhỏ
        public const string SectionListing = "Section-Listing";
        public const string SectionCreate = "Section-Create";
        public const string SectionEdit = "Section-Edit";
        public const string SectionDelete = "Section-Delete";

        public static bool IsSectionListing()
        {
            if (HttpContext.Current.User.IsInRole(SectionListing))
                return true;
            return false;
        }

        public static bool IsSectionCreate()
        {
            if (HttpContext.Current.User.IsInRole(SectionCreate))
                return true;
            return false;
        }

        public static bool IsSectionEdit()
        {
            if (HttpContext.Current.User.IsInRole(SectionEdit))
                return true;
            return false;
        }

        public static bool IsSectionDelete()
        {
            if (HttpContext.Current.User.IsInRole(SectionDelete))
                return true;
            return false;
        }

        // Câu hỏi
        public const string QuestionListing = "Question-Listing";
        public const string QuestionCreate = "Question-Create";
        public const string QuestionEdit = "Question-Edit";
        public const string QuestionDelete = "Question-Delete";

        public static bool IsQuestionListing()
        {
            if (HttpContext.Current.User.IsInRole(QuestionListing))
                return true;
            return false;
        }

        public static bool IsQuestionCreate()
        {
            if (HttpContext.Current.User.IsInRole(QuestionCreate))
                return true;
            return false;
        }

        public static bool IsQuestionEdit()
        {
            if (HttpContext.Current.User.IsInRole(QuestionEdit))
                return true;
            return false;
        }

        public static bool IsQuestionDelete()
        {
            if (HttpContext.Current.User.IsInRole(QuestionDelete))
                return true;
            return false;
        }

        // Lựa chọn
        public const string ChoiceListing = "Choice-Listing";
        public const string ChoiceCreate = "Choice-Create";
        public const string ChoiceEdit = "Choice-Edit";
        public const string ChoiceDelete = "Choice-Delete";

        public static bool IsChoiceListing()
        {
            if (HttpContext.Current.User.IsInRole(ChoiceListing))
                return true;
            return false;
        }

        public static bool IsChoiceCreate()
        {
            if (HttpContext.Current.User.IsInRole(ChoiceCreate))
                return true;
            return false;
        }

        public static bool IsChoiceEdit()
        {
            if (HttpContext.Current.User.IsInRole(ChoiceEdit))
                return true;
            return false;
        }

        public static bool IsChoiceDelete()
        {
            if (HttpContext.Current.User.IsInRole(ChoiceDelete))
                return true;
            return false;
        }
        // Trả lời

        public const string AnswerListing = "Answer-Listing";
        public const string AnswerCreate = "Answer-Create";
        public const string AnswerEdit = "Answer-Edit";
        public const string AnswerDelete = "Answer-Delete";

        public static bool IsAnswerListing()
        {
            if (HttpContext.Current.User.IsInRole(AnswerListing))
                return true;
            return false;
        }

        public static bool IsAnswerCreate()
        {
            if (HttpContext.Current.User.IsInRole(AnswerCreate))
                return true;
            return false;
        }

        public static bool IsAnswerEdit()
        {
            if (HttpContext.Current.User.IsInRole(AnswerEdit))
                return true;
            return false;
        }

        public static bool IsAnswerDelete()
        {
            if (HttpContext.Current.User.IsInRole(AnswerDelete))
                return true;
            return false;
        }
        #endregion

        #region  Roles

        public const string RoleListing = "Role-Listing";
        public const string RoleCreate = "Role-Create";
        public const string RoleEdit = "Role-Edit";
        public const string RoleDelete = "Role-Delete";
        public const string RoleInvisible = "Role-Invisible";

        public static bool IsRoleListing()
        {
            if (HttpContext.Current.User.IsInRole(RoleListing))
                return true;

            return false;
        }

        public static bool IsRoleCreate()
        {
            if (HttpContext.Current.User.IsInRole(RoleCreate))
                return true;

            return false;
        }

        public static bool IsRoleEdit()
        {
            if (HttpContext.Current.User.IsInRole(RoleEdit))
                return true;

            return false;
        }

        public static bool IsRoleDelete()
        {
            if (HttpContext.Current.User.IsInRole(RoleDelete))
                return true;

            return false;
        }

        public static bool IsRoleInvisible()
        {
            if (HttpContext.Current.User.IsInRole(RoleInvisible))
                return true;

            return false;
        }


        #endregion

        #region Configurations

        public const string ConfigurationSetting = "Configuration-Setting";

        public static bool IsSetting()
        {
            if (HttpContext.Current.User.IsInRole(ConfigurationSetting))
                return true;

            return false;
        }

        #endregion

        #region  Users

        public const string UserListing = "Users-Listing";
        public const string UserCreate = "Users-Create";
        public const string UserEdit = "Users-Edit";
        public const string UserDelete = "Users-Delete";
        public const string UserInvisibe = "Users-Invisible";

        public static bool IsUserListing()
        {
            if (HttpContext.Current.User.IsInRole(UserListing))
                return true;

            return false;
        }

        public static bool IsUserCreate()
        {
            if (HttpContext.Current.User.IsInRole(UserCreate))
                return true;

            return false;
        }

        public static bool IsUserEdit()
        {
            if (HttpContext.Current.User.IsInRole(UserEdit))
                return true;

            return false;
        }

        public static bool IsUserDelete()
        {
            if (HttpContext.Current.User.IsInRole(UserDelete))
                return true;

            return false;
        }

        public static bool IsUserInvisibe()
        {
            if (HttpContext.Current.User.IsInRole(UserInvisibe))
                return true;

            return false;
        }

        #endregion

        #region Helper methods

        public static string GetDisplayActionName(string actionType)
        {
            var result = string.Empty;
            switch (actionType)
            {
                case "Listing":
                    result = "Danh sách";
                    break;
                case "Create":
                    result = "Thêm";
                    break;
                case "Luu":
                    result = "Luu";
                    break;
            }

            return result;
        }

        public static string GetDisplayModuleName(string moduleType)
        {
            var result = string.Empty;
            switch (moduleType)
            {
                case "PKH":
                    result = "Hiển thị các danh mục phòng kế hoạch";
                    break;
                case "PKD":
                    result = "Hiển thị các danh mục phòng kinh doanh";
                    break;
            }
            return result;
        }

        #endregion
    }
}
