using Tms.Models;
using Tms.Models.User;
using Tms.Services;
using System;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Linq;
using Tms.Web.Framework.Authentication;
using Tms.Web.Framework.Helpers;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using EcommerceSystem.Core.Configurations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using EcommerceSystem.Core;
using System.Collections.Generic;
using Tms.Services.Services;
using System.Globalization;

namespace Tms.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly IContextService _contextService;
        private readonly IMailService _mailService;
        private readonly ICountryService _countryService;
        private readonly IUserTestQuestionAnswerService _userTestQuestionAnswerService;

        const string EMAIL_CONFIRMATION = "EmailConfirmation";
        const string PASSWORD_RESET = "ResetPassword";

        public AccountController(IUserService userService, IContextService contextService, IRoleService roleService, IMailService mailService, ICountryService countryService, IUserTestQuestionAnswerService userTestQuestionAnswerService)
        {
            _roleService = roleService;
            _userService = userService;
            _contextService = contextService;
            _mailService = mailService;
            _countryService = countryService;
            _userTestQuestionAnswerService = userTestQuestionAnswerService;
        }


        //GET: /Acccount/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            List<SelectListItem> levelHSK = new List<SelectListItem>();
            levelHSK.Add(new SelectListItem() { Value = "1", Text = "HSK1" });
            levelHSK.Add(new SelectListItem() { Value = "2", Text = "HSK2" });
            levelHSK.Add(new SelectListItem() { Value = "3", Text = "HSK3" });
            levelHSK.Add(new SelectListItem() { Value = "4", Text = "HSK4" });
            levelHSK.Add(new SelectListItem() { Value = "5", Text = "HSK5" });
            levelHSK.Add(new SelectListItem() { Value = "6", Text = "HSK6" });
            levelHSK.Add(new SelectListItem() { Value = "7", Text = "HSK7-9" });
            ViewBag.LevelHSK = levelHSK.ToList();
            List<SelectListItem> typePlaceOfBirth = new List<SelectListItem>();
            typePlaceOfBirth.Add(new SelectListItem() { Value = "1", Text = "Hộ chiếu" });
            typePlaceOfBirth.Add(new SelectListItem() { Value = "2", Text = "Chứng minh thư" });
            typePlaceOfBirth.Add(new SelectListItem() { Value = "3", Text = "Căn cước công dân", Selected = true });
            ViewBag.TypePlaceOfBirth = typePlaceOfBirth;
            var countryModel = _countryService.GetAllCountry();
            //ViewBag.Country = countryModel.Select(c => new SelectListItem{ Value = c.CountryID.ToString(), Text = c.CountryName, Selected = c.ISO2.ToUpper() == "VN" ? true : false }).ToList();
            return View();
        }


        public string FileUpload(HttpPostedFileBase file)
        {
            string _path = "";
            var fileTail = file.FileName.Split('.');
            string _FileName = "";
            if (file.ContentLength > 0)
            {
                 _FileName = Path.GetFileName(file.FileName);
                _path = Path.Combine("/Account/", _FileName);
                var filePath = Path.Combine(Server.MapPath("~/Account/"), _FileName);
                file.SaveAs(filePath);
            }
            return _FileName;
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterModel model, HttpPostedFileBase ImageFile)
        {
            string message;
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    model.Avatar = FileUpload(ImageFile);
                }
                var userCreate = _userService.CreateUser(model, out message);
                if (userCreate)
                {
                    TempData["success"] = message;
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Groups = _roleService.GetRoles();
                TempData["error"] = message;
                return RedirectToAction("Register", "Account");
            }

            ViewBag.Groups = _roleService.GetRoles();
            return View(model);
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
       
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserLoginModel model, string returnUrl)
        {
            var msgError = "";
            if (ModelState.IsValid)
            {
                var cacheKey = string.Format(Constant.CacheRoleModuleActionKey, model.Email);
                HttpRuntime.Cache.Remove(cacheKey);
                HttpRuntime.Cache.Remove(string.Format(Constant.CacheUserLoginKey, model.Email));
                var user = _userService.ValidateLogon(model.Email, model.Password, out msgError);
                if (user.Status == LoginResult.Success)
                {
                    var userData = JsonConvert.SerializeObject(user);
                    var authTicket = new FormsAuthenticationTicket(1,
                        user.Email,
                        DateTime.Now,
                        DateTime.Now.AddDays(4),
                        model.Remember,
                        userData);

                    var encTicket = FormsAuthentication.Encrypt(authTicket);
                    var cookieKey = FormsAuthentication.FormsCookieName;

                    _contextService.SaveInCookie(cookieKey, encTicket, 4);
                    FormsAuthentication.RedirectFromLoginPage(model.Email, false);

                    //if (encTicket != null)
                    //    HttpRuntime.Cache.Insert(cookieKey, userData, null, DateTime.Now.AddHours(30),
                    //        Cache.NoSlidingExpiration);

                    if (!string.IsNullOrEmpty(returnUrl) && returnUrl.Length > 1)
                        Response.Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }
            }

            if (!string.IsNullOrEmpty(msgError))
            {
                ViewBag.MessageError = msgError;
            }

            return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut(); //you write this when you use FormsAuthentication
            return RedirectToAction("Login", "Account");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPassworsModel model)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                string newPass = Guid.NewGuid().ToString("n").Replace("-", "").Substring(0, 6);
                var response = _userService.ResetPassword(model.Email, newPass, out message);
                if (response)
                {
                    string emailTemplate = @"<div style='font-family:Arial'>
	                                    <div style='margin:0px;padding:10px;background:#cccccc;font-family:Arial;font-size:12px'>
		                                    <div style='border-radius:6px;width:598px;margin:0px auto;padding:15px 0px;background:#ffffff;color:#4d4d4d'>
			                                    <div style='width:546px;margin:0px auto;border:4px solid #007cc2'>
				                                    <div style='padding:10px 20px'>
					                                    <p style='padding:4px 0px 10px 0px;margin:0px;font-family:Arial'>Kính chào <b>{0},</b></p>
					                                    <div style='margin-bottom:0px'>
						                                    <p style='font-family:Arial'>QTEdu đã reset lại mật khẩu của bạn trên website <a href='https://qtedu.vn/' target='_blank'>qtedu.vn</a></p>
						                                    <p style='font-family:Arial'>Mật khẩu mới của bạn là: <b>{1}</b></p>
						                                    <p style='font-family:Arial'>Xin vui lòng đăng nhập vào hệ thống bằng mật khẩu này!</p>
					                                    </div>
					                                    <div style='font-weight:bold;margin-bottom:2px;font-size:11px'>
						                                    
					                                    </div>
				                                    </div>
			                                    </div>
			                                    <div style='color:#555555;width:554px;margin:5px auto;padding:5px 0px'>
				                                    <div style='padding-top:15px;font-family:Arial' align='center'><b>Cảm ơn bạn đã tham gia vào hệ thống của chúng tôi.</b></div>
			                                    </div>
		                                    </div>
	                                    </div>
                                    </div>";
                    var bodyContent = string.Format(emailTemplate, model.Email, newPass);
                    response = _mailService.SendMail(model.Email, "[qtedu.vn] Reset mật khẩu", bodyContent, out message);
                    if(response)
                        TempData["success"] = message;
                    else
                        TempData["error"] = message;
                    return View();
                }
            }

            TempData["error"] = "Có lỗi xảy ra, xin vui lòng kiểm tra lại dữ liệu.";
            return View();

        }

        public ActionResult ResetPassword(string userID, string code)
        {
            ViewBag.PasswordToken = code;
            ViewBag.UserID = userID;
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var userStore = new UserStore<IdentityUser>();
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userStore);
            var user = manager.FindByEmail(model.Email);
            CreateTokenProvider(manager, PASSWORD_RESET);

            IdentityResult result = manager.ResetPassword(model.Email, model.PasswordToken, model.Password);
            if (result.Succeeded)
                ViewBag.Result = "The password has been reset.";
            else
                ViewBag.Result = "The password has not been reset.";
            return View();
        }

        [CustomAuthorize(Roles = RoleHelper.QuanTriDisplay)]
        public ActionResult Index()
        {
            int totalRecords;
            var searchModel = new UserSearchModel()
            {
                Users = _userService.SearchUser(1, SystemConfiguration.PageSizeDefault, null, null, null, out totalRecords),
                PageIndex = 1,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords
            };
            return View(searchModel);
        }

        public ActionResult DanhMucNhanVien()
        {
            int totalRecords;
            var searchModel = new UserSearchModel()
            {
                Users = _userService.SearchUser(1, SystemConfiguration.PageSizeDefault, null, null, null, out totalRecords),
                PageIndex = 1,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords
            };
            return View(searchModel);
        }

        public ActionResult Search(int currentPage, string textSearch, string sortColumn, string sortDirection, int pageSize, string viewtype)
        {
            int totalRecords;
            var model = new UserSearchModel
            {
                Users = _userService.SearchUser(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalRecords),
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageIndex = currentPage,
                PageSize = pageSize,
                TotalRecords = totalRecords,
            };
            var html = "";
            if (viewtype == "nhanvien")
            {
                html = RenderPartialViewToString("~/Views/Account/Partial/_DanhMucNhanVienListItems.cshtml", model);
            }
            else
            {
                html = RenderPartialViewToString("~/Views/Account/Partial/_TableUser.cshtml", model);
            }
            
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchUser(int currentPage, string textSearch, string sortColumn, string sortDirection, int pageSize, string viewtype)
        {
            int totalRecords;
            var model = new UserSearchModel
            {
                Users = _userService.SearchUser(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalRecords),
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageIndex = currentPage,
                PageSize = pageSize,
                TotalRecords = totalRecords,
            };
            var html = RenderPartialViewToString("~/Views/Account/Partial/_TableUser.cshtml", model);

            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search_NhanVien(int currentPage, string textSearch, int pageSize)
        {
            int totalRecords;
            var model = new UserSearchModel
            {
                Users = _userService.SearchUser(currentPage, pageSize, textSearch, null, null, out totalRecords),
                PageIndex = currentPage,
                PageSize = pageSize,
                TotalRecords = totalRecords,
            };

            var html = RenderPartialViewToString("~/Views/Account/Partial/_DanhMucNhanVienListItems.cshtml", model);
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }


        // Profile User
        [HttpGet]
        public ActionResult UserInfo(int? userId = null)
        {
            bool isViewOnly = userId.HasValue;
            userId = isViewOnly ? userId.Value : CurrentUser.UserId;
            var model = _userService.GetMyInfo(userId.Value);
            if (!string.IsNullOrEmpty(model.Avatar))
            {
                var stringAvata = "/Account/" + model.Avatar;
                model.Avatar = stringAvata;
            }
            if (model.Birthday.HasValue)
            {
                model.BirthdayStr = model.Birthday.Value.ToString("dd/MM/yyyy");
            }
            ViewBag.Male = model.Gender;
            model.IsPhuTrachDonVi = isViewOnly;
            List<SelectListItem> levelHSK = new List<SelectListItem>();
            levelHSK.Add(new SelectListItem() { Value = "1", Text = "HSK1", Selected = model != null && model.Level == 1 ? true : false });
            levelHSK.Add(new SelectListItem() { Value = "2", Text = "HSK2", Selected = model != null && model.Level == 2 ? true : false });
            levelHSK.Add(new SelectListItem() { Value = "3", Text = "HSK3", Selected = model != null && model.Level == 3 ? true : false });
            levelHSK.Add(new SelectListItem() { Value = "4", Text = "HSK4", Selected = model != null && model.Level == 4 ? true : false });
            levelHSK.Add(new SelectListItem() { Value = "5", Text = "HSK5", Selected = model != null && model.Level == 5 ? true : false });
            levelHSK.Add(new SelectListItem() { Value = "6", Text = "HSK6", Selected = model != null && model.Level == 6 ? true : false });
            levelHSK.Add(new SelectListItem() { Value = "7", Text = "HSK7-9", Selected = model != null && model.Level == 7 ? true : false });
            ViewBag.LevelHSK = levelHSK.ToList();

            List<SelectListItem> typePlaceOfBirth = new List<SelectListItem>();
            typePlaceOfBirth.Add(new SelectListItem() { Value = "1", Text = "Hộ chiếu",Selected = model!= null && model.PlaceOfBirth == "1"? true:false });
            typePlaceOfBirth.Add(new SelectListItem() { Value = "2", Text = "Chứng minh thư", Selected = model != null && model.PlaceOfBirth == "2" ? true : false });
            typePlaceOfBirth.Add(new SelectListItem() { Value = "3", Text = "Căn cước công dân", Selected = model != null && model.PlaceOfBirth == "3" ? true : false });
            ViewBag.TypePlaceOfBirth = typePlaceOfBirth.ToList();
            var countryModel = _countryService.GetAllCountry();
            ViewBag.Country = countryModel.Select(c => new SelectListItem { Value = c.CountryID.ToString(), Text = c.CountryName, Selected = model != null && model.CountryCode == c.CountryID ? true : false }).ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Groups = _roleService.GetRoles();
            return PartialView("Create", new UserModel());
        }

        [HttpPost]
        public ActionResult Create(UserModel model)
        {
            string message;
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("Password", "Mật khẩu không được để trống.");
            }
            if (ModelState.IsValid)
            {
                var userId = _userService.CreateUser(model, out message);
                if (userId > 0)
                {
                    if (Request.Form["FormType"] != null && Request.Form["FormType"].ToLower() == "dmnv")
                    {
                        TempData["success"] = message;
                        return RedirectToAction("Index","Account/DanhMucNhanVien");
                    }
                    TempData["success"] = message;
                    return RedirectToAction("Index", "Account");
                }

                ViewBag.Groups = _roleService.GetRoles();
                TempData["error"] = message;
                if (Request.Form["FormType"] != null && Request.Form["FormType"].ToLower() == "dmnv")
                {
                    return RedirectToAction("Index", "Account/DanhMucNhanVien");
                }
                return RedirectToAction("Index", "Account");
            }

            ViewBag.Groups = _roleService.GetRoles();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = _userService.RetrieveUser(id);
            if (user != null)
            {
                return PartialView("Edit", user);
            }

            return PartialView("Edit", user);
        }
        [HttpPost]
        public ActionResult Edit(UserModel model,string passOld,string passNew,string passNewAgain,int Gender, string type=null)
        {
            string message;
            string avatar = _userService.GetById(model.UserId).Avatar;
            model.Gender = Gender;
            var avatarPath = Server.MapPath(avatar);
            if (passOld != "")
            {
                var checkPass = _userService.CheckPass(passOld, CurrentUser.UserId);
                if (checkPass != false)
                {
                    if (passNew == passNewAgain && passNew != "")
                    {
                        model.Password = passNew;
                        model.IsChangePassword = true;
                        var userModel = _userService.UpdateUser(model,out message);
                    }
                    else
                    {
                        TempData["errorPassNew"] = "Mật khẩu không khớp";
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                }
                else
                {
                    TempData["errorPassOld"] = "Sai mật khẩu";
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            if (model.ImageFile != null)
            {
                if (System.IO.File.Exists(avatarPath))
                {
                    System.IO.File.Delete(avatarPath);
                }
                model.Avatar = null;
                model.Thumbnail = null;
                string extension = Path.GetExtension(model.ImageFile.FileName).ToLower();
                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".jfif" && extension != ".gif")
                {
                    TempData["error"] = "Định dạng ảnh không hợp lệ";
                    return Redirect(Request.UrlReferrer.ToString());
                }
                Image img = Image.FromStream(model.ImageFile.InputStream);
                string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
                var imagePath = SystemConfiguration.GetStringKey("UserAvatarPath");
                var imagePathThumbnail = SystemConfiguration.GetStringKey("UserAvatarThumbnailPath");
                model.Avatar = imagePath + fileName;
                model.Thumbnail = imagePathThumbnail + fileName;
                string fileNameThumb = Path.Combine(Server.MapPath(imagePathThumbnail), fileName);
                fileName = Path.Combine(Server.MapPath(imagePath), fileName);
                bool exists = System.IO.Directory.Exists(Server.MapPath(imagePath));
                bool existsThumb = System.IO.Directory.Exists(Server.MapPath(imagePathThumbnail));
                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(imagePath));
                }

                if (!existsThumb)
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(imagePathThumbnail));
                }
                var ratio = (double)100 / img.Height;
                int imageHeight = (int)(img.Height * ratio);
                int imageWidth = (int)(img.Width * ratio);

                Image.GetThumbnailImageAbort dCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                Image thumbnailImg = img.GetThumbnailImage(imageWidth, imageHeight, dCallback, IntPtr.Zero);

                thumbnailImg.Save(Path.Combine(Server.MapPath(imagePathThumbnail), fileNameThumb), ImageFormat.Jpeg);
                thumbnailImg.Dispose();
                model.ImageFile.SaveAs(fileName);
            }
            else
            {
                model.Avatar = avatar;
            }
            var isSuccess = _userService.UpdateUser(model, out message);
            if (isSuccess)
            {
                    
                if (Request.Form["FormType"] != null && Request.Form["FormType"].ToLower() == "dmnv")
                {
                    TempData["success"] = message;
                    return RedirectToAction("Index", "Account/DanhMucNhanVien");
                }
                if (Request.Form["FormType"] != null && Request.Form["FormType"].ToLower() == "account")
                {
                    TempData["success"] = message;
                    return RedirectToAction("Index", "Account");
                }
                if (type == "uf")
                {
                    TempData["success"] = message;
                    return RedirectToAction("UserInfo", "Account");
                }
                TempData["success"] = message;
                HttpRuntime.Cache.Remove(string.Format(Constant.CacheUserLoginKey, model.Email));
                return RedirectToAction("Index", "Account");
                //return Redirect(Request.UrlReferrer.ToString());
            }

            //ViewBag.Groups = _roleService.GetRoles();
            TempData["error"] = message;
            if (Request.Form["FormType"] != null && Request.Form["FormType"].ToLower() == "dmnv")
            {
                return RedirectToAction("Index", "Account/DanhMucNhanVien");
            }
            if (Request.Form["FormType"] != null && Request.Form["FormType"].ToLower() == "account")
            {
                return RedirectToAction("Index", "Account");
            }
            return View(model);
          

            //ViewBag.Groups = _roleService.GetRoles();
        }
        public bool ThumbnailCallback()
        {
            return false;
        }
        [HttpPost]
        public ActionResult Delete(int userId)
        {
            string message;
            if (CurrentUser.UserId == userId)
            {
                return Json(new { IsError = true, Message = "Bạn không thể xóa tài khoản của bạn" });
            }

            var result = _userService.DeleteUser(userId, out message);
            if (result)
            {
                TempData["success"] = message;
                return Json(new { IsError = false });
            }
            return Json(new { IsError = true, Message = message });
        }

        [HttpPost]
        [CustomAuthorize(Roles = RoleHelper.UserInvisibe)]
        public ActionResult Invisibe(int userId)
        {
            string message;
            var result = _userService.ChangeStatus(userId, out message);
            if (result)
            {
                return Json(new { IsError = false, Message = message });
            }
            return Json(new { IsError = true, Message = message });
        }

        [HttpPost]
        [CustomAuthorize]
        public ActionResult ChangePassword(Models.ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string message;
                var isSuccess = _userService.ChangePassword(CurrentUser.UserId, model.OldPassword, model.NewPassword, out message);
                if (isSuccess)
                {
                    ModelState.Clear();
                    return Json(new { IsError = false, Message = message });
                }

                return Json(new { IsError = true, Message = message });
            }

            var html = RenderPartialViewToString("~/Views/Account/Partial/_ChangePassword.cshtml", model);
            return Json(new { IsError = true, HTML = html, Message = "Cập nhật không thành công" });
        }

        void CreateTokenProvider(UserManager<IdentityUser> manager, string tokenType)
        {
            manager.UserTokenProvider = new EmailTokenProvider<IdentityUser>();
        }


        #region Helpers

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion

    }
}