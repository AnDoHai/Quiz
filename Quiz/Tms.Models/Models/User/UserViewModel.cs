using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tms.Models.User
{
    public class RegisterModel
    {
        [Display(Name = "Chọn ảnh đại diện")]
        public String Avatar { get; set; }

        [Required(ErrorMessage = "Email không được để trống!")]
        [EmailAddress(ErrorMessage = "Định dạng mail không đúng!")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Số điện thoại không được để trống!")]
        [RegularExpression(@"^\+?\d{1,3}?([- .])?\(?(?:\d{2,3})\)?[- .]?\d\d\d[- .]?\d\d\d\d$", ErrorMessage = "Dữ liệu không hợp lệ!")]
        [MaxLength(15)]
        [MinLength(9)]
        public string Tel { get; set; }
        [Display(Name = "Quê quán không được để trống!")]
        public string PlaceOfBirth { get; set; }
        [Required(ErrorMessage = "Chứng minh nhân dân không được để trống")]
        public string IdentityCardNo { get; set; }
        [Required(ErrorMessage = "Tên không được để trống!")]
        [MaxLength(100, ErrorMessage = "Tên tối đa 100 ký tự!")]
        public string UserName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống!")]
        public string BirthdayValue { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Dữ liệu không hợp lệ!")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự, tối đa 100 ký tự!", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        public string Country { get; set; }
        public int Gender { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không khớp!")]
        public string ConfirmPassword { get; set; }
        public string ChineseName { get; set; }
        [Required(ErrorMessage = "Cấp độ không được để trống!")]
        public string Level { get; set; }
        [Required(ErrorMessage = "Quốc tịch được để trống!")]
        public int CountryCode { get; set; }
    }

    public class ResetPasswordModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Xác nhật mật khẩu không khớp!")]
        public string ConfirmPassword { get; set; }

        public string PasswordToken { get; set; }
        public string Code { get; set; }
    }

    public class ForgotPassworsModel
    {
        [Required(ErrorMessage = "Email không được để trống!")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Mật khẩu cũ không được bỏ trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới không được bỏ trống")]
        [StringLength(100, ErrorMessage = "Mật khẩu mới phải tối thiểu 6 ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không trùng khớp")]
        public string ConfirmPassword { get; set; }
    }
}
