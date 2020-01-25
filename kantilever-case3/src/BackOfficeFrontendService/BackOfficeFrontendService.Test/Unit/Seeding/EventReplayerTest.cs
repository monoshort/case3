using System;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Commands;
using BackOfficeFrontendService.Seeding;
using BackOfficeFrontendService.Seeding.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.TestBus;
using Moq;
using RabbitMQ.Client;

namespace BackOfficeFrontendService.Test.Unit.Seeding
{
    [TestClass]
    public class EventReplayerTest
    {
        [TestMethod]
        [DataRow("testTopic", typeof(DomainEvent))]
        [DataRow("topac", typeof(string))]
        public void ReplayEvents_CallsReplayEventsAsyncOnAuditAgent(string topic, Type type)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            Mock<IAuditAgent> auditAgentMock = new Mock<IAuditAgent>();
            IBusContext<IConnection> context = new TestBusContext();
            IEventReplayer eventReplayer =
                new EventReplayer(serviceCollection, auditAgentMock.Object, new NullLoggerFactory());

            auditAgentMock.Setup(e =>
                    e.ReplayEventsAsync(It.Is<ReplayEventsCommand>(c =>
                        c.EventType == type.Name && c.TopicFilter == topic)))
                .ReturnsAsync("0")
                .Verifiable();

            // Act
            eventReplayer.ReplayEvents(context, topic, type, DateTime.Now);

            // Assert
            auditAgentMock.Verify();
        }

        [TestMethod]
        [DataRow("testTopic", typeof(DomainEvent))]
        [DataRow("topac", typeof(string))]
        public void ReplayEvents_ExitsNormallyWithZeroEvents(string topic, Type type)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            Mock<IAuditAgent> auditAgentMock = new Mock<IAuditAgent>();
            IBusContext<IConnection> context = new TestBusContext();
            IEventReplayer eventReplayer =
                new EventReplayer(serviceCollection, auditAgentMock.Object, new NullLoggerFactory());

            auditAgentMock.Setup(e =>
                    e.ReplayEventsAsync(It.Is<ReplayEventsCommand>(c =>
                        c.EventType == type.Name && c.TopicFilter == topic)))
                .ReturnsAsync("0");

            // Act
            eventReplayer.ReplayEvents(context, topic, type, DateTime.Now);

            // Assert
            // No exception \o/
        }

        [TestMethod]
        [DataRow("testTopic", typeof(DomainEvent))]
        [DataRow("topac", typeof(string))]
        public void ReplayEvents_ThrowsTimeoutExceptionAfterTimeoutIfEventsDontReplay(string topic, Type type)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            Mock<IAuditAgent> auditAgentMock = new Mock<IAuditAgent>();
            IBusContext<IConnection> context = new TestBusContext();
            IEventReplayer eventReplayer =
                new EventReplayer(serviceCollection, auditAgentMock.Object, new NullLoggerFactory());

            auditAgentMock.Setup(e =>
                    e.ReplayEventsAsync(It.Is<ReplayEventsCommand>(c =>
                        c.EventType == type.Name && c.TopicFilter == topic)))
                .ReturnsAsync("1");

            // Act
            void Act() => eventReplayer.ReplayEvents(context, topic, type, DateTime.Now);

            // Assert
            TimeoutException exception = Assert.ThrowsException<TimeoutException>(Act);
            Assert.AreEqual($"Replaying 0/1 events took longer than {EventReplayer.TimeOut}ms", exception.Message);
        }
    }
}
