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
    class ShowProvider : BaseProvider
    {
        public ShowProvider(ShowManagementDBEntities context)
            : base(context)
        {
        }

        #region GetBasicInformationForAllShows
        public Task<DataServiceCollection<Show>> GetBasicInformationForAllShows()
        {
            var tcs = new TaskCompletionSource<DataServiceCollection<Show>>();

            var query = this.Context.Shows as DataServiceQuery<Show>;

            try
            {
                query.BeginExecute(asyncResult => this.OnGetBasicInformationForAllShowsComplete(asyncResult, tcs), query);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            return tcs.Task;
        }
        private void OnGetBasicInformationForAllShowsComplete(IAsyncResult result, TaskCompletionSource<DataServiceCollection<Show>> tcs)
        {
            try
            {
                IEnumerable<Show> responseResults = null;

                if (this._showContinuationGetBasicInformationForAllShows == null)
                {
                    // Since this is the first page, we get back the query
                    var query = result.AsyncState as DataServiceQuery<Show>;

                    // Get the response of the query
                    responseResults = query.EndExecute(result);
                }
                else
                {
                    // This is not the first page, so we get back the context
                    //svcContext = result.AsyncState as NorthwindEntities;
                    //response = svcContext.EndExecute<Order>(result);
                }

                tcs.TrySetResult(new DataServiceCollection<Show>(responseResults));
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        }
        private DataServiceQueryContinuation<Show> _showContinuationGetBasicInformationForAllShows = null; 
        #endregion

        #region GetShowDetails
        public Task<Show> GetShowDetails(int showKey)
        {
            var tcs = new TaskCompletionSource<Show>();

            var query = this.Context.Shows
                .Expand(s => s.Parsers)
                .Where(s => s.ShowKey == showKey) as DataServiceQuery<Show>;

            try
            {
                query.BeginExecute(asyncResult => this.OnGetShowDetailsComplete(asyncResult, tcs), query);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            return tcs.Task;
        }
        private void OnGetShowDetailsComplete(IAsyncResult result, TaskCompletionSource<Show> tcs)
        {
            try
            {
                var query = result.AsyncState as DataServiceQuery<Show>;

                var show = query.EndExecute(result).SingleOrDefault();

                tcs.TrySetResult(show);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        } 
        #endregion
    }
}
