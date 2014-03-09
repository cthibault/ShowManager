using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowManager.Client.WPF.Events
{
    public class BaseEvent<T> where T : EventArgs
    {
        public BaseEvent(T args)
        {
            this.Args = args;
        }

        public T Args { get; private set; }
    }
}
