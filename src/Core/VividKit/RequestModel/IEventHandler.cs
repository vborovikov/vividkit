namespace Toolkit.RequestModel
{
	public interface IEventHandler<TEvent>
		where TEvent : IEvent
	{
		void Handle(TEvent e);
	}
}