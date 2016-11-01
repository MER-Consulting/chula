using MER.Chula.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MER.Chula.Infrastructure
{
    class SimpleCacheEventStore : IEventStore, IDisposable
    {
        private ReaderWriterLockSlim rwLock;
        private IEventStore store;
        private Lazy<List<EventBase>> cache;

        public SimpleCacheEventStore(IEventStore eventStore = null)
        {
            this.rwLock = new ReaderWriterLockSlim();
            this.store = eventStore;
            this.cache = new Lazy<List<EventBase>>(this.CreateCache);
        }

        private List<EventBase> CreateCache()
        {
            return new List<EventBase>(this.store ?? Enumerable.Empty<EventBase>());
        }

        public IEnumerator<EventBase> GetEnumerator()
        {
            this.rwLock.EnterReadLock();

            try
            {
                foreach (var @event in this.cache.Value)
                    yield return @event;
            }
            finally
            {
                this.rwLock.ExitReadLock();
            }
        }

        public void WriteEvents(IEnumerable<EventBase> events)
        {
            if (events == null)
                throw new ArgumentNullException(nameof(events));

            var eventList = events.ToList();

            if (eventList.Count > 0)
            {
                this.rwLock.EnterWriteLock();

                try
                {
                    this.store.WriteEvents(eventList);
                    this.cache.Value.AddRange(eventList);
                }
                finally
                {
                    this.rwLock.ExitWriteLock();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Dispose()
        {
            if (this.rwLock != null)
            {
                this.rwLock.EnterWriteLock(); // Wait for other threads to finish.
                this.rwLock.ExitWriteLock();
                this.rwLock.Dispose();
                this.rwLock = null;
            }

            (this.store as IDisposable)?.Dispose();

            this.store = null;
            this.cache = null;
        }
    }
}
