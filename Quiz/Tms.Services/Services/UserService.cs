using System.Collections.Generic;
using Tms.Models.User;
using Tms.DataAccess;
using System.Web;
using Tms.Models;
using System;
using System.Web.Caching;
using System.Linq;
using Tms.DataAccess.Repositories;
using Tms.DataAccess.Common;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

using Tms.Services.AutoMap;
using EcommerceSystem.Core;
using System.Globalization;

namespace Tms.Services
{
    public interface IUserService : IEntityService<User>
    {
        UserModel RetrieveUser(int userId);
        List<UserModel> SearchUser(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);
        bool DeleteUser(int id, out string message);
        UserCommon ValidateLogon(string userName, string password, out string msgError);
        bool IsUserExist(string userName);
        bool IsUserEmailExit(string email);
        List<string> GetRoleModuleAction(int userId, string[] roles);
        UserCommon GetUserByEmail(string email);
        List<UserCommon> GetUsersByRole(string roleCode);
        bool ChangePassword(int userId, string passwordOld, string passwordNew, out string message);
        bool ResetPassword(string email, string newPassord, out string message);
        bool ChangeStatus(int userId, out string message);
        bool UpdateUser(UserModel userModel, out string message);
        int CreateUser(UserModel userModel, out string message);
        bool CreateUser(RegisterModel model, out string message);
        List<string> GetUserEmailByRoleCode(string code);
        List<UserModel> GetAllUsers();
        List<UserModel> GetAllUsersByIds(List<int> ids);
        List<User> GetAllUserByListUser(List<string> listUser);
        UserModel GetMyInfo(int UserId);
        List<User> GetAllUserByRoleCodes(List<string> roleCodes);
        User GetByEmail(string email);
        bool CheckPass(string password, int userId);

    }
    public class UserService : EntityService<User>, IUserService
    {
        private readonly IModuleActionRepository _moduleActionRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICountryRepository _countryRepository;
        private const int CacheTimeoutInHours = 2;

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository, IUserRoleRepository userRoleRepository, IModuleActionRepository moduleActionRepository, IRoleRepository roleRepository, ICountryRepository countryRepository)
            : base(unitOfWork, userRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _moduleActionRepository = moduleActionRepository;
            _roleRepository = roleRepository;
            _countryRepository = countryRepository;
        }

        public UserModel RetrieveUser(int userId)
        {
            var userEntities = _userRepository.RetrieveUser(userId);
            if (userEntities != null)
            {
                return userEntities.MapToModel();
            }
            return null;
        }

        /// <summary>
        /// Search list user
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="textSearch"></param>
        /// <param name="totalPage"></param>
        /// <returns></returns>
        public List<UserModel> SearchUser(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage)
        {
            var userEntities = _userRepository.SearchUser(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalPage);
            if (userEntities != null)
            {
                return userEntities.MapToModels();
            }
            return null;
        }
        /// <summary>
        /// search list user by role code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<string> GetUserEmailByRoleCode(string code)
        {
            var users = _userRepository.FindAll(x => x.UserRoles.Any(z => z.Role.Code == code), x => x.UserRoles.Select(y => y.Role));
            return users?.Select(x => x.Email).ToList();
        }
        public bool DeleteUser(int id, out string message)
        {
            var user = _userRepository.GetById(id);
            if (user != null)
            {
                // Delete table User
                _userRepository.Delete(id);

                // Delete table UserRole
                var userRoleEntity = _userRoleRepository.RetrieveUserRole(id);
                _userRoleRepository.Delete(userRoleEntity);
                UnitOfWork.SaveChanges();

                message = "Xóa tài khoản thành công";
                return true;
            }

            message = "Xóa tài khoản thất bại";
            return false;
        }

        public bool CheckPass(string password, int userId)
        {
            var user = _userRepository.GetById(userId);
            if (password != null)
            {
                var passwordHasher = new CustomPasswordHasher();
                var newPass = string.Concat(user.PasswordSalt, password);

                var verify = passwordHasher.VerifyHashedPassword(user.Password, newPass);
                if (verify == PasswordVerificationResult.Success)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public UserCommon ValidateLogon(string email, string password, out string msgError)
        {
            msgError = string.Empty;
            var result = new UserCommon();

            var user = _userRepository.GetAll().Where(c => c.Email.Equals(email)).Include(c => c.UserRoles).FirstOrDefault();
            if (user == null)
            {
                msgError = "Thông tin Email không hợp lệ.";
                result.Status = LoginResult.InvalidEmail;
            }

            if (user != null)
            {
                var passwordHasher = new CustomPasswordHasher();
                var newPass = string.Concat(user.PasswordSalt, password);

                var verify = passwordHasher.VerifyHashedPassword(user.Password, newPass);
                if (verify == PasswordVerificationResult.Success)
                {
                    if (user.IsLockedOut)
                    {
                        msgError = "Tài khoản của bạn đã bị khóa, xin vui lòng liên hệ với người quản trị.";
                        result.Status = LoginResult.IsLockedOut;
                        return result;
                    }
                    var cacheKey = string.Format(Constant.CacheRoleModuleActionKey, email);
                    result.Status = LoginResult.Success;
                    result.Email = user.Email;
                    result.UserId = user.UserId;
                    result.UserName = user.UserName;
                    result.Avatar = user.Avatar;

                    var listIds = user.UserRoles.Select(c => c.RoleId).ToList();
                    if (listIds != null && listIds.Any())
                    {
                        var roleDefault = listIds.FirstOrDefault();
                        result.IsAdmin = user.IsSupperAdmin;
                        result.Roles = user.UserRoles.Select(c => c.RoleId.ToString()).ToArray();
                        if (System.Web.HttpRuntime.Cache[cacheKey] != null)
                            result.RoleModuleActions = (Dictionary<string, string>)System.Web.HttpRuntime.Cache[cacheKey];
                        else
                        {
                            result.RoleModuleActions = _moduleActionRepository.GetModuleActionByUserId(user.UserId, listIds);
                            //Store in cache,NoSlidingExpiration : timeout
                            HttpRuntime.Cache.Insert(cacheKey, result.RoleModuleActions, null, DateTime.Now.AddHours(CacheTimeoutInHours),
                                                     Cache.NoSlidingExpiration);
                        }
                    }
                    else
                    {
                        result.RoleModuleActions = new Dictionary<string, string>();
                    }
                    return result;
                }

                msgError = "Mật khẩu không hợp lệ.";
                result.Status = LoginResult.InvalidPassword;
                return result;
            }

            msgError = "Thông tin tài khoản không hợp lệ.";
            result.Status = LoginResult.Unknown;
            return result;
        }

        public bool IsUserExist(string userName)
        {
            var user = _userRepository.Query(x => x.UserName == userName).Any();
            return user;
        }

        public bool IsUserEmailExit(string email)
        {
            var user = _userRepository.Query(x => x.Email == email).Any();
            return user;
        }
        public List<string> GetRoleModuleAction(int userId, string[] roles)
        {
            return null;
        }

        public UserCommon GetUserByEmail(string email)
        {
            var result = new UserCommon();
            var user = GetAll().AsQueryable().Include(c => c.UserRoles)
                .FirstOrDefault(c => c.Email.ToLower().Equals(email.ToLower()) && (!c.IsLockedOut));

            if (user != null)
            {
                var listIds = (user.UserRoles != null) ? user.UserRoles.Select(c => c.RoleId).ToList() : new List<int>();
                var roleDefault = listIds.FirstOrDefault();
                result.IsAdmin = user.IsSupperAdmin;
                result.Roles = (user.UserRoles != null) ? user.UserRoles.Select(c => c.RoleId.ToString()).ToArray() : new string[0];

                result.UserId = user.UserId;
                result.Avatar = user.Avatar;
                result.Email = user.Email;
                result.UserName = user.UserName;

                var cacheKey = string.Format(Constant.CacheRoleModuleActionKey, email);
                if (System.Web.HttpRuntime.Cache[cacheKey] != null)
                    result.RoleModuleActions = (Dictionary<string, string>)System.Web.HttpRuntime.Cache[cacheKey];
                else
                {
                    result.RoleModuleActions = _moduleActionRepository.GetModuleActionByUserId(user.UserId, listIds);
                    //Store in cache,NoSlidingExpiration : timeout
                    HttpRuntime.Cache.Insert(cacheKey, result.RoleModuleActions, null, DateTime.Now.AddHours(CacheTimeoutInHours),
                                             Cache.NoSlidingExpiration);
                }
            }

            return result;
        }

        public List<UserCommon> GetUsersByRole(string roleCode)
        {
            var users = GetAll().AsQueryable().Include(c => c.UserRoles).Include(x => x.UserRoles.Select(y => y.Role))
                            .Where(o => o.UserRoles.Any(r => r.Role.Code == roleCode));

            var listUsers = new List<UserCommon>();
            if (users.Any())
            {
                var listIds = users.Select(x => new UserCommon { Email = x.Email, FirstName = x.FullName, UserId = x.UserId }).ToList();
                listUsers.AddRange(listIds);
            }

            return listUsers;
        }

        public bool ChangePassword(int userId, string passwordOld, string passwordNew, out string message)
        {
            var user = _userRepository.Find(c => c.UserId == userId);
            if (user != null)
            {
                var passwordHasher = new CustomPasswordHasher();
                var oldPass = string.Concat(user.PasswordSalt, passwordOld);

                var verify = passwordHasher.VerifyHashedPassword(user.Password, oldPass);
                if (verify == PasswordVerificationResult.Success)
                {
                    var newPass = string.Concat(user.PasswordSalt, passwordNew);
                    user.Password = (new CustomPasswordHasher()).HashPassword(newPass);
                    _userRepository.Update(user);

                    UnitOfWork.SaveChanges();

                    message = "Thay đổi mật khẩu thành công.";
                    return true;
                }
                message = "Mật khẩu cũ không đúng, vui lòng nhập lại.";
                return false;
            }

            message = "Thay đổi mật khẩu thất bại.";
            return false;
        }

        public bool ResetPassword(string email, string newPassord, out string message)
        {
            var user = _userRepository.Find(c => c.Email == email);
            if (user == null)
            {
                message = "Tài khoản không tồn tại, xin vui lòng nhập lại.";
                return false;
            }
            //var encryptedNewPassord = Security.SecurityUtil.EncryptText(newPassord);
            var newPass = string.Concat(user.PasswordSalt, newPassord);
            user.Password = (new CustomPasswordHasher()).HashPassword(newPass);
            _userRepository.Update(user);

            UnitOfWork.SaveChanges();

            message = "Reset mật khẩu thành công. Xin vui lòng kiểm tra email để cập nhật.";
            return true;
        }


        public bool ChangeStatus(int userId, out string message)
        {
            var position = _userRepository.Query(c => c.UserId == userId).FirstOrDefault();
            if (position != null)
            {
                if (position.IsLockedOut)
                {
                    position.IsLockedOut = false;
                }
                else
                {
                    position.IsLockedOut = true;
                }

                _userRepository.Update(position);
                UnitOfWork.SaveChanges();

                message = "Cập nhật trạng thái thành công.";
                return true;
            }

            message = "Cập nhật trạng thái thất bại.";
            return false;
        }

        public bool UpdateUser(UserModel userModel, out string message)
        {
            var userEntity = _userRepository.GetById(userModel.UserId);
            var userRoleEntity = _userRoleRepository.RetrieveUserRole(userModel.UserId);
            if (userEntity != null)
            {
                // Update table user
                if (IsUpdatedUser(userModel))
                {
                    userEntity.Email = userModel.Email;
                    userEntity.UpdatedDate = DateTime.Now;
                    userEntity.UserName = ReplaceUnicode(userModel.UserName.ToUpper());
                    userEntity.FullName = ReplaceUnicode(userModel.UserName.ToUpper());
                    userEntity.ChineseName = userModel.ChineseName;
                    userEntity.CountryCode = userModel.CountryCode.ToString();
                    userEntity.PlaceOfBirth = userModel.PlaceOfBirth;
                    userEntity.Gender = userModel.Gender;
                    userEntity.IdentityCardNo = userModel.IdentityCardNo;
                    var userModelContry = _countryRepository.GetById(userModel.CountryCode);
                    userEntity.Address = userModelContry.CountryName.ToUpper();
                    userEntity.DateOfBirth = DateTime.Parse(userModel.BirthdayStr, CultureInfo.GetCultureInfo("vi-VN")).Date.Add(DateTime.Now.TimeOfDay);
                    userEntity.Avatar = userModel.Avatar;
                    userEntity.Tel = userModel.Tel;
                    if (userEntity.Password != userModel.Password && userModel.IsChangePassword)
                    {
                        if (!string.IsNullOrEmpty(userModel.Password))
                        {
                            var newPass = string.Concat(userEntity.PasswordSalt, userModel.Password);
                            userEntity.Password = (new CustomPasswordHasher()).HashPassword(newPass);
                        }
                    }
                    _userRepository.Update(userEntity);
                }

                //Update table UserRole
                if (userRoleEntity.RoleId != userModel.RoleId)
                {
                    userRoleEntity.RoleId = userModel.RoleId;
                    _userRoleRepository.Update(userRoleEntity);
                };


                UnitOfWork.SaveChanges();

                message = "Cập nhật thành công";
                return true;
            }
            message = "Cập nhật tài khoản thất bại.";
            return false;
        }

        public int CreateUser(UserModel userModel, out string message)
        {
            if (!IsUserEmailExit(userModel.Email))
            {
                // Add table User
                userModel.PasswordSalt = (new CustomPasswordHasher()).HashPassword(userModel.Password);
                var password = string.Concat(userModel.PasswordSalt, userModel.Password);
                userModel.Password = (new CustomPasswordHasher()).HashPassword(password);

                userModel.CreatedDate = DateTime.Now;
                userModel.Status = true;
                var userEntity = _userRepository.Insert(userModel.MapToEntity());
                UnitOfWork.SaveChanges();

                // Add table UserRole
                var userRoleEntity = new UserRole();
                userRoleEntity.UserId = userEntity.UserId;
                userRoleEntity.RoleId = userModel.RoleId;
                userRoleEntity.CreatedDate = DateTime.Now;
                _userRoleRepository.Insert(userRoleEntity);
                UnitOfWork.SaveChanges();

                // Return
                message = "Thêm tài khoản thành công";
                return userEntity.UserId;
            }
            message = "Email đã tồn tại";
            return 0;
        }

        public bool CreateUser(RegisterModel model, out string message)
        {
            UserModel userModel = new UserModel();
            if (!IsUserEmailExit(model.Email))
            {
                // Add table User
                //userModel.UserName = model.FirstName.ToUpper() + " " + model.LastName.ToUpper();
                userModel.UserName = ReplaceUnicode(model.UserName);
                userModel.FullName = ReplaceUnicode(userModel.UserName.ToUpper());
                userModel.Email = model.Email;
                userModel.Tel = model.Tel;
                if (model.CountryCode != null)
                {
                    userModel.CountryCode = model.CountryCode;
                }
                else
                {
                    userModel.CountryCode = 243;
                }
                userModel.Level = Int32.Parse(model.Level);
                userModel.IdentityCardNo = model.IdentityCardNo;
                userModel.Gender = model.Gender;
                userModel.IdentityCardNo = model.IdentityCardNo;
                userModel.ChineseName = model.ChineseName;
                userModel.Birthday =  DateTime.Parse(model.BirthdayValue, CultureInfo.GetCultureInfo("vi-VN")).Date.Add(DateTime.Now.TimeOfDay); 
                userModel.PlaceOfBirth = model.PlaceOfBirth;
                var entityContry = _countryRepository.GetById(model.CountryCode);
                if (entityContry != null)
                {
                    userModel.Address = ReplaceUnicode(entityContry.CountryName).ToUpper();
                }
                else
                {
                    userModel.Address = "VIET NAM";
                }
                userModel.PasswordSalt = (new CustomPasswordHasher()).HashPassword(model.Password);
                var password = string.Concat(userModel.PasswordSalt, model.Password);
                userModel.Password = (new CustomPasswordHasher()).HashPassword(password);
                userModel.CreatedDate = DateTime.Now;
                userModel.Status = true;
                userModel.Avatar = model.Avatar;
                var userEntity = _userRepository.Insert(userModel.MapToEntity());
                UnitOfWork.SaveChanges();

                // Add table UserRole
                var userRoleEntity = new UserRole();
                userRoleEntity.UserId = userEntity.UserId;
                userRoleEntity.RoleId = 7;
                userRoleEntity.CreatedDate = DateTime.Now;
                _userRoleRepository.Insert(userRoleEntity);
                UnitOfWork.SaveChanges();

                // Return
                message = "Thêm tài khoản thành công";
                return true;
            }
            message = "Email đã tồn tại";
            return false;
        }


        /// <summary>
        /// Check user is updated
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        private bool IsUpdatedUser(UserModel userModel)
        {
            var userEntity = _userRepository.GetById(userModel.UserId);
            if (userEntity.Email != userModel.Email)
                return true;
            if (userEntity.UserName != userModel.UserName)
                return true;
            if (userEntity.Password != userModel.Password)
                return true;
            if (userEntity.Avatar != userModel.Avatar)
                return true;
            if (userEntity.Tel != userModel.Tel)
                return true;
            return false;
        }

        public List<UserModel> GetAllUsers()
        {
            return _userRepository.GetAll().Take(50).ToList().MapToModels();
        }
        public List<User> GetAllUserByListUser(List<string> listUser)
        {
            return _userRepository.GetAllUserByListUser(listUser);
        }
        public UserModel GetMyInfo(int UserId)
        {
            var user = GetAll().AsQueryable().Include(c => c.UserRoles).Include(x => x.UserRoles.Select(y => y.Role)).Where(c => c.UserId == UserId);
            return user.FirstOrDefault().MapToModel();
        }
        public User GetByEmail(string email)
        {
            var user = GetAll().AsQueryable().Where(c => c.Email == email);
            return user.FirstOrDefault();
        }

        public List<UserModel> GetAllUsersByIds(List<int> ids)
        {
            return _userRepository.GetAll().Where(c => ids.Contains(c.UserId)).ToList().MapToModels();
        }

        public List<User> GetAllUserByRoleCodes(List<string> roleCodes)
        {
            try
            {
                var entities = _userRepository.GetAllUserByRoleCodes(roleCodes);
                if (entities != null)
                {
                    return entities;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        private static string[] VietNamChar = new string[]
       {
           "aAeEoOuUiIdDyY",
           "áàạảãâấầậẩẫăắằặẳẵ",
           "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
           "éèẹẻẽêếềệểễ",
           "ÉÈẸẺẼÊẾỀỆỂỄ",
           "óòọỏõôốồộổỗơớờợởỡ",
           "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
           "úùụủũưứừựửữ",
           "ÚÙỤỦŨƯỨỪỰỬỮ",
           "íìịỉĩ",
           "ÍÌỊỈĨ",
           "đ",
           "Đ",
           "ýỳỵỷỹ",
           "ÝỲỴỶỸ"
       };
        public static string ReplaceUnicode(string strInput)
        {
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                {
                    strInput = strInput.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
                }
            }
            return strInput;
        }


    }
}
