namespace Toolkit.RequestModel.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using static Toolkit.RequestModel.Infrastructure.RequestDispatcherEventSource;

    public class TraceCommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandDispatcher commandDispatcher;

        public TraceCommandDispatcher(ICommandDispatcher commandDispatcher)
        {
            this.commandDispatcher = commandDispatcher;
        }

        public async Task ExecuteAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var dispatcherName = this.commandDispatcher.GetType().Name;
            var commandName = command.GetType().Name;

            try
            {
                Log.CommandExecuting(dispatcherName, commandName);
                Log.CommandData(dispatcherName, command.ToString());

                await this.commandDispatcher.ExecuteAsync(command);
            }
            catch (Exception x)
            {
                Log.CommandError(dispatcherName, commandName, x.ToString());
                throw;
            }
            finally
            {
                Log.CommandExecuted(dispatcherName, commandName);
            }
        }
    }
}