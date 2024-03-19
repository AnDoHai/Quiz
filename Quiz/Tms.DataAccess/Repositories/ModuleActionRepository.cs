using Tms.DataAccess.Common;
using System.Collections.Generic;
using System.Linq;

namespace Tms.DataAccess.Repositories
{
    public interface IModuleActionRepository: IBaseRepository<ModuleAction>
    {
        Dictionary<string, string> GetModuleActionByUserId(int userId, List<int> listRoleId);
    }
    public class ModuleActionRepository : BaseRepository<ModuleAction>, IModuleActionRepository
    {
        public ModuleActionRepository(QuizSystemEntities context)
            : base(context)
        {
        }

        public Dictionary<string, string> GetModuleActionByUserId(int userId, List<int> listRoleId)
        {
            if (listRoleId != null && listRoleId.Count != 0)
            {
                var query = Dbset.Where(c => c.RoleModuleActions.Any(r => listRoleId.Contains(r.RoleID)));
                var listModuleActions = query.Select(c => string.Concat(c.Module + "-", c.Action)).ToList();

                var result = new Dictionary<string, string>();
                foreach (var item in listModuleActions)
                {
                    result[item.ToLower()] = item.ToLower();
                }

                return result;
            }

            return new Dictionary<string, string>();
        }
    }
}
