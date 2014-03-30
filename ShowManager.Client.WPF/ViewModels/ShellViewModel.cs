using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.Unity;
using ShowManager.Client.WPF.Infrastructure;
using ShowManager.Client.WPF.Messages;
using ShowManager.Client.WPF.ViewModels;

namespace ShowManager.Client.WPF.ViewModels
{
    class ShellViewModel : BaseViewModel
    {
        public ShellViewModel(IUnityContainer unityContainer)
            : base(unityContainer)
        {
            this.Initialize();
        }

        #region Initialization
        private void Initialize()
        {
            this.InitializeEvents();
            this.InitializeCommands();

            this.LoadData();
        }
        private void InitializeEvents()
        {
            //this.EventPublisher.GetEvent<ClosingEvent>().Subscribe(e => { });
            
            this.MessengerInstance.Register<DisplayStatusMessage>(this, x => this.DisplayMessageController.Add(x.Text));
            this.MessengerInstance.Register<BusyMessage>(this, x => this.IsBusy = x.IsBusy);
        }
        private void InitializeCommands()
        {
            this.RemoveCurrentMessageCommand = new RelayCommand(() => this.DisplayMessageController.RemoveCurrent());
        }
        #endregion

        #region LoadData
        private void LoadData()
        {
            var showsViewModel = this.UnityContainer.Resolve<ShowsViewModel>();
            this.TabViewModels.Add(showsViewModel);

            this.SelectedTabViewModel = showsViewModel;
            showsViewModel.RefreshAll();
        }
        #endregion

        #region TabViewModels
        public ObservableCollection<ITabViewModel> TabViewModels
        {
            get
            {
                if (this._tabViewModels == null)
                {
                    this._tabViewModels = new ObservableCollection<ITabViewModel>();
                }
                return this._tabViewModels;
            }
        }
        private ObservableCollection<ITabViewModel> _tabViewModels;
        #endregion

        #region SelectedTabViewModel
        public ITabViewModel SelectedTabViewModel
        {
            get { return this._selectedTabViewModels; }
            set { this.Set(() => this.SelectedTabViewModel, ref this._selectedTabViewModels, value); }
        }
        private ITabViewModel _selectedTabViewModels;
        #endregion

        #region RemoveCurrentMessageCommand
        public RelayCommand RemoveCurrentMessageCommand { get; private set; }
        #endregion

        #region DisplayMessageManager
        public DisplayMessageController DisplayMessageController
        {
            get
            {
                if (this._displayMessageManager == null)
                {
                    this._displayMessageManager = new DisplayMessageController();
                }
                return this._displayMessageManager;
            }
        }
        private DisplayMessageController _displayMessageManager;
        #endregion

        #region IsBusy
        public bool IsBusy
        {
            get { return this._isBusy; }
            set { this.Set(() => this.IsBusy, ref this._isBusy, value); }
        }
        private bool _isBusy;
        #endregion
    }
}
