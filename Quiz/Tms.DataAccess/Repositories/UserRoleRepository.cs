using System.Linq;
using Tms.DataAccess.Common;

namespace Tms.DataAccess
{
    public interface IUserRoleRepository : IBaseRepository<UserRole>
    {
        UserRole RetrieveUserRole(int UserId);
    }
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(QuizSystemEntities context)
            : base(context)
        {
        }

        public UserRole RetrieveUserRole(int UserId)
        {
            return Dbset.FirstOrDefault(x => x.UserId == UserId);
        }
       
    }
}
