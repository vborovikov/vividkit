namespace Toolkit.RequestModel
{
    using System;
    using System.Threading.Tasks;

    public interface ICommandDispatcher : IRequestDispatcher
    {
        Task ExecuteAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}