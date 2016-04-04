#define DEBUG

namespace Toolkit.RequestModel.Infrastructure
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class DebugEventDispatcher : IEventDispatcher
    {
        private readonly IEventDispatcher dispatcher;
        private readonly string dispatcherName;

        public DebugEventDispatcher(IEventDispatcher eventDispatcher)
        {
            this.dispatcher = eventDispatcher;
            this.dispatcherName = this.dispatcher.DiscoverDispatcherName();
        }

        public async Task PublishAsync<TEvent>(TEvent e) where TEvent : class, IEvent
        {
            var eventName = e.GetType().Name + "(" + e.GetHashCode() + ")";
            var stopwatch = new Stopwatch();
            try
            {
                Debug.WriteLine("{0} publishing {1}", this.dispatcherName, eventName);
                Debug.WriteLine("{0} {1} is {2}", this.dispatcherName, eventName, e.ToString());
                stopwatch.Start();

                await this.dispatcher.PublishAsync(e);
            }
            catch (Exception x)
            {
                Debug.WriteLine("{0} caught error: {1}", this.dispatcherName, x);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                Debug.WriteLine("{0} published {1} ({2} ms)",
                    this.dispatcherName, eventName, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}