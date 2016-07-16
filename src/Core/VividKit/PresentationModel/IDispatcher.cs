namespace Toolkit.PresentationModel
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides services for managing the queue of work items for a thread.
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Executes the specified delegate asynchronously on the thread the <see cref="T:IDispatcher"/> is associated with.
        /// </summary>
        /// <param name="action">
        /// A delegate to a method that takes no arguments and does not return a value,
        /// which is pushed onto the <see cref="T:IDispatcher"/> event queue.
        /// </param>
        /// <returns>
        /// A <see cref="T:Task"/> that represents the operation
        /// that has been posted to the <see cref="T:IDispatcher"/> queue.
        /// </returns>
        Task InvokeAsync(Action action);

        /// <summary>
        /// Executes the specified delegate asynchronously on the thread the <see cref="T:IDispatcher"/> is associated with.
        /// </summary>
        /// <param name="action">
        /// A delegate to a method that takes no arguments and does not return a value,
        /// which is pushed onto the <see cref="T:IDispatcher"/> event queue.
        /// </param>
        /// <returns>
        /// A <see cref="T:Task"/> that represents the operation
        /// that has been posted to the <see cref="T:IDispatcher"/> queue.
        /// </returns>
        Task InvokeTaskAsync(Func<Task> action);

        /// <summary>
        /// Executes the specified <see cref="T:Func{TResult}"/> asynchronously
        /// on the thread the Dispatcher is associated with.
        /// </summary>
        /// <typeparam name="TResult">The return value type of the specified delegate.</typeparam>
        /// <param name="callback">A delegate to invoke through the dispatcher.</param>
        /// <returns>
        /// A <see cref="T:Task{TResult}"/> that represents the operation
        /// that has been posted to the <see cref="T:IDispatcher"/> queue.
        /// </returns>
        Task<TResult> InvokeAsync<TResult>(Func<TResult> callback);

        /// <summary>
        /// Executes the specified <see cref="T:Func{TResult}"/> asynchronously
        /// on the thread the Dispatcher is associated with.
        /// </summary>
        /// <typeparam name="TResult">The return value type of the specified delegate.</typeparam>
        /// <param name="callback">A delegate to invoke through the dispatcher.</param>
        /// <returns>
        /// A <see cref="T:Task{TResult}"/> that represents the operation
        /// that has been posted to the <see cref="T:IDispatcher"/> queue.
        /// </returns>
        Task<TResult> InvokeTaskAsync<TResult>(Func<Task<TResult>> callback);
    }
}