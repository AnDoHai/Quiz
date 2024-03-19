using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tms.Models
{
    public enum LoginResult
    {
        Unknown = 0,
        Success = 1,
        IsLockedOut = 2,
        InvalidEmail = 3,
        InvalidPassword = 4,
        UnActive = 5,
    }
    public enum MessageType
    {
        Success,
        Error,
        Notice,
        Warning
    }

    public enum CustomerStatus : short
    {
        Unactive = 0,
        Active = 1
    }

    public enum AccountTokenType : short
    {
        ActiveAccount = 1,
        PasswordRecover = 2,
        RemoteLogin = 3
    }

    public enum NotificationType
    {
        System = 1,
        Message = 2
    }
   
    public enum TransactionStatus
    {
        Done = 1,
        Error = 2
    }
    public enum CreditType
    {
        VNAccount = 1,
        CNAccount = 2,
        AlipayAccount = 3
    }

    public enum Roles
    {
        PhongKeHoach = 23,
        PhongKinhDoanh = 24,
        KhoChinh = 25,
        Xuong = 26
    }
    public enum ContestType
    {
        [Display(Name = "Phần thi nghe")]
        Listening = 1,
        [Display(Name = "Phần thi đọc")]
        Reading = 2,
        [Display(Name = "Phần thi viết")]
        Writing = 3,
        [Display(Name = "Phần thi dịch")]
        Translate = 4,
    }
    public enum StatusQuestionHistoryType
    {
        [Display(Name = "Đúng")]
        True = 1,
        [Display(Name = "Sai")]
        False = 2,
        [Display(Name = "Đang chấm")]
        LoandingQuestion = 3,
    }
    public enum TypeChoice
    {
        [Display(Name = "Hình ảnh")]
        Image = 0,
        [Display(Name = "Chữ")]
        Text = 1,
    }
    public enum TypeCategory
    {
        [Display(Name = "HSK1")]
        HSK1 = 1,
        [Display(Name = "HSK2")]
        HSK2 = 2,
        [Display(Name = "HSK3")]
        HSK3 = 3,
        [Display(Name = "HSK4")]
        HSK4 = 4,
        [Display(Name = "HSK5")]
        HSK5 = 5,
        [Display(Name = "HSK6")]
        HSK6 = 6,
        [Display(Name = "HSK7 đến 9")]
        HSK7to9 = 7,
        [Display(Name = "HSK sơ cấp")]
        HSKLow = 8,
        [Display(Name = "HSK trung cấp")]
        HSKNormal = 9,
        [Display(Name = "HSK cao cấp")]
        HSKHight = 10,

    }

    public enum LevelHSK
    {
        [Display(Name = "HSK1")]
        HSK1 = 1,
        [Display(Name = "HSK2")]
        HSK2 = 2,
        [Display(Name = "HSK3")]
        HSK3 = 3,
        [Display(Name = "HSK4")]
        HSK4 = 4,
        [Display(Name = "HSK5")]
        HSK5 = 5,
        [Display(Name = "HSK6")]
        HSK6 = 6,
        [Display(Name = "HSK7 đến 9")]
        HSK7to9 = 7,
    }
    public enum OrderSection
    {
        [Display(Name = "Hình ảnh")]
        Image = 0,
        [Display(Name = "Chữ")]
        Text = 1,
        [Display(Name = "Không hiển thị")]
        None = 2,
    }


    public enum OrderQuestion
    {
        [Display(Name = "Hình ảnh")]
        Image = 0,
        [Display(Name = "Chữ")]
        Text = 1,
        [Display(Name = "Âm thanh")]
        Audio = 2,
        [Display(Name = "Chữ và ảnh")]
        TextAndImage = 3,
        [Display(Name = "Chữ và âm thanh")]
        TextAndAudio = 4,
        [Display(Name = "Âm thanh và ảnh")]
        AudioAndImage = 5,
        [Display(Name = "Không hiển thị")]
        DisplayNone = 6,
    }
    public enum TypeSection
    {
        [Display(Name = "Có hiển thị âm thanh")]
        Audio = 0,
        [Display(Name = "Không Có hiển thị âm thanh")]
        UnAudio = 1,
    }
    public enum LayoutQuestion
    {
        [Display(Name = "Hiển thị ngang 6 cột")]
        Ngang6 = 0,
        [Display(Name = "Hiển thị ngang 4 cột")]
        Ngang4 = 1,
        [Display(Name = "Hiển thị ngang 2 cột")]
        Ngang2 = 2,
        [Display(Name = "Hiển thị dọc 1 cột")]
        Doc = 5,
    }

    //public enum TypeTrueFalse
    //{
    //    [Display(Name = "Âm thanh và ảnh")]
    //    AudioAndPicture = 0,
    //    [Display(Name = "Âm thanh và chữ")]
    //    AudioAndText = 1,
    //    [Display(Name = "Ảnh và chữ")]
    //    ImageAndText = 2,
    //    [Display(Name = "Chữ")]
    //    Text = 3,
    //}


    public enum TypeQuiz
    {
        [Display(Name = "Khởi tạo")]
        CreateExam = 1,
        [Display(Name = "Đang thi")]
        DuringExam = 2,
        [Display(Name = "Kết thúc")]
        EndExam = 3,
    }

    public enum TypeQuestion
    {
        [Display(Name = "Đúng/Sai")]
        DungSai = 0,
        [Display(Name = "Chọn 1 đáp án đúng")]
        DapAnDung = 1,
        [Display(Name = "Sắp xếp thứ tự đúng")]
        SapXep = 2,
        [Display(Name = "Nhập câu trả lời")]
        CauTraLoi = 3,
        [Display(Name = "Viết đoạn văn mô tả")]
        DoanMoTa = 4,
        [Display(Name = "Đọc nhớ và viết lại câu")]
        DocNho = 5,
        [Display(Name = "Phần nghe và nói HSKK")]
        NgheNoi = 6,
    }
    public enum TimeLimitType
    {
        [Display(Name = "Nghe một lần")]
        ListenOne = 0,
        [Display(Name = "Không giới hạn")]
        ListenMutil = 1,
    }
    public enum TransferType
    {
        FromCNYCardToAlipay = 1,
        FromAlipayToCNYCard = 2
    }
    
}
