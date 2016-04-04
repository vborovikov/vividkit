namespace Toolkit.ComponentModel
{
    using System;
    using System.Reflection;

    // Read this for more info: http://www.thomaslevesque.com/2010/05/17/c-a-simple-implementation-of-the-weakevent-pattern/
    // And this: http://diditwith.net/CommentView,guid,aacdb8ae-7baa-4423-a953-c18c1c7940ab.aspx

    internal class DefaultWeakEventHandler<T> : IWeakEventHandler
        where T : class
    {
        private delegate void OpenEventHandler(T @this, object sender, EventArgs e);

        private readonly WeakReference targetReference;
        private readonly MethodInfo method;
        private readonly OpenEventHandler openEventHandler;

        public DefaultWeakEventHandler(EventHandler eventHandler)
        {
            if (eventHandler.Target != null)
            {
                this.targetReference = new WeakReference(eventHandler.Target);
            }
            this.method = eventHandler.GetMethodInfo();
            this.openEventHandler = (OpenEventHandler)eventHandler.GetMethodInfo().CreateDelegate(typeof(OpenEventHandler));
        }

        public bool IsAlive
        {
            get { return this.targetReference == null || this.targetReference.IsAlive; }
        }

        public bool Equals(EventHandler other)
        {
            return other != null &&
                   other.Target == this.targetReference.Target &&
                   other.GetMethodInfo().Equals(this.method);
        }

        public void Invoke(object sender, EventArgs e)
        {
            if (this.targetReference != null)
            {
                var target = this.targetReference.Target as T;
                if (target != null)
                {
                    this.openEventHandler(target, sender, e);
                }
            }
            else
            {
                var handler = (EventHandler)this.method.CreateDelegate(typeof(EventHandler));
                handler(sender, e);
            }
        }
    }
}