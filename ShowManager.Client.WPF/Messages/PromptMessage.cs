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
    /// Use this class to send a message with a text message.
    /// </summary>
    class PromptMessage : DisplayMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PromptMessage"/> class.
        /// Default Type is MessageType.Information
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="buttonConfiguration"></param>
        public PromptMessage(string title, string text, MessageBoxButton buttonConfiguration, Action<string> callback) 
            : this(title, text, buttonConfiguration, callback, MessageType.Information)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PromptMessage"/> class.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public PromptMessage(string title, string text, MessageBoxButton buttonConfiguration, Action<string> callback, MessageType type)
            : base(title, text, type)
        {
            this.ButtonConfiguration = buttonConfiguration;
            this.Callback = callback;
        }

        /// <summary>
        /// The Button Configuration to be used
        /// </summary>
        public MessageBoxButton ButtonConfiguration { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Action<string> Callback { get; private set; }
    }
}
