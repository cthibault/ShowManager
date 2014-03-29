using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using ShowManager.Client.WPF.Enums;

namespace ShowManager.Client.WPF.Messages
{
    /// <summary>
    /// Use this class to send status message.
    /// </summary>
    class DisplayStatusMessage : DisplayMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayStatusMessage"/> class.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="type"></param>
        public DisplayStatusMessage(string text, MessageType type)
            : base(string.Empty, text, type)
        {

        }
    }
}
