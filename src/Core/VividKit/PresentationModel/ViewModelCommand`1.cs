namespace Toolkit.PresentationModel
{
	using System;

	/// <summary>
	/// Represents the <see cref="ViewModel"/> command which requires a parameter.
	/// </summary>
	/// <typeparam name="T">The command parameter type.</typeparam>
	sealed class ViewModelCommand<T> : ViewModelCommandBase
	{
		private readonly Predicate<T> canExecute;
		private readonly Action<T> execute;

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModelCommand&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="owner">The <see cref="ViewModel"/> that provides this command.</param>
		/// <param name="canExecute">The delegate that determines whether the command can be executed.</param>
		/// <param name="execute">The execution entry point for the command.</param>
		public ViewModelCommand(ViewModel owner, Predicate<T> canExecute, Action<T> execute)
			: base(owner)
		{
			this.canExecute = canExecute;
			this.execute = execute;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModelCommand&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="owner">The <see cref="ViewModel"/> that provides this command.</param>
		/// <param name="execute">The execution entry point for the command.</param>
		public ViewModelCommand(ViewModel owner, Action<T> execute) : this(owner, null, execute) { }

		/// <summary>
		/// When overridden in a derived class, defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		/// <returns>
		/// true if this command can be executed; otherwise, false.
		/// </returns>
		protected override bool CanExecuteOverride(object parameter)
		{
			return (parameter != null && parameter is T) && (this.canExecute == null || this.canExecute((T)parameter));
		}

		/// <summary>
		/// When overridden in a derived class, defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		protected override void ExecuteOverride(object parameter)
		{
			this.execute((T)parameter);
		}
	}
}
