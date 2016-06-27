﻿namespace Toolkit.ComponentModel
{
    using System;
    using PresentationModel;

    public interface ICommandManager
    {
        event EventHandler RequerySuggested;

        void InvalidateRequerySuggested();

        IViewModelCommand CreateCommand(Action execute, Func<bool> canExecute);

        IViewModelCommand CreateCommand<T>(Action<T> execute, Func<T, bool> canExecute);
    }
}