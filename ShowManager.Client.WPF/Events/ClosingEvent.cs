using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowManager.Client.WPF.Events
{
    public class ClosingEvent : BaseEvent<CancelEventArgs>
    {
        public ClosingEvent(CancelEventArgs args)
            : base(args)
        {

        }
    }
}
