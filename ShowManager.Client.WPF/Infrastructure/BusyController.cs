using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ShowManager.Client.WPF.Messages;

namespace ShowManager.Client.WPF.Infrastructure
{
    /// <summary>
    /// Use this class to send a message when the busy state of an application changes
    /// </summary>
    class BusyController : ObservableObject
    {
        /// <summary>
        /// Initializes static members of the <see cref="BusyController"/> class.
        /// </summary>
        static BusyController()
        {
            _default = new BusyController();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusyController"/> class.
        /// </summary>
        public BusyController() : this(Messenger.Default) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="BusyController"/> class.
        /// </summary>
        /// <param name="messenger">Optional. An <see cref="IMessenger"/> which will be used to send messages.</param>
        public BusyController(IMessenger messenger)
        {
            this.MessengerInstance = messenger;
        }


        #region Default
        /// <summary>
        /// Gets the default busy controller instance
        /// </summary>
        public static BusyController Default
        {
            get { return _default; }
        }
        private static BusyController _default;
        #endregion

        #region MessengerInstance
        /// <summary>
        /// Gets the messenger instance used by the controller
        /// </summary>
        protected IMessenger MessengerInstance
        {
            get { return this._messengerInstance; }
            set
            {
                if (this._messengerInstance != value)
                {
                    this._messengerInstance = value;
                    this.RaisePropertyChanged(() => this.MessengerInstance);
                }
            }
        }
        private IMessenger _messengerInstance;
        #endregion

        #region IsBusy
        /// <summary>
        /// Gets a value indicating if the controller is in a busy state
        /// </summary>
        public bool IsBusy
        {
            get { return this._isBusy; }
            private set
            {
                if (this._isBusy != value)
                {
                    this._isBusy = value;
                    this.RaisePropertyChanged(() => this.IsBusy);
                }
            }
        }
        private bool _isBusy = false;
        #endregion

        #region Count
        private int _count;
        #endregion

        #region CallingMethodNames
        private List<Tuple<string, bool>> CallingMethodNames
        {
            get
            {
                if (this._callingMethodNames == null)
                {
                    this._callingMethodNames = new List<Tuple<string, bool>>();
                }
                return this._callingMethodNames;
            }
        }
        private List<Tuple<string, bool>> _callingMethodNames;
        #endregion

        #region Reset
        /// <summary>
        /// Resets the controller
        /// </summary>
        public void Reset()
        {
            lock (this._syncRoot)
            {
                this.IsBusy = false;
                this._count = 0;

                // TODO: Should this be done within the LOCK?
                if (this.MessengerInstance != null)
                {
                    // Send the message indicating that the controller is no longer busy
                    this.MessengerInstance.Send<BusyMessage>(new BusyMessage(this.IsBusy, null));
                }
            }
        }
        #endregion

        #region SendMessage
        /// <summary>
        /// Sends the message
        /// </summary>
        /// <param name="value"><b>true</b> if the application is busy; otherwise, <b>false</b>.</param>
        public void SendMessage(bool value, [CallerMemberName] string callingMethodName = null)
        {
            this.SendMessage(value, null, callingMethodName);
        }
        /// <summary>
        /// Sends the message
        /// </summary>
        /// <param name="value"><b>true</b> if the application is busy; otherwise, <b>false</b>.</param>
        /// <param name="userState">Optional.  User state data to include with the message if the controller transitions between busy states</param>
        public void SendMessage(bool value, object userState, [CallerMemberName] string callingMethodName = null)
        {
            lock (this._syncRoot)
            {
                bool send = false;

                if (callingMethodName != null)
                {
                    this.CallingMethodNames.Add(new Tuple<string, bool>(callingMethodName, value));
                }

                if (value)
                {
                    Interlocked.Increment(ref this._count);

                    if (this._count == 1)
                    {
                        this.IsBusy = true;

                        // the controller is now busy, send the message
                        send = true;
                    }
                }
                else
                {
                    Interlocked.Decrement(ref this._count);

                    if (this._count == 0)
                    {
                        this.IsBusy = false;

                        // the controller is no longer busy, send the message
                        send = true;
                    }
                }

                if (send && this.MessengerInstance != null)
                {
                    this.MessengerInstance.Send<BusyMessage>(new BusyMessage(this.IsBusy, userState, this));
                }
            }
        }
        #endregion

        #region Private Fields
        private readonly object _syncRoot = new object();
        #endregion
    }
}
