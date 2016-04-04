namespace Toolkit.RequestModel.Infrastructure
{
    using System;
    using System.Threading.Tasks;

    public class DefaultCommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public DefaultCommandDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task ExecuteAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var asyncCommandHandler = this.serviceProvider.GetService<IAsyncCommandHandler<TCommand>>();
            if (asyncCommandHandler != null)
            {
                await asyncCommandHandler.ExecuteAsync(command).ConfigureAwait(false);
            }
            else
            {
                var commandHandler = this.serviceProvider.GetService<ICommandHandler<TCommand>>();
                await Task.Run(delegate { commandHandler.Execute(command); }, command.CancellationToken).ConfigureAwait(false);
            }
        }
    }
}