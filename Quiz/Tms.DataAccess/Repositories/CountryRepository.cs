using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tms.DataAccess.Common;

namespace Tms.DataAccess.Repositories
{
    public interface ICountryRepository : IBaseRepository<Country>
    {
       
    }

    public class CountryRepository : BaseRepository<Country>, ICountryRepository
    {
        public CountryRepository(QuizSystemEntities context) : base(context)
        {
        }

     
    }
}
