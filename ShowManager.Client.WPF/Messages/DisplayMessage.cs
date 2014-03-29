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
    class DisplayMessage : MessageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMessage"/> class.
        /// Default Type is MessageType.Information
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public DisplayMessage(string title, string text)
            : this(title, text, MessageType.Information)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMessage"/> class.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="type"></param>
        public DisplayMessage(string title, string text, MessageType type)
        {
            this.Title = title;
            this.Text = text;
            this.Type = type;
        }

        /// <summary>
        /// The Title text to display
        /// </summary>
        public string Title { get; protected set; }

        /// <summary>
        /// Gets the Text value
        /// </summary>
        public string Text { get; protected set; }

        /// <summary>
        /// Gets the MessageType value
        /// </summary>
        public MessageType Type { get; protected set; }
    }
}
