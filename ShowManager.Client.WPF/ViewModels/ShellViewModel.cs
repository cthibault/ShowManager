using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using ShowManager.Client.WPF.Events;
using ShowManager.Client.WPF.Infrastructure;
using ShowManager.Client.WPF.ViewModels.Interfaces;

namespace ShowManager.Client.WPF.ViewModels
{
    class ShellViewModel : BaseViewModel
    {
        public ShellViewModel(IUnityContainer unityContainer, IEventPublisher eventPublisher)
            : base(unityContainer, eventPublisher)
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
            this.EventPublisher.GetEvent<ClosingEvent>().Subscribe(e => { });
        }
        private void InitializeCommands()
        {
            
        }
        #endregion

        #region LoadData
        private void LoadData()
        {
            var showsViewModel = this.UnityContainer.Resolve<ShowsViewModel>();
            this.TabViewModels.Add(showsViewModel);
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

    }
}
