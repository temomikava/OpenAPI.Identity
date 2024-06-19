
using MassTransit;

namespace OpenAPI.Identity
{
    public class IntegrationEventService : IIntegrationEventService
    {
        private readonly IBus eventBus;
        private readonly List<IIntegrationEvent> events = new List<IIntegrationEvent>();

        public IntegrationEventService(IBus eventBus)
        {
            this.eventBus = eventBus;
        }
        public Task AddEventAsync(BaseIntegrationEvent @event)
        {
            events.Add(@event);
            return Task.CompletedTask;
        }

        public async Task PublishEventsAsync(Guid correlationId, CancellationToken token)
        {
            foreach (var @event in events)
            {
                await eventBus.Publish(@event, @event.GetType(), c =>
                {
                    c.CorrelationId = correlationId;
                }, token);
            }
        }
    }
}
