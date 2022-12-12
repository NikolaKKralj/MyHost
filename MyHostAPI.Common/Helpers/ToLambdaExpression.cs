using System;
using System.Linq.Expressions;

namespace MyHostAPI.Common.Helpers
{
    public static class ToLambdaExpression
    {
        public static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));
            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }
    }
}

