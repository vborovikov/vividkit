namespace Toolkit.RequestModel
{
	using System;
	using System.Threading.Tasks;

	public interface IQueryDispatcher
	{
		TResult Run<TResult>(IQuery<TResult> query);

		Task<TResult> RunAsync<TResult>(IQuery<TResult> query);
	}
}