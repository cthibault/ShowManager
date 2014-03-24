using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using ShowManager.Client.WPF.Infrastructure;
using ShowManager.Client.WPF.ShowManagement;
using ShowManager.Client.WPF.ViewModels;
using ShowManager.Client.WPF.Views;
using ShowManager.Client.WPF.Extensions;
using GalaSoft.MvvmLight.Command;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using ShowManager.Client.WPF.Messages;
using System.Runtime.CompilerServices;


namespace ShowManager.Client.WPF.ViewModels
{
    class ShowsViewModel : BaseViewModel, ITabViewModel
    {
        public ShowsViewModel(IUnityContainer unityContainer)
            : base(unityContainer)
        {
            this.Initialize();
        }

        #region Initialization
        private void Initialize()
        {
            this.InitializeEvents();
            this.InitializeCommands();
        } 

        private void InitializeEvents()
        {
        }
        
        private void InitializeCommands()
        {
            this.RefreshAllCommand = new RelayCommand(this.OnRefreshAll);
            //this.RefreshCommand = new RelayCommand<Show>(this.OnRefresh);
            //this.AddCommand = new RelayCommand(this.OnAdd, () => this.Shows != null);
            this.EditCommand = new RelayCommand<Show>(this.OnEdit);
            //this.CloneCommand = new RelayCommand<Show>(this.OnClone);
            //this.DeleteCommand = new RelayCommand<Show>(this.OnDelete);
        }
        #endregion

        #region RefreshAll
        private DataServiceQueryContinuation<Show> _showContinuationToken;
        public RelayCommand RefreshAllCommand { get; private set; }
        
        private void OnRefreshAll()
        {
            BusyController.Default.SendMessage(true);

            this._showContinuationToken = null;

            var query = this.Context.Shows as DataServiceQuery<Show>;

            try
            {
                query.BeginExecute(this.OnRefreshAllComplete, query);
            }
            catch (Exception ex)
            {
                this.SendErrorMessage(ex.Message);
            }
        }

        private void OnRefreshAllComplete(IAsyncResult result)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    try
                    {
                        IEnumerable<Show> responseResults = null;

                        if (this._showContinuationToken == null)
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

                        this.Shows = new DataServiceCollection<Show>(responseResults);
                    }
                    catch (DataServiceQueryException ex)
                    {
                        this.SendErrorMessage(ex.Message);
                    }
                    finally
                    {
                        BusyController.Default.SendMessage(false);
                    }
                }, DispatcherPriority.Input);
        }
        #endregion

        #region Edit
        public RelayCommand<Show> EditCommand { get; private set; }

        private void OnEdit(Show show)
        {
            if (show != null)
            {
                this.EditShowViewModel.Reset()

                Action<Show> editAction = (s) => 
                {
                    if (s != null)
                    {
                        this.EditShowViewModel.Populate("edit", s);
                    }
                };

                this.GetShowDetails(show.ShowKey, editAction);
            }
        }        
        #endregion


        #region GetShowDetails
        private void GetShowDetails(int showKey, Action<Show> callbackAction)
        {
            BusyController.Default.SendMessage(true);

            var query = this.Context.Shows
                .Expand(s => s.ShowParsers)
                .Where(s => s.ShowKey == showKey) as DataServiceQuery<Show>;

            try
            {
                query.BeginExecute(ar => this.GetShowDetailsCompleted(ar, callbackAction), query);
            }
            catch (Exception ex)
            {
                this.SendErrorMessage(ex.Message);
            }
        }
        private void GetShowDetailsCompleted(IAsyncResult result, Action<Show> callbackAction)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    try
                    {
                        var query = result.AsyncState as DataServiceQuery<Show>;

                        var responseResults = query.EndExecute(result);

                        var show = responseResults.SingleOrDefault();
                        if (show != null)
                        {
                            var existingShow = this.Shows.SingleOrDefault(s => s.ShowKey == show.ShowKey);
                            if (existingShow != null)
                            {
                                existingShow = show;
                            }
                            else
                            {
                                this.Shows.Add(show);
                            }

                            callbackAction(show);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.SendErrorMessage(ex.Message);
                    }
                    finally
                    {
                        BusyController.Default.SendMessage(false);
                    }
                }, DispatcherPriority.Input);
        } 
        #endregion

        #region Shows
        public DataServiceCollection<Show> Shows
        {
            get { return this._shows; }
            set { this.Set(() => this.Shows, ref this._shows, value); }
        }
        private DataServiceCollection<Show> _shows;
        #endregion

        #region Selected Show
        public Show SelectedShow
        {
            get { return this._selectedShow; }
            set { this.Set(() => this.SelectedShow, ref this._selectedShow, value); }
        }
        private Show _selectedShow;
        #endregion

        #region EditShowViewModel
        public IEditShowViewModel EditShowViewModel
        {
            get
            {
                if (this._editShowViewModel == null)
                {
                    this._editShowViewModel = this.UnityContainer.Resolve<IEditShowViewModel>();

                    //this._editShowViewModel.Refreshed = show => this.OnRefresh(show);
                    //this._editShowViewModel.Saved = show => this.OnRefresh(show);
                    //this._editShowViewModel.Deleted = show => this.OnDelete(show);
                }

                return this._editShowViewModel;
            }
        }
        private IEditShowViewModel _editShowViewModel;
        #endregion

        #region Context
        private ShowManagement.ShowManagementDBEntities Context
        {
            get
            {
                if (this._context == null)
                {
                    this._context = ContextFactory.Create(MergeOption.OverwriteChanges);
                }
                return this._context;
            }
        }
        private ShowManagement.ShowManagementDBEntities _context;
        #endregion

        #region ITabViewModel
        public bool IsActive { get; set; }
        public string Title { get { return ShowsViewModel.TitleText; } }
        public IView View
        {
            get
            {
                if (this._view == null)
                {
                    this._view = this.UnityContainer.Resolve<IView>(this.Title);
                    this._view.DataContext = this;
                }
                return this._view;
            }
        }
        private IView _view;
        #endregion

        #region Constants
        public static readonly string TitleText = "shows";
        #endregion
    }
}
