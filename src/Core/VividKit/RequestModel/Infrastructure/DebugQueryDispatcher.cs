#define DEBUG

namespace Toolkit.RequestModel.Infrastructure
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class DebugQueryDispatcher : IQueryDispatcher
    {
        private readonly IQueryDispatcher dispatcher;
        private readonly string dispatcherName;

        public DebugQueryDispatcher(IQueryDispatcher queryDispatcher)
        {
            this.dispatcher = queryDispatcher;
            this.dispatcherName = this.dispatcher.DiscoverDispatcherName();
        }

        public TResult Run<TResult>(IQuery<TResult> query)
        {
            var queryName = query.GetType().Name + "(" + query.GetHashCode() + ")";
            var stopwatch = new Stopwatch();
            try
            {
                Debug.WriteLine("{0} running {1}", this.dispatcherName, queryName);
                Debug.WriteLine("{0} {1} is {2}", this.dispatcherName, queryName, query.ToString());
                stopwatch.Start();

                return this.dispatcher.Run(query);
            }
            catch (Exception x)
            {
                Debug.WriteLine("{0} caught error: {1}", this.dispatcherName, x);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                Debug.WriteLine("{0} ran {1} ({2} ms)", this.dispatcherName,
                    queryName, stopwatch.ElapsedMilliseconds);
            }
        }

        public async Task<TResult> RunAsync<TResult>(IQuery<TResult> query)
        {
            var queryName = query.GetType().Name + "(" + query.GetHashCode() + ")";
            var stopwatch = new Stopwatch();
            try
            {
                Debug.WriteLine("{0} running {1}", this.dispatcherName, queryName);
                Debug.WriteLine("{0} {1} is {2}", this.dispatcherName, queryName, query.ToString());
                stopwatch.Start();

                return await this.dispatcher.RunAsync(query);
            }
            catch (Exception x)
            {
                Debug.WriteLine("{0} caught error: {1}", this.dispatcherName, x);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                Debug.WriteLine("{0} ran {1} ({2} ms)", this.dispatcherName,
                    queryName, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}