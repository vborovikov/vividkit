namespace Toolkit.RequestModel
{
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public static class DispatcherExtensions
    {
        private static readonly MethodInfo executeAsyncMethod =
#if PORTABLE
            typeof(ICommandDispatcher).GetTypeInfo().DeclaredMethods
                .Single(m => m.Name == "ExecuteAsync" && m.IsPublic && m.IsStatic == false);

#else
            typeof(ICommandDispatcher).GetMethod("ExecuteAsync", BindingFlags.Public | BindingFlags.Instance);
#endif

        private static readonly MethodInfo publishAsyncMethod =
#if PORTABLE
            typeof(ICommandDispatcher).GetTypeInfo().DeclaredMethods
                .Single(m => m.Name == "PublishAsync" && m.IsPublic && m.IsStatic == false);

#else
            typeof(IEventDispatcher).GetMethod("PublishAsync", BindingFlags.Public | BindingFlags.Instance);
#endif

        public static Task ExecuteGenericAsync(this ICommandDispatcher commandDispatcher, ICommand command)
        {
            var method = executeAsyncMethod.MakeGenericMethod(command.GetType());
            return (Task)method.Invoke(commandDispatcher, new[] { command });
        }

        public static Task PublishGenericAsync(this IEventDispatcher eventDispatcher, IEvent @event)
        {
            var method = publishAsyncMethod.MakeGenericMethod(@event.GetType());
            return (Task)method.Invoke(eventDispatcher, new[] { @event });
        }

        internal static string DiscoverDispatcherName(this object dispatcher)
        {
            var name = dispatcher.ToString();

            if (name == dispatcher.GetType().FullName)
            {
                name = dispatcher.GetType().Name;
            }

            return name;
        }
    }
}