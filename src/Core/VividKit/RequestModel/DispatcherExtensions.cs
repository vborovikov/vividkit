namespace Toolkit.RequestModel
{
    using System.Reflection;
    using System.Threading.Tasks;

    public static class DispatcherExtensions
    {
        private static readonly MethodInfo executeAsyncMethod = typeof(ICommandDispatcher).GetMethod("ExecuteAsync", BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo publishAsyncMethod = typeof(IEventDispatcher).GetMethod("PublishAsync", BindingFlags.Public | BindingFlags.Instance);

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
    }
}