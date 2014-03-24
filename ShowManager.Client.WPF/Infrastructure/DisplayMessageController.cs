using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace ShowManager.Client.WPF.Infrastructure
{
    class DisplayMessageController : ObservableObject
    {
        #region Public Methods
        public void Add(string message)
        {
            this.DisplayMessageStack.Push(message);
            this.Update();
        }

        public void Clear()
        {
            this.DisplayMessageStack.Clear();
            this.Update();
        }

        public void RemoveCurrent()
        {
            this.DisplayMessageStack.Pop();
            this.Update();
        }
        #endregion

        #region Private Methods
        private void Update()
        {
            this.RaisePropertyChanged(() => this.HasCurrentMessage);
            this.RaisePropertyChanged(() => this.CurrentMessage);
        } 
        #endregion


        #region Public Properties
        public bool HasCurrentMessage
        {
            get { return this.DisplayMessageStack.Any(); }
        }

        public string CurrentMessage
        {
            get
            {
                var message = string.Empty;

                if (this.HasCurrentMessage)
                {
                    message = this.DisplayMessageStack.Peek();
                }

                return message;
            }
        }
        #endregion

        #region Private Properties
        private Stack<string> DisplayMessageStack
        {
            get
            {
                if (this._displayMessageStack == null)
                {
                    this._displayMessageStack = new Stack<string>();
                }
                return this._displayMessageStack;
            }
        }
        private Stack<string> _displayMessageStack;
        #endregion
    }
}
