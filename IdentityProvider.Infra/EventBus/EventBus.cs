using System;
using System.Threading.Tasks;
using IdentityProvider.Bus;
using MassTransit;

namespace IdentityProvider.Infra.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly IBusControl _busControl;

        public EventBus(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public Task Publish<T>(T @event) where T : IntegrationEvent
        {
            throw new NotImplementedException();
        }

        public Task Send<T>(T command, string queue) where T : CommandEvent
        {
            throw new NotImplementedException();
        }
    }
}
