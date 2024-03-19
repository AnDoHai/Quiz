using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tms.DataAccess;
using Tms.Models.Models.CountryModel;

namespace Tms.Services.AutoMap
{
    public static class CountryMapper
    {
		#region Mapping Contest
		public static CountryModel MapToModel(this Country entity)
		{
			return new CountryModel
			{
				CountryID = entity.CountryID,
				CountryName = entity.CountryName,
				ISO2 = entity.ISO2,
				ISO3 = entity.ISO3,
			};
		}
		public static CountryModel MapToModel(this Country entity, CountryModel model)
		{
			model.CountryID = entity.CountryID;
			model.CountryName = entity.CountryName;
			model.ISO2 = entity.ISO2;
			model.ISO3 = entity.ISO3;

			return model;
		}
		public static Country MapToEntity(this CountryModel model)
		{
			return new Country
			{
				CountryID = model.CountryID,
				CountryName = model.CountryName,
				ISO2 = model.ISO2,
				ISO3 = model.ISO3,
			};
		}
		public static Country MapToEntity(this CountryModel model, Country entity)
		{
			entity.CountryID = model.CountryID;
			entity.CountryName = model.CountryName;
			entity.ISO2 = model.ISO2;
			entity.ISO3 = model.ISO3;

			return entity;
		}
		public static List<Country> MapToEntities(this List<CountryModel> models)
		{
			return models.Select(x => x.MapToEntity()).ToList();
		}

		public static List<CountryModel> MapToModels(this List<Country> entities)
		{
			return entities.Select(x => x.MapToModel()).ToList();
		}
		#endregion
	}
}
