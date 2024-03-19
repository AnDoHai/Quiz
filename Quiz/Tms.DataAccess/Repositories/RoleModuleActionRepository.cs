using Tms.DataAccess.Common;

namespace Tms.DataAccess.Repositories
{
    public interface IRoleModuleActionRepository : IBaseRepository<RoleModuleAction>
    {

    }
    public class RoleModuleActionRepository : BaseRepository<RoleModuleAction>, IRoleModuleActionRepository
    {
        public RoleModuleActionRepository(QuizSystemEntities context)
            : base(context)
        {
        }

    }
}
