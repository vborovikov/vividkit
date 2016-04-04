namespace Toolkit.RequestModel
{
	using System.Threading.Tasks;

	public interface IEventDispatcher
	{
		Task PublishAsync<TEvent>(TEvent e) where TEvent : class, IEvent;
	}
}
