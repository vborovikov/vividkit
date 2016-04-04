namespace Toolkit.RequestModel
{
	using System.Collections.Generic;

	public interface IEventHandlerProvider
	{
		IEnumerable<IEventHandler<TEvent>> GetHandlers<TEvent>() where TEvent : class, IEvent;

		IEnumerable<IAsyncEventHandler<TEvent>> GetAsyncHandlers<TEvent>() where TEvent : class, IEvent;
	}
}