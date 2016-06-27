namespace Toolkit.PresentationModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Threading;
    using Toolkit.ComponentModel;

    /// <summary>
    /// Abstracts the View and serves in data binding between the View and the Model.
    /// </summary>
    public abstract class ViewModel : Disposable, INotifyPropertyChanged
    {
        private class BusyMonitor : Disposable
        {
            private readonly ViewModel owner;

            internal BusyMonitor(ViewModel owner, string status)
            {
                this.owner = owner;

                var getBusy = false;
                if (Interlocked.CompareExchange(ref this.owner.busyCounter, 1, 0) == 0)
                {
                    getBusy = true;
                }
                else
                {
                    Interlocked.Increment(ref this.owner.busyCounter);
                }

                if (getBusy || (String.IsNullOrWhiteSpace(status) == false))
                {
                    this.owner.Dispatcher.InvokeAsync(delegate
                    {
                        if (getBusy)
                        {
                            this.owner.IsBusy = true;
                        }
                        if (String.IsNullOrWhiteSpace(status) == false)
                        {
                            this.owner.BusyStatus = status;
                        }
                    });
                }
            }

            protected override void DisposeManagedObjects()
            {
                if (Interlocked.Decrement(ref this.owner.busyCounter) == 0)
                {
                    this.owner.Dispatcher.InvokeAsync(delegate
                    {
                        this.owner.IsBusy = false;
                        this.owner.BusyStatus = null;
                    });
                }
            }
        }

        private static IServiceProvider serviceProvider;

        private readonly Dictionary<Delegate, IViewModelCommand> commands = new Dictionary<Delegate, IViewModelCommand>();
        private int busyCounter;
        private string busyStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        protected ViewModel()
        {
            this.Dispatcher = ViewModel.GetService<IDispatcher>();
            this.CommandManager = ViewModel.GetService<ICommandManager>();
        }

        public bool IsBusy
        {
            get
            {
                return this.busyCounter != 0;
            }
            private set
            {
                RaisePropertyChanged(() => this.IsBusy);
                this.CommandManager.InvalidateRequerySuggested();
            }
        }

        public string BusyStatus
        {
            get
            {
                return this.busyStatus;
            }
            private set
            {
                if (this.busyStatus != value)
                {
                    this.busyStatus = value;
                    RaisePropertyChanged(() => this.BusyStatus);
                }
            }
        }

        protected internal ICommandManager CommandManager { get; private set; }

        protected IDispatcher Dispatcher { get; private set; }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        internal static void RegisterServiceProvider(IServiceProvider serviceProvider)
        {
            if (ViewModel.serviceProvider != null || serviceProvider == null)
            {
                throw new InvalidOperationException();
            }

            ViewModel.serviceProvider = serviceProvider;
        }

        protected static TService GetService<TService>()
                    where TService : class
        {
            if (serviceProvider == null)
            {
                throw new InvalidOperationException();
            }

            return serviceProvider.GetService(typeof(TService)) as TService;
        }

        /// <summary>
        /// Gets the <see cref="ViewModelCommand"/> for the specified <see cref="Action"/> delegate.
        /// </summary>
        /// <param name="execute">The execution entry point for the command.</param>
        /// <param name="canExecute">The delegate that determines whenever the command can be executed.</param>
        /// <param name="setup">The delegate that sets the command properties.</param>
        /// <returns>Returns the command object.</returns>
        protected IViewModelCommand GetCommand(Action execute, Func<bool> canExecute = null, Action<IViewModelCommand> setup = null)
        {
            IViewModelCommand command = null;
            if (this.commands.TryGetValue(execute, out command))
                return command;

            command = this.CommandManager.CreateCommand(execute, canExecute);
            this.commands.Add(execute, command);
            if (setup != null)
                setup(command);

            return command;
        }

        /// <summary>
        /// Gets the <see cref="ViewModelCommand{T}"/> for the specified <see cref="Action{T}"/> delegate.
        /// </summary>
        /// <typeparam name="T">The command parameter type.</typeparam>
        /// <param name="execute">The execution entry point for the command.</param>
        /// <param name="canExecute">The delegate that determines whenever the command can be executed.</param>
        /// <param name="setup">The delegate that sets the command properties.</param>
        /// <returns>Returns the command object</returns>
        protected IViewModelCommand GetCommand<T>(Action<T> execute, Func<T, bool> canExecute = null, Action<IViewModelCommand> setup = null)
        {
            IViewModelCommand command = null;
            if (this.commands.TryGetValue(execute, out command))
                return command;

            command = this.CommandManager.CreateCommand(execute, canExecute);
            this.commands.Add(execute, command);
            if (setup != null)
                setup(command);

            return command;
        }

        protected IDisposable StayingBusy(string status = null)
        {
            return new BusyMonitor(this, status);
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="args">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyManager.RaisePropertyChanged(this, this.PropertyChanged, args);
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="propertyExpression">The strongly typed lambda expression of the property.</param>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            OnPropertyChanged(PropertyManager.GetPropertyChangedEventArgs(this, propertyExpression));
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void RaisePropertyChanged(string propertyName = null)
        {
            if (propertyName == null)
                propertyName = String.Empty;

            OnPropertyChanged(PropertyManager.GetPropertyChangedEventArgs(this, propertyName));
        }

        /// <summary>
        /// Releases managed resources.
        /// </summary>
        protected override void DisposeManagedObjects()
        {
            foreach (var command in this.commands)
                command.Value.Dispose();

            this.commands.Clear();
        }
    }
}