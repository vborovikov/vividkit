namespace Toolkit.DocumentModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.ExceptionServices;

    public abstract class Document : IRevertibleChangeTracking
    {
        public const int NoVersion = -1;

        private static readonly MethodInfo applyChangeMethod =
            typeof(Document).GetMethod("ApplyChangeInternal", BindingFlags.NonPublic | BindingFlags.Static);

        private readonly List<Edit> changes;

        protected Document()
        {
            this.changes = new List<Edit>();
            this.Version = NoVersion;
            this.IsActive = true;
        }

        public abstract Guid Id { get; }

        public int Version { get; private set; }

        public bool IsActive { get; protected set; }

        public bool IsChanged { get { return this.changes.Any(); } }

        public void Load(IEnumerable<Edit> history)
        {
            foreach (var @event in history)
            {
                ApplyChange(@event, fromHistory: true);
            }
        }

        public IEnumerable<Edit> GetRecentChanges()
        {
            return this.changes.ToArray();
        }

        public void AcceptChanges()
        {
            this.Version += this.changes.Count;
            this.changes.Clear();
        }

        public void RejectChanges()
        {
            this.changes.Clear();
        }

        protected void ApplyChange(Edit edit)
        {
            ApplyChange(edit, fromHistory: false);
        }

        private static void ApplyChangeInternal<TEdit>(TEdit edit, Document that)
            where TEdit : Edit
        {
            var applicator = that as IChangeApplying<TEdit>;
            if (applicator == null)
            {
                throw new NotImplementedException();
            }

            applicator.Apply(edit);
        }

        private void ApplyChange(Edit edit, bool fromHistory)
        {
            try
            {
                var applyChange = applyChangeMethod.MakeGenericMethod(edit.GetType());
                applyChange.Invoke(null, new object[] { edit, this });

                if (fromHistory == false)
                {
                    this.changes.Add(edit);
                }
                else
                {
                    this.Version += 1;
                }
            }
            catch (TargetInvocationException tix)
            {
                if (tix.InnerException != null)
                {
                    ExceptionDispatchInfo.Capture(tix.InnerException).Throw();
                }
            }
        }
    }
}