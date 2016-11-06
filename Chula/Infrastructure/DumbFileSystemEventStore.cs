using MER.Chula.Domain;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MER.Chula.Infrastructure
{
    /// <summary>
    /// Very simple EventStore using append-only file for persistence.
    /// </summary>
    public sealed class DumbFileSystemEventStore : IEventStore, IDisposable
    {
        private ReaderWriterLockSlim fileLock = new ReaderWriterLockSlim();
        private FileInfo file;
        private JsonSerializerSettings serializerSettings;

        public IEventSourceWhere Where
        {
            get { return new SimpleEventSourceQueryable(this); }
        }

        public DumbFileSystemEventStore(string fileName, JsonSerializerSettings serializerSettings)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            if (serializerSettings == null)
                throw new ArgumentNullException(nameof(serializerSettings));

            this.file = new FileInfo(fileName);
            this.serializerSettings = serializerSettings;
        }

        public DumbFileSystemEventStore(string fileName) : this(fileName, new JsonSerializerSettings()) { }

        public IEnumerator<EventBase> GetEnumerator()
        {
            this.fileLock.EnterReadLock();
            try
            {
                using (var reader = this.file.OpenText())
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        yield return JsonConvert.DeserializeObject<EventBase>(line, this.serializerSettings);
                    }
                }
            }
            finally { this.fileLock.ExitReadLock(); }
        }

        public void WriteEvents(IEnumerable<EventBase> events)
        {
            this.fileLock.EnterWriteLock();
            try
            {
                using (var writer = this.file.AppendText())
                {
                    foreach (var @event in events)
                    {
                        string line = JsonConvert.SerializeObject(@event, this.serializerSettings);
                        writer.WriteLine(line);
                    }
                }
            }
            finally { this.fileLock.ExitWriteLock(); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Dispose()
        {
            if (this.fileLock != null)
            {
                this.fileLock.EnterWriteLock(); // Wait for other threads to finish.
                this.fileLock.ExitWriteLock();
                this.fileLock.Dispose();
                this.fileLock = null;
            }
        }
    }
}
