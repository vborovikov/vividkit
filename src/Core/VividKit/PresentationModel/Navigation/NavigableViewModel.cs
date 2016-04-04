namespace Toolkit.PresentationModel.Navigation
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using Toolkit.ComponentModel;

	/// <summary>
	/// Supports view navigation and state persistence.
	/// </summary>
	public abstract partial class NavigableViewModel : ViewModel, INavigable, ITombstone
	{
		private readonly INavigationService navigationService;

		/// <summary>
		/// Initializes a new instance of the <see cref="NavigableViewModel" /> class.
		/// </summary>
		protected NavigableViewModel()
			: base()
		{
			this.navigationService = ViewModel.GetService<INavigationService>();
		}

		/// <summary>
		/// Gets a value that indicates whether there is at least one entry in back navigation history.
		/// </summary>
		/// <value>
		///		<c>true</c> if there is at least one entry in back navigation history; otherwise, <c>false</c>.
		/// </value>
		protected bool CanGoBack
		{
			get
			{
				return this.navigationService.CanGoBack;
			}
		}

		void INavigable.OnNavigatedFrom(object parameter)
		{
			OnNavigatedFrom();

			var commandManager = this.CommandManager as ICleanup;
			if (commandManager != null)
			{
				commandManager.Cleanup();
			}
		}

		void INavigable.OnNavigatedTo(object parameter)
		{
			OnNavigatedTo((bool)parameter);
		}

		/// <summary>
		/// Navigates to the most recent entry in back navigation history, if there is one.
		/// </summary>
		protected void GoBack()
		{
			this.navigationService.GoBack();
		}

		/// <summary>
		/// Navigates to the default view associated with the specified <see cref="T:NavigatorViewModel">view model type</see>.
		/// </summary>
		/// <param name="parameter">The parameter.</param>
		protected void Navigate<TViewModel>(object parameter)
			where TViewModel : NavigableViewModel
		{
			Navigate<TViewModel>(NavigableAttribute.DefaultTarget, parameter);
		}

		/// <summary>
		/// Navigates to a view associated with the specified <see cref="T:NavigatorViewModel">view model type</see>.
		/// </summary>
		/// <param name="target">The target view name.</param>
		/// <param name="parameter">The parameter.</param>
		protected void Navigate<TViewModel>(string target, object parameter)
			where TViewModel : NavigableViewModel
		{
			this.navigationService.Navigate(this, typeof(TViewModel), target, parameter);
		}

		/// <summary>
		/// Called when the View is navigated from.
		/// </summary>
		protected virtual void OnNavigatedFrom()
		{
		}

		/// <summary>
		/// Called when the View is navigated to.
		/// </summary>
		/// <param name="navigatedBackward">Indicates that the view is navigated backward.</param>
		protected virtual void OnNavigatedTo(bool navigatedBackward)
		{
		}

		/// <summary>
		/// Removes the most recent journal entry from back history.
		/// </summary>
		protected void RemoveBackEntry()
		{
			this.navigationService.RemoveBackEntry();
		}

		/// <summary>
		/// Removes all journal entries from back history.
		/// </summary>
		protected void RemoveBackHistory()
		{
			while (this.CanGoBack)
			{
				this.RemoveBackEntry();
			}
		}

		internal void RaiseBackKeyPress(CancelEventArgs args)
		{
			OnBackKeyPress(args);
		}

		/// <summary>
		/// Handles the back hardware key press event.
		/// </summary>
		/// <param name="args">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
		protected virtual void OnBackKeyPress(CancelEventArgs args)
		{
		}

		void ITombstone.OnSerializing(IDictionary<string, object> state)
		{
			OnSerializing(state);
		}

		/// <summary>
		/// Called when the <see cref="T:NavigatorViewModel" /> is serializing.
		/// </summary>
		/// <param name="state">The state.</param>
		protected abstract void OnSerializing(IDictionary<string, object> state);

		void ITombstone.OnDeserializing(IDictionary<string, object> state)
		{
			OnDeserializing(state);
		}

		/// <summary>
		/// Called when the <see cref="T:NavigatorViewModel" /> is deserializing.
		/// </summary>
		/// <param name="state">The state.</param>
		protected abstract void OnDeserializing(IDictionary<string, object> state);

	}
}
