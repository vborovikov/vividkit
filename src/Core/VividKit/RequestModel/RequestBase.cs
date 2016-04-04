namespace Toolkit.RequestModel
{
    using System.Threading;

    public abstract class RequestBase : IRequest
    {
        protected RequestBase()
        {
            this.CancellationToken = CancellationToken.None;
        }

        public CancellationToken CancellationToken { get; set; }
    }
}