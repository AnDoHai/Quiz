using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tms.Models
{
    public static class Constant
    {
        public static int DefaultPageSize = 20;
        public static int CacheTimeoutInDaysDinhMucKH = 90;//90days
        public static int CacheTimeoutInMinuteTonKhoKH = 4320;//1440=24*60
        public static int CacheTimeoutInMinuteTonKho = 3;//180=3*60
        public static string CacheUserLoginKey = "User_{0}"; //0:UserName
        public static string CacheRoleModuleActionKey = "RoleModuleAction_{0}"; //0:email
        public static string CacheDinhMucSanXuatKey = "DinhMucSX_{0}"; //0:KhSXNKXHId
        public static string CacheTonKhoSanXuatKey = "TonKhoSX_{0}_{1}_{2}"; //0:VatTuId, KhoId, NgayTon

        //ISO
        public static string ISO_PhieuYeuCauMuaHang = "PL-09-KH-01-008-06/00";
        public static string ISO_PhieuKiemHoa = "PL-07-KT-01-001-05/00";
    }
}
