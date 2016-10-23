using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MER.Chula.Domain
{
    public interface IEventStore : IEventSource
    {
        void WriteEvents(IEnumerable<EventBase> events);
    }
}
