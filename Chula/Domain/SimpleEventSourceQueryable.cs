using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MER.Chula.Domain
{
    public sealed class SimpleEventSourceQueryable : IEventSource, IEventSourceWhere, IEventSourceWhereAggregateId,
        IEventSourceWhereAggregateType, IEventSourceWhereEventType, IEventSourceWhereSequenceNumber
    {
        private IEnumerable<EventBase> eventStream;

        public SimpleEventSourceQueryable(IEnumerable<EventBase> eventStream)
        {
            if (eventStream == null)
                throw new ArgumentNullException(nameof(eventStream));

            this.eventStream = eventStream;
        }

        IEventSourceWhere IEventSource.Where { get { return this; } }

        IEventSourceWhereAggregateId IEventSourceWhere.AggregateId { get { return this; } }

        IEventSourceWhereAggregateType IEventSourceWhere.AggregateType { get { return this; } }

        IEventSourceWhereEventType IEventSourceWhere.EventType { get { return this; } }

        IEventSourceWhereSequenceNumber IEventSourceWhere.SequenceNumber { get { return this; } }

        IEventSource IEventSourceWhereEventType.Equals(NameVersionPair eventType)
        {
            return new SimpleEventSourceQueryable(this.eventStream.Where(e => e.EventType.Equals(eventType)));
        }

        IEventSource IEventSourceWhereAggregateType.Equals(string aggregateType)
        {
            return new SimpleEventSourceQueryable(this.eventStream.Where(e => e.AggregateType.Equals(aggregateType)));
        }

        IEventSource IEventSourceWhereAggregateId.Equals(string aggregateId)
        {
            return new SimpleEventSourceQueryable(this.eventStream.Where(e => e.AggregateId.Equals(aggregateId)));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.eventStream.GetEnumerator();
        }

        IEnumerator<EventBase> IEnumerable<EventBase>.GetEnumerator()
        {
            return this.eventStream.GetEnumerator();
        }

        IEventSource IEventSourceWhereSequenceNumber.GreaterThan(ulong sequenceNumber)
        {
            return new SimpleEventSourceQueryable(this.eventStream.Where(e => e.SequenceNumber > sequenceNumber));
        }

        IEventSource IEventSourceWhereEventType.In(ISet<NameVersionPair> eventTypes)
        {
            return new SimpleEventSourceQueryable(this.eventStream.Where(e => eventTypes.Contains(e.EventType)));
        }
    }
}
