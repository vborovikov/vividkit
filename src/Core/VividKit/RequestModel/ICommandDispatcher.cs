namespace Toolkit.RequestModel
{
	using System;
	using System.Threading.Tasks;

	public interface ICommandDispatcher
	{
		Task ExecuteAsync<TCommand>(TCommand command) where TCommand : class, ICommand;
	}
}