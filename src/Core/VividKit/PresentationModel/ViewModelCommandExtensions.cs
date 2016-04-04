namespace Toolkit.PresentationModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows.Input;

	public static class ViewModelCommandExtensions
	{
		public static bool TryExecute(this ICommand command, object parameter = null)
		{
			var canExecute = command.CanExecute(parameter);
			if (canExecute)
			{
				command.Execute(parameter);
			}

			return canExecute;
		}
	}
}
