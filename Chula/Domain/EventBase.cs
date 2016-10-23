using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MER.Chula.Domain
{
    public abstract class EventBase
    {
        public ulong SequenceNumber { get; }

        public NameVersionPair EventType { get; }

        public string AggregateType { get; }

        public string AggregateId { get; }

        public string Data { get; }

    }
}
