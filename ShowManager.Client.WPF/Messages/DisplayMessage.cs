using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace ShowManager.Client.WPF.Messages
{
    /// <summary>
    /// Use this class to send a message with a text message.
    /// </summary>
    class DisplayMessage : MessageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMessage"/> class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="text"></param>
        public DisplayMessage(MessageType type, string text)
        {
            this.Type = type;
            this.Text = text;
        }

        /// <summary>
        /// Gets the Text value
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Gets the MessageType value
        /// </summary>
        public MessageType Type { get; private set; }
    }

    enum MessageType
    {
        Information,
        Warning,
        Error,
        Debug
    }
}
