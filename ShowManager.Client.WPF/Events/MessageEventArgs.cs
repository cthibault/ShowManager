using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowManager.Client.WPF.Events
{
    class MessageEventArgs : EventArgs
    {
        public MessageEventArgs()
            : base()
        {
            this.AffirmativeButtonText = "OK";
        }

        /// <summary>
        /// The Title text to display
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Message text to display
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// The text used for the Affirmative button.
        /// Default is "OK"
        /// </summary>
        /// <example>"OK" or "Yes"</example>
        public string AffirmativeButtonText { get; set; }

        /// <summary>
        /// The text used for the Negative button.
        /// </summary>
        /// <example>"Cancel" or "No"</example>
        public string NegativeButtonText { get; set; }
    }
}
