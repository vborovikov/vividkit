namespace Toolkit.RequestModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IRequestDispatcher : IDisposable
    {
        bool Wait(TimeSpan timeout);
    }
}