using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.Repositories;
using Tms.Models.UserTestQuestionAnswerModel;
using Tms.Services.AutoMap;
using System;
using System.Collections.Generic;
using System.Linq;
using Tms.Models.UserTestModel;
using EcommerceSystem.Core.Configurations;
using System.Text.RegularExpressions;

namespace Tms.Services
{
    public interface IUserTestQuestionAnswerService : IEntityService<UserTestQuestionAnswer>
    {
        List<UserTestQuestionAnswerModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);
        bool UpdateUserTestQuestionAnswer(UserTestQuestionAnswerModel UserTestQuestionAnswerModel, out string message);
        bool Delete(int userTestQuestionAnswerId, out string message);
        List<UserTestQuestionAnswerModel> GetAllUserTestQuestionAnswers();
        bool CreateUserTestQuestionAnswer(UserTestQuestionAnswerModel model);
        bool ChangeStatus(int userTestQuestionAnswerId, out string message);

        bool MarkExamTest(List<UserTestQuestionAnswerModel> UserTestQuestionAnswers, int userTestId);

        DegreeModel Degree(int userTestId);

        List<UserTestQuestionAnswerModel> GetAllUserTestQuestionAnswersById(int id);

    }
    public class UserTestQuestionAnswerService : EntityService<UserTestQuestionAnswer>, IUserTestQuestionAnswerService
    {
        private readonly IUserTestQuestionAnswerRepository _userTestQuestionAnswerRepository;
        private readonly IUserTestRepository _userTestRepository;
        private readonly IUserTestQuestionRepository _userTestQuestionRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        public UserTestQuestionAnswerService(IUnitOfWork unitOfWork, IUserTestQuestionAnswerRepository userTestQuestionAnswerRepository, IQuestionRepository questionRepository, IUserRepository userRepository, IUserTestRepository userTestRepository, IUserTestQuestionRepository userTestQuestionRepository)
            : base(unitOfWork, userTestQuestionAnswerRepository)
        {
            _userTestQuestionAnswerRepository = userTestQuestionAnswerRepository;
            _questionRepository = questionRepository;
            _userRepository = userRepository;
            _userTestRepository = userTestRepository;
            _userTestQuestionRepository = userTestQuestionRepository;
        }

        public List<UserTestQuestionAnswerModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage)
        {
            try
            {
                var userTestQuestionAnswerEntities = _userTestQuestionAnswerRepository.Search(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalPage);
                if (userTestQuestionAnswerEntities != null)
                {
                    return userTestQuestionAnswerEntities.MapToModels();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Search UserTestQuestionAnswer error", ex);
            }
            totalPage = 0;
            return null;
        }

        public bool UpdateUserTestQuestionAnswer(UserTestQuestionAnswerModel userTestQuestionAnswerModel, out string message)
        {
            try
            {
                var userTestQuestionAnswerEntity = _userTestQuestionAnswerRepository.GetById(userTestQuestionAnswerModel.UserTestQuestionAnswerID);
                if (userTestQuestionAnswerEntity != null)
                {
                    userTestQuestionAnswerEntity = userTestQuestionAnswerModel.MapToEntity(userTestQuestionAnswerEntity);

                    _userTestQuestionAnswerRepository.Update(userTestQuestionAnswerEntity);
                    UnitOfWork.SaveChanges();

                    message = "Cập nhật thành công";
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update UserTestQuestionAnswer error", ex);
            }
            message = "Cập nhật bản ghi thất bại.";
            return false;
        }

        public bool CreateUserTestQuestionAnswer(UserTestQuestionAnswerModel model)
        {
            try
            {
                var createdUserTestQuestionAnswer = _userTestQuestionAnswerRepository.Insert(model.MapToEntity());
                UnitOfWork.SaveChanges();
                var errorMsg = string.Empty;
                if (createdUserTestQuestionAnswer == null)
                {
                    Log.Error("Create userTestQuestionAnswer error");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Create UserTestQuestionAnswer error", ex);
                return false;
            }

        }

        public bool Delete(int userTestQuestionAnswerId, out string message)
        {


            try
            {
                var entity = _userTestQuestionAnswerRepository.GetById(userTestQuestionAnswerId);
                if (entity != null)
                {
                    _userTestQuestionAnswerRepository.Delete(userTestQuestionAnswerId);
                    UnitOfWork.SaveChanges();

                    message = "Xóa bản ghi thành công";
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Delete UserTestQuestionAnswer error", ex);
            }

            message = "Xóa bản ghi thất bại";
            return false;
        }

        public List<UserTestQuestionAnswerModel> GetAllUserTestQuestionAnswers()
        {
            //Igrone userTestQuestionAnswer system
            return _userTestQuestionAnswerRepository.GetAll().ToList().MapToModels();
        }

        public bool ChangeStatus(int userTestQuestionAnswerId, out string message)
        {
            try
            {
                var entity = _userTestQuestionAnswerRepository.Query(c => c.UserTestQuestionAnswerID == userTestQuestionAnswerId).FirstOrDefault();
                if (entity != null)
                {
                    if (entity.Status)
                    {
                        entity.Status = false;
                    }
                    else
                    {
                        entity.Status = true;
                    }

                    _userTestQuestionAnswerRepository.Update(entity);
                    UnitOfWork.SaveChanges();

                    message = "Cập nhật trạng thái thành công.";
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Delete UserTestQuestionAnswer error", ex);
            }

            message = "Cập nhật trạng thái thất bại.";
            return false;
        }

        public List<UserTestQuestionAnswerModel> GetAllUserTestQuestionAnswersById(int id)
        {
            try
            {
                var entities = _userTestQuestionAnswerRepository.GetByUserTestId(id);
                if (entities != null && entities.Count() > 0)
                {
                    var modelEntities = entities.MapToModels();
                    return modelEntities;
                }
                return null;

            }
            catch (Exception ex)
            {
                Log.Error("UserTestQuestionAnswer error", ex);
            }
            return null;
        }

        public bool MarkExamTest(List<UserTestQuestionAnswerModel> UserTestQuestionAnswers,int userTestId)
        {
            try
            {
                //update question point
                var entities = _userTestQuestionAnswerRepository.GetAll();
                foreach (var item in UserTestQuestionAnswers)
                {
                    double point = 0;
                    var entityModel = entities.Where(c => c.UserTestQuestionAnswerID == item.UserTestQuestionAnswerID).FirstOrDefault();
                    if (item.Point != null)
                    {
                        point = (double)item.Point;
                    }
                    entityModel.Point = point;
                    var updateEntity = _userTestQuestionAnswerRepository.Update(entityModel);
                    UnitOfWork.SaveChanges();
                }

                //update total point exam
                var entityListQuestion = _userTestQuestionRepository.GetByUserTestId(userTestId);
                var contestGroup = entityListQuestion.GroupBy(c => c.ContestID);
                double totalPoint = 0;
                foreach (var item in contestGroup)
                {
                    var point = Math.Round((double)item.Select(c => c.UserTestQuestionAnswers.Select(g => g.Point).Sum()).Sum());
                    totalPoint += point;
                }

                //get userTest for update
                var userTestEntity = _userTestRepository.GetById(userTestId);
                userTestEntity.TotalPoint = totalPoint;
                _userTestRepository.Update(userTestEntity);
                UnitOfWork.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("UserTestQuestionAnswer error", ex);
            }
            return false;
        }
        
        public static String StripUnicodeCharactersFromString(string inputValue)
        {
            return Regex.Replace(inputValue, @"[^\u0000-\u007F]", String.Empty);
        }

        public DegreeModel Degree(int userTestId)
        {
            try
            {
                double? totalListening = 0;
                double? totalReading = 0;
                double? totalWriting = 0;
                double? totalTranslation = 0;
                var entities = _userTestQuestionAnswerRepository.GetByTestId(userTestId);

                if (entities != null && entities.Any())
                {
                    var userEntity = _userRepository.GetById((int)entities.FirstOrDefault().UserTestQuestion.UserTest.UserId);
                    var model = new DegreeModel();
                    if (userEntity != null)
                    {
                        model.User = userEntity.MapToModel();
                        if (entities.FirstOrDefault().Type != 6)
                        {
                            foreach (var item in entities)
                            {
                                //nghe
                                if (item.Type == 1 && item.Point.HasValue)
                                {
                                    totalListening += item.Point.Value;
                                }
                                //đọc
                                if (item.Type == 2 && item.Point.HasValue)
                                {
                                    totalReading += item.Point.Value;
                                }
                                //viết
                                if ((item.Type == 3 && item.Point.HasValue) || (item.Type != 4 && item.Type != 1 && item.Type != 2))
                                {
                                    totalWriting += item.Point.Value;
                                }
                                if (item.Type == 4 && item.Point.HasValue)
                                {
                                    totalTranslation += item.Point.Value;
                                }
                            }
                        }
                        else
                        {
                            foreach (var item in entities)
                            {
                                totalListening += (double)item.Point.Value;
                            }
                        }
                    }
                    model.NameHSK = entities.FirstOrDefault().UserTestQuestion.UserTest.Quiz.QuizName;
                    model.User.Male = userEntity.Gender == 1 ? "MALE" : "FEMALE";
                    var imagePath = SystemConfiguration.GetStringKey("UserAvatarPath");
                    var imagePathThumbnail = "/Account/";
                    if (!string.IsNullOrEmpty(model.User.Avatar))
                    {
                        model.User.Avatar = imagePath + userEntity.Avatar;
                        model.User.Thumbnail = imagePathThumbnail + userEntity.Avatar;
                    }
                    model.User.Birthday = (DateTime)userEntity.DateOfBirth;
                    model.DateExam = entities.FirstOrDefault().UserTestQuestion.UserTest.CreatedDate;
                    model.TypeHSK = (int)entities.FirstOrDefault().UserTestQuestion.UserTest.Quiz.Category.Type;
                    var entityUsertest = _userTestRepository.GetQuizById(userTestId);
                    if (entities != null)
                    {
                        //lấy mã
                        var preFix = entityUsertest.Quiz.Category.PreFix;
                        var dateNumber = entityUsertest.CreatedDate.ToString("dd/MM");
                        var arrayDateTime = dateNumber.Split('/');
                        var stringDateTime = "";
                        for (int i = 0; i < arrayDateTime.Count(); i++)
                        {
                            stringDateTime += arrayDateTime[i];
                        }
                        var countDegree = _userTestRepository.GetAll().Where(c => c.Status == true).Count();
                        var degreeCode = (countDegree + 1).ToString().PadLeft(6, '0');
                        var codeDegree = preFix + stringDateTime + degreeCode;
                        //end lấy mã
                        entityUsertest.Title = codeDegree;
                    }
                    model.User.EmployerCode = entityUsertest.Title;
                    model.CertificateFont = entities.FirstOrDefault().UserTestQuestion.UserTest.Quiz.Category.CertificateImageFont;
                    model.CertificateBack = entities.FirstOrDefault().UserTestQuestion.UserTest.Quiz.Category.CertificateImageBack;
                    model.User.FullName = string.IsNullOrEmpty(userEntity.FullName) ? "" : StripUnicodeCharactersFromString(userEntity.FullName);
                    model.User.Nationality = userEntity.Address;
                    model.totalListening =  Math.Round((double)totalListening);
                    model.totalReading = Math.Round((double)totalReading);
                    model.totalWriting = Math.Round((double)totalWriting);
                    model.totalTranslation = Math.Round((double)totalTranslation);
                    return model;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Error("UserTestQuestionAnswer error", ex);
            }
            return null;
        }
    }
}
