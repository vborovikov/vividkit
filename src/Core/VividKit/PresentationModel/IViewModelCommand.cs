namespace Toolkit.PresentationModel
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Represents the <see cref="ViewModel"/> command.
    /// </summary>
    public interface IViewModelCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the command short name.
        /// </summary>
        /// <value>
        /// The name of the command.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the command description.
        /// </summary>
        /// <value>
        /// The description text.
        /// </value>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the image <see cref="T:System.Uri"></see>.
        /// </summary>
        /// <value>
        /// The image URI.
        /// </value>
        Uri Image { get; set; }

        /// <summary>
        /// Gets or sets an arbitrary object value that can be used to store custom information about the command.
        /// </summary>
        /// <value>
        /// The custom object.
        /// </value>
        object Tag { get; set; }
    }
}