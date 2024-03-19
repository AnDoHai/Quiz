using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Tms.Web.Controllers
{
    public class UploadController : Controller
    {
        //
        // GET: /Upload/
        const string UploadPath = "/Uploads/Editors/";

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UploadUsingCK(HttpPostedFileBase fileToUpload)
        {
            string resultIconUrl = "";
            // Check xem phải có file được submit
            if (fileToUpload != null && fileToUpload.ContentLength > 0)
            {
                // Biến lưu tên file
                var fileName = Path.GetFileName(fileToUpload.FileName);
                if (System.IO.File.Exists(Server.MapPath("~" + UploadPath + fileName)))
                {
                    // cắt chuỗi bởi dấu '.' -> lấy phần mở rộng của tập tin, ví dụ .jpg, .doc, ...
                    string[] fileParts = fileName.Split(new char[] { '.' });
                    fileName = fileParts[0] + "2" + "." + fileParts[1];
                }
                //Lấy địa chỉ của ảnh IconUrl
                resultIconUrl = UploadPath + fileName;
                // Lưu file vào ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~" + UploadPath), fileName);
                fileToUpload.SaveAs(path);
            }
            return Json(new { url = resultIconUrl }, JsonRequestBehavior.AllowGet);
        }

    }
}