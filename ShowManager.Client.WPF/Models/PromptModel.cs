using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ShowManager.Client.WPF.Models
{
    class PromptModel : ObservableObject, IPromptModel
    {
        public PromptModel(string header, Action confirm, Action reject, bool isOpen = true)
        {
            this.Header = header;
            this.IsOpen = isOpen;

            this.Confirm = confirm;
            this.ConfirmCommand = new RelayCommand(this.Confirm);

            this.Reject = reject;
            this.RejectCommand = new RelayCommand(this.Reject);
        }

        #region Header
        public string Header { get; private set; } 
        #endregion

        #region IsOpen
        public bool IsOpen
        {
            get { return this._isOpen; }
            set { this.Set(() => this.IsOpen, ref this._isOpen, value); }
        }
        private bool _isOpen; 
        #endregion

        #region Confirm
        public Action Confirm { get; private set; }
        public ICommand ConfirmCommand { get; private set; } 
        #endregion

        #region Reject
        public Action Reject { get; private set; }
        public ICommand RejectCommand { get; private set; } 
        #endregion
    }

    class PromptModel<T> : ObservableObject, IPromptModel
    {
        public PromptModel(string header, Action<T> confirm, Action<T> reject) : this(header, confirm, reject, false) { }

        public PromptModel(string header, Action<T> confirm, Action<T> reject, bool isOpen)
        {
            this.Header = header;
            this.IsOpen = isOpen;

            this.Confirm = confirm;
            this.ConfirmCommand = new RelayCommand<T>(this.Confirm);

            this.Reject = reject;
            this.RejectCommand = new RelayCommand<T>(this.Reject);
        }

        #region Header
        public string Header { get; private set; }
        #endregion

        #region IsOpen
        public bool IsOpen
        {
            get { return this._isOpen; }
            set { this.Set(() => this.IsOpen, ref this._isOpen, value); }
        }
        private bool _isOpen;
        #endregion

        #region Confirm
        public Action<T> Confirm { get; private set; }
        public ICommand ConfirmCommand { get; private set; }
        #endregion

        #region Reject
        public Action<T> Reject { get; private set; }
        public ICommand RejectCommand { get; private set; }
        #endregion
    }
}
