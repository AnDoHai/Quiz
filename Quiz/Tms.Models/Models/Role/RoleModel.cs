using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tms.Models.Role
{
    public class RoleModel
    {
        public int RoleId { get; set; }
        public string Code { get; set; }
        [Required(ErrorMessage = "Tên quyền không được bỏ trống")]
        public string Name { get; set; }
        public string Note { get; set; }
        public bool Status { get; set; }
        public string CreatedByUserId { get; set; }

        public string UpdatedUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<ModuleActionModel> ModuleActions { get; set; }
    }
}
