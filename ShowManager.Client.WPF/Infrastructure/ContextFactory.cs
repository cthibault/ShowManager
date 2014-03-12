using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowManager.Client.WPF.ShowManagement;

namespace ShowManager.Client.WPF.Infrastructure
{
    public static class ContextFactory
    {
        public static ShowManagementDBEntities Create()
        {
            return Create(MergeOption.OverwriteChanges);
        }

        public static ShowManagementDBEntities Create(MergeOption mergeOption)
        {
            var context = new ShowManagement.ShowManagementDBEntities(ContextFactory.ServiceUri);
            context.MergeOption = mergeOption;

            return context;
        }


        private static Uri ServiceUri
        {
            get
            {
                if (_serviceUri == null)
                {
                    var uriValue = ConfigurationManager.AppSettings["ContextUri"];

                    if (string.IsNullOrWhiteSpace(uriValue))
                    {
                        throw new InvalidOperationException("Invalid Context Uri");
                    }

                    _serviceUri = new Uri(uriValue);
                }

                return _serviceUri;
            }
        }
        private static Uri _serviceUri;
    }
}
