namespace Toolkit.PresentationModel
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using ComponentModel;

    public class DefaultCommandManager : ICommandManager
    {
        private readonly IDispatcher dispatcher;
        private readonly List<IWeakEventHandler> eventHandlers;
        private object dispatcherOperation;

        public DefaultCommandManager(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            this.eventHandlers = new List<IWeakEventHandler>();
        }

        event EventHandler ICommandManager.RequerySuggested
        {
            add
            {
                this.eventHandlers.Add(CreateWeakEventHandler(value));
            }
            remove
            {
                Cleanup(value);
            }
        }

        void ICommandManager.InvalidateRequerySuggested()
        {
            if (this.dispatcherOperation == null)
            {
                this.dispatcherOperation = this.dispatcher.InvokeAsync(InvokeEventHandlers);
            }
        }

        public IViewModelCommand CreateCommand(Action execute, Func<bool> canExecute)
        {
            return new ViewModelCommand(this, execute, canExecute);
        }

        public IViewModelCommand CreateCommand<T>(Action<T> execute, Func<T, bool> canExecute)
        {
            return new ViewModelCommand<T>(this, execute, canExecute);
        }

        protected virtual IWeakEventHandler CreateWeakEventHandler(EventHandler eventHandler)
        {
            var wehType = typeof(DefaultWeakEventHandler<>).MakeGenericType(eventHandler.GetMethodInfo().DeclaringType);
            var wehConstructor = wehType.GetConstructor(new[] { typeof(EventHandler) });
            var weh = (IWeakEventHandler)wehConstructor.Invoke(new[] { eventHandler });

            return weh;
        }

        private void Cleanup(EventHandler value)
        {
            for (var i = this.eventHandlers.Count - 1; i >= 0; --i)
            {
                var eventHandler = this.eventHandlers[i];
                if (eventHandler.IsAlive == false || eventHandler.Equals(value))
                {
                    this.eventHandlers.RemoveAt(i);
                }
            }
        }

        private void InvokeEventHandlers()
        {
            this.dispatcherOperation = null;

            foreach (var eventHandler in this.eventHandlers)
            {
                eventHandler.Invoke(null, EventArgs.Empty);
            }

            Cleanup(null);
        }
    }
}