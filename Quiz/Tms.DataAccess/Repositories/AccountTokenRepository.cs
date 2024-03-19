using Tms.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tms.DataAccess.Repositories
{
    public interface IAccountTokenRepository: IBaseRepository<AccountToken>
    {
    }
    public class AccountTokenRepository : BaseRepository<AccountToken>, IAccountTokenRepository
    {
        public AccountTokenRepository(QuizSystemEntities context)
            : base(context)
        {
        }
    }
}
