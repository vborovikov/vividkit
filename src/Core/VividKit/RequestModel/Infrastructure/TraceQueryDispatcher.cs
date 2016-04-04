namespace Toolkit.RequestModel.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using static Toolkit.RequestModel.Infrastructure.RequestDispatcherEventSource;

    public class TraceQueryDispatcher : IQueryDispatcher
    {
        private readonly IQueryDispatcher queryDispatcher;

        public TraceQueryDispatcher(IQueryDispatcher queryDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
        }

        public TResult Run<TResult>(IQuery<TResult> query)
        {
            var dispatcherName = this.queryDispatcher.GetType().Name;
            var queryName = query.GetType().Name;

            try
            {
                Log.QueryRunning(dispatcherName, queryName);
                Log.QueryData(dispatcherName, query.ToString());

                return this.queryDispatcher.Run(query);
            }
            catch (Exception x)
            {
                Log.QueryError(dispatcherName, queryName, x.ToString());
                throw;
            }
            finally
            {
                Log.QueryRan(dispatcherName, queryName);
            }
        }

        public async Task<TResult> RunAsync<TResult>(IQuery<TResult> query)
        {
            var dispatcherName = this.queryDispatcher.GetType().Name;
            var queryName = query.GetType().Name;

            try
            {
                Log.QueryRunning(dispatcherName, queryName);
                Log.QueryData(dispatcherName, query.ToString());

                return await this.queryDispatcher.RunAsync(query);
            }
            catch (Exception x)
            {
                Log.QueryError(dispatcherName, queryName, x.ToString());
                throw;
            }
            finally
            {
                Log.QueryRan(dispatcherName, queryName);
            }
        }
    }
}