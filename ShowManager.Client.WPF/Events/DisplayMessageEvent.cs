using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowManager.Client.WPF.Events
{
    class DisplayMessageEvent : BaseEvent<MessageEventArgs>
    {
        public DisplayMessageEvent(MessageEventArgs args)
            : base(args)
        {
        }
    }
}
