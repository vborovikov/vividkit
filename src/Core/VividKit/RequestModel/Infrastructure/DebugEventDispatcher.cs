#define DEBUG

namespace Toolkit.RequestModel.Infrastructure
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class DebugEventDispatcher : IEventDispatcher
    {
        private readonly IEventDispatcher eventDispatcher;

        public DebugEventDispatcher(IEventDispatcher eventDispatcher)
        {
            this.eventDispatcher = eventDispatcher;
        }

        public async Task PublishAsync<TEvent>(TEvent e) where TEvent : class, IEvent
        {
            var dispatcherName = this.eventDispatcher.GetType().Name;
            var eventName = e.GetType().Name + "(" + e.GetHashCode() + ")";
            var stopwatch = new Stopwatch();
            try
            {
                Debug.WriteLine("{0} publishing {1}", dispatcherName, eventName);
                Debug.WriteLine("{0} {1} is {2}", dispatcherName, eventName, e.ToString());
                stopwatch.Start();

                await this.eventDispatcher.PublishAsync(e);
            }
            catch (Exception x)
            {
                Debug.WriteLine("{0} caught error: {1}", dispatcherName, x);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                Debug.WriteLine("{0} published {1} ({2} ms)",
                    dispatcherName, eventName, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}