namespace Toolkit.RequestModel
{
    public abstract class QueryBase<TResult> : RequestBase, IQuery<TResult>
    {
        protected QueryBase()
        {
        }
    }
}