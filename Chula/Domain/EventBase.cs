using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MER.Chula.Domain
{
    public abstract class EventBase
    {
        public abstract ulong SequenceNumber { get; }

        public abstract NameVersionPair EventType { get; }

        public abstract string AggregateType { get; }

        public abstract string AggregateId { get; }

        public abstract string Data { get; }

    }
}
