using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Interfaces;

namespace Ecommerce.Dispatcher
{
    public class EventDispatcher
    {
        private readonly Dictionary<Type, List<object>> _handlers = new Dictionary<Type, List<object>>();

        public void RegisterHandler<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            if (!_handlers.ContainsKey(eventType))
                _handlers[eventType] = new List<object>();
            _handlers[eventType].Add(handler);
        }

        public void Dispatch<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            if (_handlers.ContainsKey(eventType))
            {
                foreach (var handlerObj in _handlers[eventType])
                {
                    var handler = handlerObj as IEventHandler<TEvent>;
                    handler?.Handle(@event);
                }
            }
        }
    }
}
