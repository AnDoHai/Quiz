using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Tms.Models.User
{
    public class UserModel
    {
        public Int32 UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email đăng nhập không được để trống")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Chứng minh nhân dân không được để trống")]
        public string IdentityCardNo { get; set; }
        [Display(Name = "Quê quán không được để trống!")]
        public string PlaceOfBirth { get; set; }
        public string PasswordSalt { get; set; }
        public string FullName { get; set; }
        public bool IsSupperAdmin { get; set; }
        public string ChineseName { get; set; }
        public string LevelName { get; set; }
        public string Address { get; set; }
        public string Male { get; set; }
        public int Gender { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string EmployerCode { get; set; }
        public DateTime? Birthday { get; set; }
        [Required(ErrorMessage = "Ngày sinh không được để trống!")]
        public string BirthdayStr { get; set; }
        public DateTime LastActivityDate { get; set; }
        public bool IsLockedOut { get; set; }
        public String Avatar { get; set; }
        public bool Status { get; set; }
        public bool Remember { get; set; }
        [Required(ErrorMessage = "Nhóm không được để trống")]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public IList<SelectListItem> AvailableInvnetories { get; set; }
        public int? InventorySelected { get; set; }
        public int? DonViId { get; set; }
        public int? KhoId { get; set; }
        public string DonViName { get; set; }
        public bool IsPhuTrachDonVi { get; set; }
        public string ChucVu { get; set; }
        [Display(Name = "Số điện thoại không được để trống!")]
        [RegularExpression(@"^\+?\d{1,3}?([- .])?\(?(?:\d{2,3})\)?[- .]?\d\d\d[- .]?\d\d\d\d$", ErrorMessage = "Dữ liệu không hợp lệ!")]
        [MaxLength(15)]
        [MinLength(9)]
        public string Tel { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public string Thumbnail { get; set; }
        public string Nationality { get; set; }
        public bool IsChangePassword { get; set; }
        [Required(ErrorMessage = "Cấp độ không được để trống!")]
        public int Level { get; set; }
        [Required(ErrorMessage = "Quốc tịch được để trống!")]
        public int CountryCode { get; set; }

        public UserModel()
        {
            this.BirthdayStr = this.Birthday.HasValue ? this.Birthday.Value.ToString("dd/MM/yyyy") : "";
        }
    }

    public class UserLoginModel
    {
        public Int32 UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email đăng nhập không được để trống")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        //[StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự!", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        //[Required(ErrorMessage = "Chứng minh nhân dân không được để trống")]
        public string IdentityCardNo { get; set; }
        //[Display(Name = "Quê quán không được để trống!")]
        public string PlaceOfBirth { get; set; }
        public string PasswordSalt { get; set; }
        public string FullName { get; set; }
        public bool IsSupperAdmin { get; set; }
        public string ChineseName { get; set; }
        public string Address { get; set; }
        public string Male { get; set; }
        public int Gender { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string EmployerCode { get; set; }
        public DateTime? Birthday { get; set; }
        //[Required(ErrorMessage = "Ngày sinh không được để trống!")]
        public string BirthdayStr { get; set; }
        public DateTime LastActivityDate { get; set; }
        public bool IsLockedOut { get; set; }
        public String Avatar { get; set; }
        public bool Status { get; set; }
        public bool Remember { get; set; }
        //[Required(ErrorMessage = "Nhóm không được để trống")]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public IList<SelectListItem> AvailableInvnetories { get; set; }
        public int? InventorySelected { get; set; }
        public int? DonViId { get; set; }
        public int? KhoId { get; set; }
        public string DonViName { get; set; }
        public bool IsPhuTrachDonVi { get; set; }
        public string ChucVu { get; set; }
        //[Display(Name = "Số điện thoại không được để trống!")]
        //[RegularExpression(@"^\+?\d{1,3}?([- .])?\(?(?:\d{2,3})\)?[- .]?\d\d\d[- .]?\d\d\d\d$", ErrorMessage = "Dữ liệu không hợp lệ!")]
        //[MaxLength(15)]
        //[MinLength(9)]
        public string Tel { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public string Thumbnail { get; set; }
        public string Nationality { get; set; }
        public bool IsChangePassword { get; set; }
        //[Required(ErrorMessage = "Cấp độ không được để trống!")]
        public int Level { get; set; }
        //[Required(ErrorMessage = "Quốc tịch được để trống!")]
        public int CountryCode { get; set; }

        public UserLoginModel()
        {
            this.BirthdayStr = this.Birthday.HasValue ? this.Birthday.Value.ToString("dd/MM/yyyy") : "";
        }
    }
}
