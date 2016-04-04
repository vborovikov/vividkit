namespace Toolkit.DocumentModel
{
    public interface IChangeApplying<TEdit>
        where TEdit : Edit
    {
        void Apply(TEdit e);
    }
}