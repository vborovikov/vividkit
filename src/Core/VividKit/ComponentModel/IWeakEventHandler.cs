namespace Toolkit.ComponentModel
{
	using System;

	public interface IWeakEventHandler : IEquatable<EventHandler>
	{
		bool IsAlive { get; }

		void Invoke(object sender, EventArgs e);
	}
}