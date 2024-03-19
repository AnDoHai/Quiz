using System.Data.Entity;
using System.Linq;
using Tms.DataAccess.Common;
using System.Collections.Generic;
using Tms.DataAccess.LinqExtensions;

namespace Tms.DataAccess
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User RetrieveUser(int userId);
        List<User> SearchUser(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);

        User GetUserByEmail(string email);
        User GetUserByUserName(string username);
        List<User> GetAllUserByListUser(List<string> listUser);
        List<User> GetAllUserByRoleCodes(List<string> roleCodes);

    }
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(QuizSystemEntities context)
            : base(context)
        {
        }

        public User RetrieveUser(int userId)
        {
            return Dbset.Include(c => c.UserRoles).FirstOrDefault(x => x.UserId == userId);
        }

        public User GetUserByUserName(string username)
        {
            return Dbset.Include(c => c.UserRoles).FirstOrDefault(c => c.UserName.Equals(username) && (!c.IsLockedOut));
        }

        public List<User> SearchUser(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage)
        {
            currentPage = (currentPage <= 0) ? 1 : currentPage;
            var query = Dbset.Include(x => x.UserRoles.Select(y => y.Role)).AsQueryable().Where(x => !x.IsSupperAdmin);
            if (!string.IsNullOrEmpty(textSearch))
            {
                query = query.Where(c => c.UserName.Contains(textSearch) || c.Email.Contains(textSearch) || c.UserRoles.Any(y => y.Role.Name.Contains(textSearch)));
            }

            totalPage = query.Count();
            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = query.OrderByField(sortColumn.Trim().Replace("\t" , ""), sortDirection);
            }
            else
                query = query.OrderByDescending(c => c.UserId);



            return query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

        }

        public User GetUserByEmail(string email)
        {
            return Dbset.Include(ct => ct.UserRoles).FirstOrDefault(c => c.Email.Equals(email));
        }

        public List<User> GetAllUserByListUser(List<string> listUser)
        {
            using (var dbContext = new QuizSystemEntities())
            {
                return dbContext.Users.Where(c => listUser.Contains(c.Email)).ToList();
            }
        }

        public List<User> GetAllUserByRoleCodes(List<string> roleCodes)
        {
            using (var dbContext = new QuizSystemEntities())
            {
                return dbContext.Users.Where(c => c.UserRoles.Any(o => roleCodes.Contains(o.Role.Code))).ToList();
            }
        }
    }
}
