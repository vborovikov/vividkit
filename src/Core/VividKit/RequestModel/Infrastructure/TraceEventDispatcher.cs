namespace Toolkit.RequestModel.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using static Toolkit.RequestModel.Infrastructure.RequestDispatcherEventSource;

    public class TraceEventDispatcher : IEventDispatcher
    {
        private readonly IEventDispatcher dispatcher;
        private readonly string dispatcherName;

        public TraceEventDispatcher(IEventDispatcher eventDispatcher)
        {
            this.dispatcher = eventDispatcher;
            this.dispatcherName = this.dispatcher.DiscoverDispatcherName();
        }

        public async Task PublishAsync<TEvent>(TEvent e) where TEvent : class, IEvent
        {
            var eventName = e.GetType().Name;

            try
            {
                Log.EventPublishing(this.dispatcherName, eventName);
                Log.EventData(this.dispatcherName, e.ToString());

                await this.dispatcher.PublishAsync(e);
            }
            catch (Exception x)
            {
                Log.EventError(this.dispatcherName, eventName, x.ToString());
                throw;
            }
            finally
            {
                Log.EventPublished(this.dispatcherName, eventName);
            }
        }
    }
}