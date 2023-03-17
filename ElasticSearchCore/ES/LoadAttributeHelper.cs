using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Zhaoxi.ElasticSearchCore.ES
{
    public class LoadAttributeHelper
    {
        public static object LoadAttributeByType<T, TS>(Expression<Func<T, object>> expression)
        {
            var propertyInfo = ReflectionExtensionHelper.GetProperty(expression) as PropertyInfo;
            if (propertyInfo == null)
                throw new ArgumentNullException(typeof(T).Name);

            var attribute = propertyInfo.GetCustomAttributes(typeof(TS), false)
                .FirstOrDefault();
            if (attribute == null)
                throw new ArgumentNullException(typeof(TS).Name);
            return attribute;
        }
    }
}
