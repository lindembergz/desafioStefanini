using Ecommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Interfaces;

namespace Ecommerce.Events
{
    public class PurchaseCompletedEvent : IEvent
    {
        public Cart Cart { get; }
        public PurchaseCompletedEvent(Cart cart)
        {
            Cart = cart;
        }
    }
}
