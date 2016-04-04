namespace Toolkit.PresentationModel
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using Toolkit.ComponentModel;

    /// <summary>
    /// Represents the <see cref="ViewModel"/> command.
    /// </summary>
    internal abstract class ViewModelCommandBase : IViewModelCommand, INotifyPropertyChanged
    {
        private readonly ViewModel owner;
        private string name;
        private string text;
        private Uri image;
        private object tag;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelCommandBase"/> class.
        /// </summary>
        /// <param name="owner">The owner of the command.</param>
        protected ViewModelCommandBase(ViewModel owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="ViewModelCommandBase"/> class from being created.
        /// </summary>
        private ViewModelCommandBase()
        {
        }

        /// <summary>
        /// Gets or sets the command short name.
        /// </summary>
        /// <value>
        /// The name of the command.
        /// </value>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; RaisePropertyChanged(() => this.Name); }
        }

        /// <summary>
        /// Gets or sets the command description.
        /// </summary>
        /// <value>
        /// The description text.
        /// </value>
        public string Text
        {
            get { return this.text; }
            set { this.text = value; RaisePropertyChanged(() => this.Text); }
        }

        /// <summary>
        /// Gets or sets the image <see cref="T:System.Uri"></see>.
        /// </summary>
        /// <value>
        /// The image URI.
        /// </value>
        public Uri Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                RaisePropertyChanged(() => this.Image);
            }
        }

        /// <summary>
        /// Gets or sets an arbitrary object value that can be used to store custom information about the command.
        /// </summary>
        /// <value>
        /// The custom object.
        /// </value>
        public object Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                this.tag = value;
                RaisePropertyChanged(() => this.Tag);
            }
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { this.owner.CommandManager.RequerySuggested += value; }
            remove { this.owner.CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return CanExecuteOverride(parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            ExecuteCore(parameter);
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="propertyExpression">The strongly typed lambda expression of the property.</param>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            PropertyManager.RaisePropertyChanged(this, PropertyChanged, propertyExpression);
        }

        /// <summary>
        /// When overridden in a derived class, defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        protected abstract bool CanExecuteOverride(object parameter);

        /// <summary>
        /// When overridden in a derived class, defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        protected abstract void ExecuteOverride(object parameter);

        private void ExecuteCore(object parameter)
        {
            try
            {
                ExecuteOverride(parameter);
            }
            catch (Exception exception)
            {
                this.owner.NotifyErrorOccured(exception, this, parameter);
            }
        }
    }
}