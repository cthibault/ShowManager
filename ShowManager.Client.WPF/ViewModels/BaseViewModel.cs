using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Microsoft.Practices.Unity;
using ShowManager.Client.WPF.Enums;
using ShowManager.Client.WPF.Extensions;
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
            this.SendErrorMessage(message, null, callingMethodName);
        }
        protected void SendErrorMessage(string message, Exception exception, [CallerMemberName] string callingMethodName = null)
        {
            if (callingMethodName != null)
            {
                message = string.Format("'{0}' => {1}", callingMethodName, message);
            } 

            if (exception != null)
            {
                message += "\r\n" + exception.ExtractExceptionMessage();
            }

            if (exception != null && System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }

            var eventArgs = new DisplayStatusMessage(message, MessageType.Error);

            this.MessengerInstance.Send<DisplayStatusMessage>(eventArgs);
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
