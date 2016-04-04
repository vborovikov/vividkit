namespace Toolkit.RequestModel.Infrastructure
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    public class DefaultQueryDispatcher : IQueryDispatcher
    {
        private readonly MethodInfo runAsyncMethod;
        private readonly MethodInfo runMethod;
        private readonly IServiceProvider serviceProvider;

        public DefaultQueryDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.runMethod = GetType().GetTypeInfo().GetDeclaredMethod("RunInternal");
            this.runAsyncMethod = GetType().GetTypeInfo().GetDeclaredMethod("RunAsyncInternal");
        }

        public TResult Run<TResult>(IQuery<TResult> query)
        {
            var queryHandlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var queryHandler = this.serviceProvider.GetService(queryHandlerType);

            var method = this.runMethod.MakeGenericMethod(query.GetType(), typeof(TResult));
            return (TResult)method.Invoke(this, new[] { query, queryHandler });
        }

        public async Task<TResult> RunAsync<TResult>(IQuery<TResult> query)
        {
            var result = default(TResult);

            var queryHandlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var queryHandler = this.serviceProvider.GetService(queryHandlerType);
            if (queryHandler != null)
            {
                var method = this.runMethod.MakeGenericMethod(query.GetType(), typeof(TResult));
                result = await Task.Run(() => (TResult)method.Invoke(this, new[] { query, queryHandler }),
                    query.CancellationToken).ConfigureAwait(false);
            }
            else
            {
                var asyncQueryHandlerType = typeof(IAsyncQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
                var asyncQueryHandler = this.serviceProvider.GetService(asyncQueryHandlerType);

                var asyncMethod = this.runAsyncMethod.MakeGenericMethod(query.GetType(), typeof(TResult));
                result = await ((Task<TResult>)asyncMethod.Invoke(this, new[] { query, asyncQueryHandler })).ConfigureAwait(false);
            }

            return result;
        }

        public Task<TResult> RunAsyncInternal<TQuery, TResult>(TQuery query, IAsyncQueryHandler<TQuery, TResult> handler) where TQuery : IQuery<TResult>
        {
            var resultTask = handler.RunAsync(query);
            return resultTask;
        }

        public TResult RunInternal<TQuery, TResult>(TQuery query, IQueryHandler<TQuery, TResult> handler) where TQuery : IQuery<TResult>
        {
            var result = handler.Run(query);
            return result;
        }
    }
}