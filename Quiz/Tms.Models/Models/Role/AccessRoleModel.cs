using Tms.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tms.Models
{
    public class AccessRoleModel
    {
        public List<RoleModel> Roles { get; set; }
        public List<ModuleActionModel> ModuleActions { get; set; }
        public int RoleId { get; set; }
    }
}
