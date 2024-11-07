using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Interfaces
{
    public interface IEvent { }

    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        void Handle(TEvent @event);
    }
}
