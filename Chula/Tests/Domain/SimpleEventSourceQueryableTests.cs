using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System.Collections.Generic;
using System.Linq;

namespace MER.Chula.Domain.Tests
{
    [TestClass()]
    public class SimpleEventSourceQueryableTests
    {
        [TestMethod()]
        public void WhereAggregateIdEqualsTest()
        {
            // Arrange
            var eventStream = new[] { "AId1", "AId2" }.Select(
                aggId =>
                {
                    var mock = MockRepository.GenerateStub<EventBase>();
                    mock.Stub(e => e.AggregateId).Return(aggId);
                    return mock;
                });

            IEventSource target = new SimpleEventSourceQueryable(eventStream);

            // Act
            var actual = target.Where.AggregateId.Equals("AId1");

            // Assert
            Assert.IsTrue(actual.All(e => e.AggregateId.Equals("AId1")));
        }

        [TestMethod()]
        public void WhereAggregateTypeEqualsTest()
        {
            // Arrange
            var eventStream = new[] { "AT1", "AT2" }.Select(
                aggType =>
                {
                    var mock = MockRepository.GenerateStub<EventBase>();
                    mock.Stub(e => e.AggregateType).Return(aggType);
                    return mock;
                });

            IEventSource target = new SimpleEventSourceQueryable(eventStream);

            // Act
            var actual = target.Where.AggregateType.Equals("AT1");

            // Assert
            Assert.IsTrue(actual.All(e => e.AggregateType.Equals("AT1")));
        }

        [TestMethod()]
        public void WhereEventTypeEqualsTest()
        {
            // Arrange
            var eventStream = new[] { "ET-1", "ET-2" }.Select(
                eventType =>
                {
                    var mock = MockRepository.GenerateStub<EventBase>();
                    mock.Stub(e => e.EventType).Return(NameVersionPair.Parse(eventType));
                    return mock;
                });

            IEventSource target = new SimpleEventSourceQueryable(eventStream);

            // Act
            var actual = target.Where.EventType.Equals(NameVersionPair.Parse("ET-1"));

            // Assert
            Assert.IsTrue(actual.All(e => e.EventType.Equals(NameVersionPair.Parse("ET-1"))));
        }

        [TestMethod()]
        public void WhereEventTypeInTest()
        {
            // Arrange
            var eventStream = new[] { "ET-1", "ET-2" }.Select(
                eventType =>
                {
                    var mock = MockRepository.GenerateStub<EventBase>();
                    mock.Stub(e => e.EventType).Return(NameVersionPair.Parse(eventType));
                    return mock;
                });

            IEventSource target = new SimpleEventSourceQueryable(eventStream);

            var nameVersionPairs = new HashSet<NameVersionPair>
            {
                NameVersionPair.Parse("ET-2"),
                NameVersionPair.Parse("ET-3")
            };

            // Act
            var actual = target.Where.EventType.In(nameVersionPairs);

            // Assert
            Assert.IsTrue(actual.All(e => e.EventType.Equals(NameVersionPair.Parse("ET-2"))));
        }

        [TestMethod()]
        public void WhereSequenceNumberGreaterThanTest()
        {
            // Arrange
            var eventStream = Enumerable.Range(1, 3).Select(
                seqNo =>
                {
                    var mock = MockRepository.GenerateStub<EventBase>();
                    mock.Stub(e => e.SequenceNumber).Return((ulong)seqNo);
                    return mock;
                });

            IEventSource target = new SimpleEventSourceQueryable(eventStream);

            // Act
            var actual = target.Where.SequenceNumber.GreaterThan(2);

            // Assert
            Assert.IsTrue(actual.All(e => e.SequenceNumber > 2));
        }
    }
}