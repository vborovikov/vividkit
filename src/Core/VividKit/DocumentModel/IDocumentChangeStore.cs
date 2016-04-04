namespace Toolkit.DocumentModel
{
    using System;
    using System.Collections.Generic;

    public interface IDocumentChangeStore
    {
        void StoreChanges(Guid documentId, IEnumerable<Edit> edits, int expectedVersion);

        IEnumerable<Edit> RestoreChanges(Guid documentId);
    }
}