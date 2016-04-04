namespace Toolkit.RequestModel.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using static Toolkit.RequestModel.Infrastructure.RequestDispatcherEventSource;

    public class TraceCommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandDispatcher dispatcher;
        private readonly string dispatcherName;

        public TraceCommandDispatcher(ICommandDispatcher commandDispatcher)
        {
            this.dispatcher = commandDispatcher;
            this.dispatcherName = this.dispatcher.DiscoverDispatcherName();
        }

        public async Task ExecuteAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var commandName = command.GetType().Name;

            try
            {
                Log.CommandExecuting(this.dispatcherName, commandName);
                Log.CommandData(this.dispatcherName, command.ToString());

                await this.dispatcher.ExecuteAsync(command);
            }
            catch (Exception x)
            {
                Log.CommandError(this.dispatcherName, commandName, x.ToString());
                throw;
            }
            finally
            {
                Log.CommandExecuted(this.dispatcherName, commandName);
            }
        }
    }
}