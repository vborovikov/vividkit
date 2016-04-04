namespace Toolkit.RequestModel.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using System.Threading.Tasks;

    internal sealed class RequestDispatcherEventSource : EventSource
    {
        public static readonly RequestDispatcherEventSource Log = new RequestDispatcherEventSource();

        private RequestDispatcherEventSource() : base()
        {
        }

        [Event(1, Level = EventLevel.Informational, Opcode = EventOpcode.Start)]
        public void CommandExecuting(string dispatcherName, string commandName)
        {
            WriteEvent(1, dispatcherName, commandName);
        }

        [Event(2, Level = EventLevel.Informational, Opcode = EventOpcode.Stop)]
        public void CommandExecuted(string dispatcherName, string commandName)
        {
            WriteEvent(2, dispatcherName, commandName);
        }

        [Event(3, Level = EventLevel.Verbose, Opcode = EventOpcode.Info)]
        public void CommandData(string dispatcherName, string command)
        {
            WriteEvent(3, dispatcherName, command);
        }

        [Event(4, Level = EventLevel.Error, Opcode = EventOpcode.Info)]
        public void CommandError(string dispatcherName, string command, string exception)
        {
            WriteEvent(4, dispatcherName, command, exception);
        }

        [Event(5, Level = EventLevel.Informational, Opcode = EventOpcode.Start)]
        public void QueryRunning(string dispatcherName, string queryName)
        {
            WriteEvent(5, dispatcherName, queryName);
        }

        [Event(6, Level = EventLevel.Informational, Opcode = EventOpcode.Stop)]
        public void QueryRan(string dispatcherName, string queryName)
        {
            WriteEvent(6, dispatcherName, queryName);
        }

        [Event(7, Level = EventLevel.Verbose, Opcode = EventOpcode.Info)]
        public void QueryData(string dispatcherName, string query)
        {
            WriteEvent(7, dispatcherName, query);
        }

        [Event(8, Level = EventLevel.Error, Opcode = EventOpcode.Info)]
        public void QueryError(string dispatcherName, string query, string exception)
        {
            WriteEvent(8, dispatcherName, query, exception);
        }

        [Event(9, Level = EventLevel.Informational, Opcode = EventOpcode.Start)]
        public void EventPublishing(string dispatcherName, string eventName)
        {
            WriteEvent(9, dispatcherName, eventName);
        }

        [Event(10, Level = EventLevel.Informational, Opcode = EventOpcode.Stop)]
        public void EventPublished(string dispatcherName, string eventName)
        {
            WriteEvent(10, dispatcherName, eventName);
        }

        [Event(11, Level = EventLevel.Verbose, Opcode = EventOpcode.Info)]
        public new void EventData(string dispatcherName, string @event)
        {
            WriteEvent(11, dispatcherName, @event);
        }

        [Event(12, Level = EventLevel.Error, Opcode = EventOpcode.Info)]
        public void EventError(string dispatcherName, string @event, string exception)
        {
            WriteEvent(12, dispatcherName, @event, exception);
        }
    }
}