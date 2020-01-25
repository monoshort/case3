using System;
using BestelService.Constants;
using BestelService.Core.Repositories;
using BestelService.Events;
using BestelService.Seeding;
using BestelService.Seeding.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy;
using Minor.Miffy.TestBus;
using Moq;
using RabbitMQ.Client;

namespace BestelService.Test.Unit.Seeding
{
    [TestClass]
    public class DatabaseCacherTest
    {
        private static readonly ILoggerFactory Logger = new LoggerFactory();

        [TestMethod]
        public void EnsureKlanten_DoesNothingIfKlantenExist()
        {
            // Arrange
            var klantRepoMock = new Mock<IKlantRepository>();
            var eventReplayer = new Mock<IEventReplayer>();

            klantRepoMock.Setup(e => e.IsEmpty()).Returns(false);

            DatabaseCacher databasecacher = new DatabaseCacher(eventReplayer.Object, klantRepoMock.Object, Logger);

            // Act
            databasecacher.EnsureKlanten(null);

            // Assert
            eventReplayer.Verify(e =>
                e.ReplayEvents(It.IsAny<IBusContext<IConnection>>(), It.IsAny<string>(), It.IsAny<Type>(), It.IsAny<DateTime>()), Times.Never);
        }

        [TestMethod]
        public void EnsureKlanten_CallsReplayEventsWithProperValues()
        {
            // Arrange
            var klantRepoMock = new Mock<IKlantRepository>();
            var eventReplayer = new Mock<IEventReplayer>();

            klantRepoMock.Setup(e => e.IsEmpty()).Returns(true);

            DatabaseCacher databasecacher = new DatabaseCacher(eventReplayer.Object, klantRepoMock.Object, Logger);

            IBusContext<IConnection> context = new TestBusContext();

            // Act
            databasecacher.EnsureKlanten(context);

            // Assert
            eventReplayer.Verify(e => e.ReplayEvents(context, TopicNames.NieuweKlantAangemaakt, typeof(NieuweKlantAangemaaktEvent), It.IsAny<DateTime>()));
        }
    }
}
