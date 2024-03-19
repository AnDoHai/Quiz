using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Tms.DataAccess;

namespace Tms.Services
{
    public static class AutoMapper
    {
        private static IMapper _mapper;

        #region Constructor
        public static IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    Init();
                }

                return _mapper;
            }
        }
        public static MapperConfiguration MapperConfiguration { get; private set; }

        //Map extension
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return Mapper.Map(source, destination);
        }
        #endregion

        public static void Init()
        {
            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<string, string>().ConvertUsing<StringTypeConverter>();
                cfg.CreateMap<string, int?>().ConvertUsing(new IntTypeConverter());
                cfg.CreateMap<string, decimal?>().ConvertUsing(new DecimalTypeConverter());
                cfg.CreateMap<string, DateTime?>().ConvertUsing(new DateTimeTypeConverter());
                cfg.CreateMap<string, Type>().ConvertUsing<TypeTypeConverter>();
                cfg.CreateMap<DateTime?, string>().ConvertUsing(new DateTimeToStringTypeConverter());
                cfg.CreateMap<DateTime, string>().ConvertUsing(new DateToStringTypeConverter());
            });
            _mapper = MapperConfiguration.CreateMapper();
        }

        


        #region Private Class
        private class StringTypeConverter : ITypeConverter<string, string>
        {
            public string Convert(string source, string destination, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source))
                {
                    return string.Empty;
                }
                return source?.Trim();
            }
        }

        private class DateTimeTypeConverter : ITypeConverter<string, DateTime?>
        {
            public DateTime? Convert(string source, DateTime? destination, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source))
                {
                    return null;
                }
                try
                {
                    var date = DateTime.TryParse(source, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dest);
                    if (!date)
                    {
                        var _date = DateTime.ParseExact(source, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                        return _date;
                    }
                    return System.Convert.ToDateTime(source);
                }
                catch (Exception)
                {
                    DateTime dt = DateTime.ParseExact(source, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    return dt;
                }

            }
        }

        private class DateTimeToStringTypeConverter : ITypeConverter<DateTime?, string>
        {
            public string Convert(DateTime? source, string destination, ResolutionContext context)
            {
                try
                {
                    if (source == null || !source.HasValue)
                    {
                        return string.Empty;
                    }
                    return source.Value.ToString("yyyy-MM-dd");
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        private class DateToStringTypeConverter : ITypeConverter<DateTime, string>
        {
            public string Convert(DateTime source, string destination, ResolutionContext context)
            {
                try
                {

                    return source.ToString("yyyy-MM-dd");
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        private class IntTypeConverter : ITypeConverter<string, int?>
        {
            public int? Convert(string source, int? destination, ResolutionContext context)
            {
                try
                {
                    if (string.IsNullOrEmpty(source))
                    {
                        return null;
                    }
                    return System.Convert.ToInt32(source);
                }
                catch
                {
                    return null;
                }
            }
        }

        private class DecimalTypeConverter : ITypeConverter<string, decimal?>
        {
            public decimal? Convert(string source, decimal? destination, ResolutionContext context)
            {
                try
                {
                    if (string.IsNullOrEmpty(source))
                    {
                        return null;
                    }
                    return System.Convert.ToDecimal(source);
                }
                catch
                {
                    return null;
                }
            }
        }

        private class TypeTypeConverter : ITypeConverter<string, Type>
        {
            public Type Convert(string source, Type destination, ResolutionContext context)
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetType(source);
            }
        } 
        #endregion
    }
}
