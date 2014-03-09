using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowManager.Client.WPF.Infrastructure
{
    public interface IEventPublisher
    {
        IObservable<TEvent> GetEvent<TEvent>();

        void Publish<TEvent>(TEvent anEvent);
        Task PublishAsync<TEvent>(TEvent anEvent);
    }
}
