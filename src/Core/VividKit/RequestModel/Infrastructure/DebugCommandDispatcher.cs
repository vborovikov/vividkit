#define DEBUG

namespace Toolkit.RequestModel.Infrastructure
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class DebugCommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandDispatcher commandDispatcher;

        public DebugCommandDispatcher(ICommandDispatcher commandDispatcher)
        {
            this.commandDispatcher = commandDispatcher;
        }

        public async Task ExecuteAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var dispatcherName = this.commandDispatcher.GetType().Name;
            var commandName = command.GetType().Name + "(" + command.GetHashCode() + ")";
            var stopwatch = new Stopwatch();
            try
            {
                Debug.WriteLine("{0} executing {1}", dispatcherName, commandName);
                Debug.WriteLine("{0} {1} is {2}", dispatcherName, commandName, command.ToString());
                stopwatch.Start();

                await this.commandDispatcher.ExecuteAsync(command);
            }
            catch (Exception x)
            {
                Debug.WriteLine("{0} caught error: {1}", dispatcherName, x);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                Debug.WriteLine("{0} executed {1} ({2} ms)", dispatcherName,
                    commandName, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}