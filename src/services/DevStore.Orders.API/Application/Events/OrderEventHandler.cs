using DevStore.Core.Messages.Integration;
using DevStore.MessageBus;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DevStore.Orders.API.Application.Events
{
    public class OrderEventHandler : INotificationHandler<OrderDoneEvent>
    {
        private readonly IMessageBus _bus;

        public OrderEventHandler(IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(OrderDoneEvent message, CancellationToken cancellationToken)
        {
            await _bus.PublishAsync(new OrderDoneIntegrationEvent(message.CustomerId));
        }
    }
}