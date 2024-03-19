using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Security;
using Tms.Models;
using Tms.Services;

namespace Tms.Web.Framework.Security
{
    public class TmsMembershipProvider : MembershipProvider
    {
        #region Properties

        private int _cacheTimeoutInMinutes = 30;

        #endregion

        public override void Initialize(string name, NameValueCollection config)
        {
            int val;
            if (!string.IsNullOrEmpty(config["cacheTimeoutInMinutes"]) && Int32.TryParse(config["cacheTimeoutInMinutes"], out val))
                _cacheTimeoutInMinutes = val;

            base.Initialize(name, config);
        }

        public override bool ValidateUser(string username, string password)
        {
            var userServices = DependencyResolver.Current.GetService(typeof(IUserService)) as IUserService;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            if (userServices != null)
            {
                string message;
                var result = userServices.ValidateLogon(username, password, out message);
                if (result != null && result.UserId != 0)
                {
                    return true;
                }
            }
            return false;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var cacheKey = string.Format(Constant.CacheUserLoginKey, username);
            if (System.Web.HttpRuntime.Cache[cacheKey] != null)
            {
                var currentUser = (Tms.Models.User.UserCommon)System.Web.HttpRuntime.Cache[cacheKey];
                var menuType = 0;
                var xuongId = 0;
                string xuongName = "";
                var requestCookie = System.Web.HttpContext.Current.Request.Cookies["menuViewTypeClient"];
                if (requestCookie != null)
                {
                    int.TryParse(requestCookie.Value, out menuType);
                }
                currentUser.MenuType = menuType;

                requestCookie = System.Web.HttpContext.Current.Request.Cookies["xuongViewType"];
                if (requestCookie != null)
                {
                    int.TryParse(requestCookie.Value, out xuongId);
                    if(xuongId > 0)
                        currentUser.XuongId = xuongId;
                }
                else if (menuType == 0)
                {
                    //reset phòng kế hoạch
                    currentUser.XuongId = 0;
                }
                else if(currentUser.NhomXuongIds != null && currentUser.NhomXuongIds.Count > 0)
                {
                    currentUser.XuongId = currentUser.NhomXuongIds[0];
                }    

                requestCookie = System.Web.HttpContext.Current.Request.Cookies["xuongViewName"];
                if (requestCookie != null)
                {
                    xuongName = HttpUtility.UrlDecode(requestCookie.Value.ToString());
                    if (!string.IsNullOrEmpty(xuongName))
                        currentUser.XuongName = xuongName;
                }

                var membershipUser = new TmsMembershipUser(currentUser);
                return membershipUser;
            }

            //Get Data from Database
            var userServices = DependencyResolver.Current.GetService(typeof(IUserService)) as IUserService;
            if (userServices != null)
            {
                var userResult = userServices.GetUserByEmail(username);
                if (userResult == null || userResult.UserId == 0)
                {
                    return null;
                }
                var menuType = 0;
                var xuongId = 0;
                string xuongName = "";
                var requestCookie = System.Web.HttpContext.Current.Request.Cookies["menuViewTypeClient"];
                if(requestCookie != null)
                {
                    int.TryParse(requestCookie.Value, out menuType);
                }
                userResult.MenuType = menuType;

                requestCookie = System.Web.HttpContext.Current.Request.Cookies["xuongViewType"];
                if (requestCookie != null)
                {
                    int.TryParse(requestCookie.Value, out xuongId);
                    if (xuongId > 0)
                        userResult.XuongId = xuongId;
                }
                else if(menuType == 0)
                {
                    //reset phòng kế hoạch
                    userResult.XuongId = 0;
                }    

                requestCookie = System.Web.HttpContext.Current.Request.Cookies["xuongViewName"];
                if (requestCookie != null)
                {
                    xuongName = HttpUtility.UrlDecode(requestCookie.Value.ToString()); ;
                    if(!string.IsNullOrEmpty(xuongName))
                        userResult.XuongName = xuongName;
                }

                //Store in cache,NoSlidingExpiration : timeout
                System.Web.HttpRuntime.Cache.Insert(cacheKey, userResult, null, DateTime.UtcNow.AddMinutes(_cacheTimeoutInMinutes), Cache.NoSlidingExpiration);
                var membershipUser = new TmsMembershipUser(userResult);

                return membershipUser;
            }

            return null;
        }

        #region Overrides of MembershipProvider

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override string ApplicationName { get; set; }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}