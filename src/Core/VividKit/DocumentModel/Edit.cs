namespace Toolkit.DocumentModel
{
    using System;
    using Toolkit.RequestModel;

    public abstract class Edit : EventBase
    {
        protected Edit(Guid documentId)
        {
            this.DocumentId = documentId;
            this.Created = DateTime.UtcNow;
        }

        public Guid DocumentId { get; private set; }

        public DateTime Created { get; private set; }
    }
}