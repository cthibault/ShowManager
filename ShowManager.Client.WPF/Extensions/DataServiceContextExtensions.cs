using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShowManager.Client.WPF.Extensions
{
    public static class DataServiceContextExtensions
    {
        public static void AttachTo<T>(this DataServiceContext context, Expression<Func<T>> entitySetExpression, object entity)
        {
            try
            {
                var entitySetPropertyName = PropertyHelper.ExtractPropertyName<T>(entitySetExpression);

                if (string.IsNullOrWhiteSpace(entitySetPropertyName))
                {
                    throw new ArgumentException("Invalid EntitySet expression", "entitySetExpression");
                }

                context.AttachTo(entitySetPropertyName, entity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public static class PropertyHelper
    {
        public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("The member access expression does not access a property.", "propertyExpression");
            }

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException("The expression is not a member access expression.", "propertyExpression");
            }

            return memberExpression.Member.Name;
        }
    }
}
