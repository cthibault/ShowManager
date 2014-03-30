using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ShowManager.Client.WPF.Helpers;

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
}
