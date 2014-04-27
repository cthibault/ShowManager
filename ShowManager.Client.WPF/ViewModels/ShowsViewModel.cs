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
using ShowManager.Client.WPF.Providers;


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
            this.AddCommand = new RelayCommand(this.OnAdd, () => this.Shows != null);
            this.EditCommand = new RelayCommand<Show>(this.OnEdit);

            //this.RefreshCommand = new RelayCommand<Show>(this.OnRefresh);
            //this.CloneCommand = new RelayCommand<Show>(this.OnClone);
            //this.DeleteCommand = new RelayCommand<Show>(this.OnDelete);
        }
        #endregion

        #region RefreshAll
        private DataServiceQueryContinuation<Show> _showContinuationToken;
        public RelayCommand RefreshAllCommand { get; private set; }
        
        private async void OnRefreshAll()
        {
            await this.RefreshAll();
        }
        public async Task RefreshAll()
        {
            BusyController.Default.SendMessage(true);

            var sp = new ShowProvider(this.Context);

            try
            {
                var showsResult = await sp.GetBasicInformationForAllShows();

                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    this.Shows = showsResult;
                }, DispatcherPriority.Input);
            }
            catch (Exception ex)
            {
                this.SendErrorMessage("Error Retrieving Shows", ex);
            }
            finally
            {
                BusyController.Default.SendMessage(false);
            }
        }
        #endregion

        #region Add
        public RelayCommand AddCommand { get; private set; }

        private async void OnAdd()
        {
            await this.Add();
        }
        private async Task Add()
        {
            BusyController.Default.SendMessage(true);

            var clear = await this.EditShowViewModel.TryClearAsyc();
            if (clear)
            {
                var show = new Show() { AppInstanceKey = 17 };

                this.Shows.Add(show);

                this.EditShowViewModel.Populate("add", show);
            }

            BusyController.Default.SendMessage(false);
        }

        #endregion

        #region Edit
        public RelayCommand<Show> EditCommand { get; private set; }

        private async void OnEdit(Show show)
        {
            await this.Edit(show);
        }   
        private async Task Edit(Show show)
        {
            if (show != null)
            {
                BusyController.Default.SendMessage(true);

                var clear = await this.EditShowViewModel.TryClearAsyc();
                if (clear)
                {
                    var showResult = await this.GetShowDetails(show.ShowKey);

                    if (showResult != null)
                    {
                        this.EditShowViewModel.Populate("edit", showResult);
                    }
                }

                BusyController.Default.SendMessage(false);
            }
        }
        #endregion

        #region Save
        private async Task Save()
        {
            BusyController.Default.SendMessage(true);

            var sp = new ShowProvider(this.Context);

            try
            {
                var saveSuccess = await sp.SaveContext();

                if (saveSuccess)
                {
                    await this.EditShowViewModel.CloseAndDiscardChangesAsync();
                }
            }
            catch (Exception ex)
            {
                this.SendErrorMessage("Error Retrieving Shows", ex);
            }

            BusyController.Default.SendMessage(false);
        }
        #endregion

        #region Cancel
        private async Task Cancel(Show show)
        {
            BusyController.Default.SendMessage(true);

            var clear = await this.EditShowViewModel.TryClearAsyc();
            if (clear)
            {
                await this.EditShowViewModel.CloseAndDiscardChangesAsync();

                if (show != null && show.ShowKey > 0)
                {
                    await this.GetShowDetails(show.ShowKey);
                }
                else
                {
                    this.Shows.Remove(show);
                }
            }

            BusyController.Default.SendMessage(false);
        }
        #endregion

        #region Delete
        private async Task Delete(Show show)
        {
            BusyController.Default.SendMessage(true);

            var currentShow = this.Shows.SingleOrDefault(s => s.ShowKey == show.ShowKey);
            if (currentShow != null)
            {
                this.Shows.Remove(currentShow);

                var sp = new ShowProvider(this.Context);

                try
                {
                    var saveSuccess = await sp.SaveContext();

                    if (saveSuccess)
                    {
                        await this.EditShowViewModel.CloseAndDiscardChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    this.SendErrorMessage("Error Retrieving Shows", ex);
                }
            }

            BusyController.Default.SendMessage(false);
        }
        #endregion


        #region GetShowDetails
        private async Task<Show> GetShowDetails(int showKey)
        {
            Show show = null;

            var sp = new ShowProvider(this.Context);

            try
            {
                show = await sp.GetShowDetails(showKey);

                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
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
                    }
                }, DispatcherPriority.Input);
            }
            catch (Exception ex)
            {
                this.SendErrorMessage("Error Retrieving Shows", ex);
            }

            return show;
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

                    this._editShowViewModel.Save = async show => await this.Save();
                    this._editShowViewModel.Refresh = async show => await this.Edit(show);
                    this._editShowViewModel.Cancel = async show => await this.Cancel(show);
                    this._editShowViewModel.Delete = async show => await this.Delete(show);
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
