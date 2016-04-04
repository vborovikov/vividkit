namespace Toolkit.RequestModel.Infrastructure
{
    using System.Linq;
    using System.Threading.Tasks;

    public class DefaultEventDispatcher : IEventDispatcher
    {
        private readonly IEventHandlerProvider handlerProvider;

        public DefaultEventDispatcher(IEventHandlerProvider handlerProvider)
        {
            this.handlerProvider = handlerProvider;
        }

        public Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            var syncEventHandlerTasks = from eventHandler in this.handlerProvider.GetHandlers<TEvent>()
                                        select Task.Run(delegate { eventHandler.Handle(@event); });

            var asyncEventHandlersTasks = from eventHandler in this.handlerProvider.GetAsyncHandlers<TEvent>()
                                          select eventHandler.HandleAsync(@event);

            return Task.WhenAll(syncEventHandlerTasks.Concat(asyncEventHandlersTasks));
        }
    }
}