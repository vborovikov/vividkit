namespace Toolkit.ComponentModel
{
	using System;

	/// <summary>
	/// Provides canonical implementation of <see cref="IDisposable"/> interface.
	/// </summary>
	public abstract class Disposable : IDisposable
	{
		/// <summary>
		/// Gets a value that indicates whenever the object is disposed.
		/// </summary>
		protected bool IsDisposed { get; private set; }

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="Disposable"/> is reclaimed by garbage collection.
		/// </summary>
		~Disposable()
		{
			if (!this.IsDisposed)
			{
				this.IsDisposed = true;
				Dispose(false);
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (!this.IsDisposed)
			{
				this.IsDisposed = true;
				Dispose(true);
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposeManagedObjects"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		private void Dispose(bool disposeManagedObjects)
		{
			if (disposeManagedObjects)
				DisposeManagedObjects();
			DisposeUnmanagedObjects();
		}

		/// <summary>
		/// Releases managed resources.
		/// </summary>
		protected abstract void DisposeManagedObjects();

		/// <summary>
		/// Releases unmanaged resources.
		/// </summary>
		protected virtual void DisposeUnmanagedObjects()
		{
		}
	}
}
