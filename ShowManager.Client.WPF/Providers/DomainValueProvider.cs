using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowManager.Client.WPF.Helpers;
using ShowManager.Client.WPF.ShowManagement;

namespace ShowManager.Client.WPF.Providers
{
    class DomainValueProvider : BaseProvider
    {
        public DomainValueProvider(ShowManagementDBEntities context)
            : base(context)
        {
        }

        #region GetParserTypes
        public Task<List<ParserType>> GetParserTypes()
        {
            var tcs = new TaskCompletionSource<List<ParserType>>();

            var query = this.Context.ParserTypes as DataServiceQuery<ParserType>;

            try
            {
                query.BeginExecute(asyncResult => this.OnGetParserTypesComplete(asyncResult, tcs), query);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            return tcs.Task;
        }
        private void OnGetParserTypesComplete(IAsyncResult result, TaskCompletionSource<List<ParserType>> tcs)
        {
            try
            {
                var query = result.AsyncState as DataServiceQuery<ParserType>;

                var parserTypes = query.EndExecute(result).ToList();

                tcs.TrySetResult(parserTypes);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        }
        #endregion
    }
}
