using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Microsoft.Practices.Unity;
using ShowManager.Client.WPF.Infrastructure;

namespace ShowManager.Client.WPF.ViewModels
{
    class BaseViewModel : ViewModelBase
    {
        protected BaseViewModel(IUnityContainer unityContainer, IEventPublisher eventPublisher)
        {
            this.UnityContainer = unityContainer;
            this.EventPublisher = eventPublisher;
        }

        protected IUnityContainer UnityContainer
        {
            get
            {
                if (this._unityContainer == null)
                {
                    this._unityContainer = App.UnityContainer;
                }
                return this._unityContainer;
            }
            set { this._unityContainer = value; }
        }
        private IUnityContainer _unityContainer;

        protected IEventPublisher EventPublisher
        {
            get
            {
                if (this._eventPublisher == null)
                {
                    this._eventPublisher = this.UnityContainer.Resolve<IEventPublisher>();
                }
                return this._eventPublisher;
            }
            set { this._eventPublisher = value; }
        }
        private IEventPublisher _eventPublisher;
    }
}
