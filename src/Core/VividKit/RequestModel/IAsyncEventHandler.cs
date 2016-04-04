namespace Toolkit.RequestModel
{
	using System.Threading.Tasks;

	public interface IAsyncEventHandler<TEvent>
		where TEvent : IEvent
	{
		Task HandleAsync(TEvent e);
	}
}
