#define DEBUG

namespace Toolkit.RequestModel.Infrastructure
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class DebugCommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandDispatcher dispatcher;
        private readonly string dispatcherName;

        public DebugCommandDispatcher(ICommandDispatcher commandDispatcher)
        {
            this.dispatcher = commandDispatcher;
            this.dispatcherName = this.dispatcher.DiscoverDispatcherName();
        }

        public async Task ExecuteAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var commandName = command.GetType().Name + "(" + command.GetHashCode() + ")";
            var stopwatch = new Stopwatch();
            try
            {
                Debug.WriteLine("{0} executing {1}", this.dispatcherName, commandName);
                Debug.WriteLine("{0} {1} is {2}", this.dispatcherName, commandName, command.ToString());
                stopwatch.Start();

                await this.dispatcher.ExecuteAsync(command);
            }
            catch (Exception x)
            {
                Debug.WriteLine("{0} caught error: {1}", this.dispatcherName, x);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                Debug.WriteLine("{0} executed {1} ({2} ms)", this.dispatcherName,
                    commandName, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}