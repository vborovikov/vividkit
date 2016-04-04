namespace Toolkit.ComponentModel
{
	using System;

	public interface ICommandManager
	{
		event EventHandler RequerySuggested;

		void InvalidateRequerySuggested();
	}
}
