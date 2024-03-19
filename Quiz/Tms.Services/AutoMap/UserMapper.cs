using Tms.DataAccess;
using Tms.Models;
using Tms.Models.User;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Tms.Services.AutoMap
{
    public static class UserMapper
    {
        #region Mapping User
        public static User MapToEntity(this UserModel model)
        {
            var user = new User();
            user.Email = model.Email;
            user.Password = model.Password;
            user.PasswordSalt = model.PasswordSalt;
            user.UserName = model.UserName;
            user.CreatedDate = model.CreatedDate;
            user.IsLockedOut = model.IsLockedOut;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.FullName = model.FullName;
            user.IsSupperAdmin = model.IsSupperAdmin;
            user.Status = model.Status;
            user.IdentityCardNo = model.IdentityCardNo;
            user.PlaceOfBirth = model.PlaceOfBirth;
            user.DateOfBirth = model.Birthday;
            user.Address = model.Address;
            user.Level = model.Level;
            user.CountryCode = model.CountryCode.ToString();
            user.Gender = model.Gender;
            user.Avatar = model.Avatar;
            user.ChineseName = model.ChineseName;
            user.PasswordSalt = model.PasswordSalt;
            user.Tel = model.Tel;
            return user;
        }
        public static User MapToEntity(this UserModel model, User entity)
        {
            entity.Email = model.Email;
            entity.Password = model.Password;
            return entity;
        }
        public static UserModel MapToModel(this User entity)
        {
            var levelName = "";
            switch (entity.Level)
            {
                case 1:
                    levelName = "HSK1";
                    break;
                case 2:
                    levelName = "HSK2";
                    break;
                case 3:
                    levelName = "HSK3";
                    break;
                case 4:
                    levelName = "HSK4";
                    break;
                case 5:
                    levelName = "HSK5";
                    break;
                case 6:
                    levelName = "HSK6";
                    break;
                case 7:
                    levelName = "HSK7-9";
                    break;
            }
            var user = new UserModel();
            user.UserId = entity.UserId;
            user.UserName = entity.UserName;
            user.Email = entity.Email;
            user.CreatedDate = entity.CreatedDate;
            user.Status = entity.Status;
            user.Password = entity.Password;
            user.RoleId = entity.UserRoles.Select(x => x.RoleId).FirstOrDefault();
            user.RoleName = entity.UserRoles.Select(x => x.Role.Name).FirstOrDefault();
            user.Avatar = entity.Avatar;
            user.Tel = entity.Tel;
            user.LevelName = levelName;
            user.Gender = entity.Gender.HasValue ? entity.Gender.Value: 0;
            user.Level = entity.Level.HasValue ? entity.Level.Value : 0;
            user.PlaceOfBirth = entity.PlaceOfBirth;
            user.IdentityCardNo = entity.IdentityCardNo;
            user.Birthday = entity.DateOfBirth;
            user.ChineseName = entity.ChineseName;
            user.CountryCode = !string.IsNullOrEmpty(entity.CountryCode) ? Int32.Parse(entity.CountryCode) : 0; 
            user.IsLockedOut = entity.IsLockedOut;
            return user;
        }
        public static List<User> MapToEntities(this List<UserModel> models)
        {
            return models.Select(x => x.MapToEntity()).ToList();
        }

        public static List<UserModel> MapToModels(this List<User> entities)
        {
            return entities.Select(x => x.MapToModel()).ToList();
        }
        #endregion

        #region Configuration
        public static ConfigurationModel MaptoModel(this Configuration entity)
        {
            return new ConfigurationModel
            {
                AdminAcceptanceBalancePercentage = entity.AdminAcceptanceBalancePercentage,
                ClientAcceptanceBalancePercentage = entity.ClientAcceptanceBalancePercentage,
                ExchangeRate = entity.ExchangeRate ?? 0
            };
        }
        #endregion

    }
}
