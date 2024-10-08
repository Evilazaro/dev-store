using DevStore.Core.Messages;
using System;

namespace DevStore.Orders.API.Application.Events
{
    public class OrderDoneEvent : Event
    {
        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }

        public OrderDoneEvent(Guid orderId, Guid customerId)
        {
            OrderId = orderId;
            CustomerId = customerId;
        }
    }
}