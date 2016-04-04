namespace Toolkit.RequestModel.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using static Toolkit.RequestModel.Infrastructure.RequestDispatcherEventSource;

    public class TraceQueryDispatcher : IQueryDispatcher
    {
        private readonly IQueryDispatcher dispatcher;
        private readonly string dispatcherName;

        public TraceQueryDispatcher(IQueryDispatcher queryDispatcher)
        {
            this.dispatcher = queryDispatcher;
            this.dispatcherName = this.dispatcher.DiscoverDispatcherName();
        }

        public TResult Run<TResult>(IQuery<TResult> query)
        {
            var queryName = query.GetType().Name;

            try
            {
                Log.QueryRunning(this.dispatcherName, queryName);
                Log.QueryData(this.dispatcherName, query.ToString());

                return this.dispatcher.Run(query);
            }
            catch (Exception x)
            {
                Log.QueryError(this.dispatcherName, queryName, x.ToString());
                throw;
            }
            finally
            {
                Log.QueryRan(this.dispatcherName, queryName);
            }
        }

        public async Task<TResult> RunAsync<TResult>(IQuery<TResult> query)
        {
            var queryName = query.GetType().Name;

            try
            {
                Log.QueryRunning(this.dispatcherName, queryName);
                Log.QueryData(this.dispatcherName, query.ToString());

                return await this.dispatcher.RunAsync(query);
            }
            catch (Exception x)
            {
                Log.QueryError(this.dispatcherName, queryName, x.ToString());
                throw;
            }
            finally
            {
                Log.QueryRan(this.dispatcherName, queryName);
            }
        }
    }
}