namespace Toolkit.RequestModel.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using ComponentModel;

    public abstract class RequestDispatcherBase : Disposable, IRequestDispatcher
    {
        private readonly EventWaitHandle waitHandle;

        protected RequestDispatcherBase()
        {
            this.waitHandle = new ManualResetEvent(true);
        }

        public bool Wait(TimeSpan timeout)
        {
            return this.waitHandle.WaitOne(timeout);
        }

        protected override void DisposeManagedObjects()
        {
            this.waitHandle.Dispose();
        }

        protected void ResetEvent()
        {
            this.waitHandle.Reset();
        }

        protected void SetEvent()
        {
            this.waitHandle.Set();
        }
    }
}