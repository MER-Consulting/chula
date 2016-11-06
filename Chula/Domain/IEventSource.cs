using System.Collections.Generic;

namespace MER.Chula.Domain
{
    public interface IEventSourceWhereSequenceNumber
    {
        IEventSource GreaterThan(ulong sequenceNumber);
    }

    public interface IEventSourceWhereEventType
    {
        IEventSource Equals(NameVersionPair eventType);
        IEventSource In(ISet<NameVersionPair> eventTypes);
    }

    public interface IEventSourceWhereAggregateId
    {
        IEventSource Equals(string aggregateId);
    }

    public interface IEventSourceWhereAggregateType
    {
        IEventSource Equals(string aggregateType);
    }

    public interface IEventSourceWhere
    {
        IEventSourceWhereSequenceNumber SequenceNumber { get; }
        IEventSourceWhereEventType EventType { get; }
        IEventSourceWhereAggregateId AggregateId { get; }
        IEventSourceWhereAggregateType AggregateType { get; }
    }

    public interface IEventSource : IEnumerable<EventBase>
    {
        IEventSourceWhere Where { get; }
    }
}
