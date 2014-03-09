using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowManager.Client.WPF.Infrastructure
{
    public class EventPublisher : IEventPublisher
    {
        public IObservable<TEvent> GetEvent<TEvent>()
        {
            var subject = (ISubject<TEvent>)subjects.GetOrAdd(typeof(TEvent), t => new Subject<TEvent>());

            return subject.AsObservable();
        }

        public void Publish<TEvent>(TEvent anEvent)
        {
            object subject = null;

            if (subjects.TryGetValue(typeof(TEvent), out subject))
            {
                ((ISubject<TEvent>)subject).OnNext(anEvent);
            }
        }

        public async Task PublishAsync<TEvent>(TEvent anEvent)
        {
            object subject = null;

            if (subjects.TryGetValue(typeof(TEvent), out subject))
            {
                ((ISubject<TEvent>)subject).OnNext(anEvent);
            }
        }

        #region Private Fields
        private readonly ConcurrentDictionary<Type, object> subjects = new ConcurrentDictionary<Type, object>();
        #endregion
    }
}
