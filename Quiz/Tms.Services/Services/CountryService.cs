using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tms.DataAccess;
using Tms.Services.AutoMap;
using Tms.DataAccess.Common;
using Tms.DataAccess.Repositories;
using Tms.Models.Models.CountryModel;

namespace Tms.Services.Services
{
    public interface ICountryService : IEntityService<Country>
    {
        List<CountryModel> GetAllCountry();
    }
    public class CountryService : EntityService<Country>, ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        public CountryService(IUnitOfWork unitOfWork, ICountryRepository countryRepository)
            : base(unitOfWork, countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public List<CountryModel> GetAllCountry()
        {
            try
            {
                var entities = _countryRepository.GetAll().ToList();
                if (entities != null && entities.Any())
                {
                    return entities.MapToModels();
                }
            }catch(Exception ex)
            {
                Log.Error("Search Contest error", ex);
            }
            return null;
        }
    }
}
