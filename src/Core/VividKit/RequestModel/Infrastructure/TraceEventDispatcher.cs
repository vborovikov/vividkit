namespace Toolkit.RequestModel.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using static Toolkit.RequestModel.Infrastructure.RequestDispatcherEventSource;

    public class TraceEventDispatcher : IEventDispatcher
    {
        private readonly IEventDispatcher eventDispatcher;

        public TraceEventDispatcher(IEventDispatcher eventDispatcher)
        {
            this.eventDispatcher = eventDispatcher;
        }

        public async Task PublishAsync<TEvent>(TEvent e) where TEvent : class, IEvent
        {
            var dispatcherName = this.eventDispatcher.GetType().Name;
            var eventName = e.GetType().Name;

            try
            {
                Log.EventPublishing(dispatcherName, eventName);
                Log.EventData(dispatcherName, e.ToString());

                await this.eventDispatcher.PublishAsync(e);
            }
            catch (Exception x)
            {
                Log.EventError(dispatcherName, eventName, x.ToString());
                throw;
            }
            finally
            {
                Log.EventPublished(dispatcherName, eventName);
            }
        }
    }
}