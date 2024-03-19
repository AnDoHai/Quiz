using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Tms.DataAccess.LinqExtensions
{
    public static class OrderExtension
    {
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> q, string SortField, bool Ascending)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, SortField);
            var exp = Expression.Lambda(prop, param);
            string method = Ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> q, string SortField, string Ascending)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var childField = SortField.Split('.');
            var prop = Expression.Property(param, childField[0]);
            if (childField.Length > 1)
            {
                prop = Expression.Property(prop, childField[1]);
            }
            var exp = Expression.Lambda(prop, param);
            string method = (Ascending.Trim().Replace("\t" , "").ToLower() == "0") ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }

        public static IQueryable<T> SearchByField<T>(this IQueryable<T> q, string SearchField, string SearchValue)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var childField = SearchField.Split('.');
            var prop = Expression.Property(param, childField[0]);
            if (childField.Length > 1)
            {
                prop = Expression.Property(prop, childField[1]);
            }
            var searchValue = Expression.Constant(SearchValue);
            MethodInfo methodContains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var exp = Expression.Lambda<Func<T, bool>>(Expression.Call(prop, methodContains, searchValue), param);
            string method = "Where";
            Type[] types = new Type[] { q.ElementType };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }

        //Nếu chuỗi có dấu " => trả về 0 (sắp xếp chuỗi có dấu " => xếp trước, ngược lại xếp sau)
        public static int ExtractString(string s)
        {
            int s2 = s.Contains("\"") ? 0 : 1;
            return s2;
        }
        //Lấy số đầu tiên trong chuỗi
        public static int ExtractNumber(string s)
        {
            string str = Regex.Match(s, @"\d+").Value;
            int s2 = str != "" ? Convert.ToInt32(str) : 0;
            return s2;
        }
    }
}
