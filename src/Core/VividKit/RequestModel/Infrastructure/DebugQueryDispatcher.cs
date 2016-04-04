#define DEBUG

namespace Toolkit.RequestModel.Infrastructure
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class DebugQueryDispatcher : IQueryDispatcher
    {
        private readonly IQueryDispatcher queryDispatcher;

        public DebugQueryDispatcher(IQueryDispatcher queryDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
        }

        public TResult Run<TResult>(IQuery<TResult> query)
        {
            var dispatcherName = this.queryDispatcher.GetType().Name;
            var queryName = query.GetType().Name + "(" + query.GetHashCode() + ")";
            var stopwatch = new Stopwatch();
            try
            {
                Debug.WriteLine("{0} running {1}", dispatcherName, queryName);
                Debug.WriteLine("{0} {1} is {2}", dispatcherName, queryName, query.ToString());
                stopwatch.Start();

                return this.queryDispatcher.Run(query);
            }
            catch (Exception x)
            {
                Debug.WriteLine("{0} caught error: {1}", dispatcherName, x);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                Debug.WriteLine("{0} ran {1} ({2} ms)", dispatcherName,
                    queryName, stopwatch.ElapsedMilliseconds);
            }
        }

        public async Task<TResult> RunAsync<TResult>(IQuery<TResult> query)
        {
            var dispatcherName = this.queryDispatcher.GetType().Name;
            var queryName = query.GetType().Name + "(" + query.GetHashCode() + ")";
            var stopwatch = new Stopwatch();
            try
            {
                Debug.WriteLine("{0} running {1}", dispatcherName, queryName);
                Debug.WriteLine("{0} {1} is {2}", dispatcherName, queryName, query.ToString());
                stopwatch.Start();

                return await this.queryDispatcher.RunAsync(query);
            }
            catch (Exception x)
            {
                Debug.WriteLine("{0} caught error: {1}", dispatcherName, x);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                Debug.WriteLine("{0} ran {1} ({2} ms)", dispatcherName,
                    queryName, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}