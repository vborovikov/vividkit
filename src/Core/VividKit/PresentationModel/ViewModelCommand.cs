﻿namespace Toolkit.PresentationModel
{
    using System;
    using ComponentModel;

    /// <summary>
    /// Represents the <see cref="ViewModel"/> command which requires a parameter.
    /// </summary>
    public class ViewModelCommand : ViewModelCommandBase
    {
        private readonly Func<bool> canExecute;
        private readonly Action execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelCommand"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="ICommandManager"/> that created this command.</param>
        /// <param name="execute">The execution entry point for the command.</param>
        /// <param name="canExecute">The delegate that determines whether the command can be executed.</param>
        public ViewModelCommand(ICommandManager owner, Action execute, Func<bool> canExecute)
            : base(owner)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelCommand"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="ICommandManager"/> that created this command.</param>
        /// <param name="execute">The execution entry point for the command.</param>
        public ViewModelCommand(ICommandManager owner, Action execute)
            : this(owner, execute, null)
        {
        }

        /// <summary>
        /// When overridden in a derived class, defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        protected override bool CanExecuteOverride(object parameter)
        {
            return this.canExecute == null || this.canExecute();
        }

        /// <summary>
        /// When overridden in a derived class, defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        protected override void ExecuteOverride(object parameter)
        {
            this.execute();
        }

        /// <summary>
        /// Releases managed resources.
        /// </summary>
        protected override void DisposeManagedObjects()
        {
        }
    }
}