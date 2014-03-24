using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Microsoft.Practices.Unity;
using ShowManager.Client.WPF.Infrastructure;
using ShowManager.Client.WPF.Messages;

namespace ShowManager.Client.WPF.ViewModels
{
    class BaseViewModel : ViewModelBase
    {
        protected BaseViewModel(IUnityContainer unityContainer)
        {
            this.UnityContainer = unityContainer;
        }

        #region SendErrorMessage
        protected void SendErrorMessage(string message, [CallerMemberName] string callingMethodName = null)
        {
            if (callingMethodName != null)
            {
                message = string.Format("'{0}' => {1}", callingMethodName, message);
            }

            this.MessengerInstance.Send<DisplayMessage>(new DisplayMessage(MessageType.Error, message));
        }
        #endregion

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
    }
}
