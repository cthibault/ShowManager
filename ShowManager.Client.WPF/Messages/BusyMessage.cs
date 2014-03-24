using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace ShowManager.Client.WPF.Messages
{
    /// <summary>
    /// Use this class to send a message when the busy state of an application changes.
    /// </summary>
    public class BusyMessage : MessageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusyMessage"/> class.
        /// </summary>
        /// <param name="isBusy"><b>true</b> if the application is busy; otherwise, <b>false</b>.</param>
        /// <param name="userState">Optional. User state information to include with the message.</param>
        public BusyMessage(bool isBusy, object userState)
            : this(isBusy, userState, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusyMessage"/> class.
        /// </summary>
        /// <param name="isBusy"><b>true</b> if the application is busy; otherwise, <b>false</b>.</param>
        /// <param name="userState">Optional. User state information to include with the message.</param>
        /// <param name="sender">Optional. The message's original sender.</param>
        public BusyMessage(bool isBusy, object userState, object sender)
            : base(sender)
        {
            this.IsBusy = isBusy;
            this.UserState = userState;
        }

        /// <summary>
        /// Gets a value indicating whether the application is currently in a busy state.
        /// </summary>
        public bool IsBusy { get; private set; }

        /// <summary>
        /// Gets user state information to include with the message.
        /// </summary>
        public object UserState { get; private set; }
    }
}
