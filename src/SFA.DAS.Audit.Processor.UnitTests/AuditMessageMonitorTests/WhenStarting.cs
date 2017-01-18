using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Audit.Types;
using SFA.DAS.Messaging;

namespace SFA.DAS.Audit.Processor.UnitTests.AuditMessageMonitorTests
{
    public class WhenStarting
    {
        private Mock<IEventingMessageReceiver<AuditMessage>> _messageReceiver;
        private Mock<IMediator> _mediator;
        private AuditMessageMonitor _monitor;

        [SetUp]
        public void Arrange()
        {
            _messageReceiver = new Mock<IEventingMessageReceiver<AuditMessage>>();
            _messageReceiver.Setup(r => r.RunAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<object>(null));

            _mediator = new Mock<IMediator>();

            _monitor = new AuditMessageMonitor(_messageReceiver.Object, _mediator.Object);
        }

        [Test]
        public async Task ThenItShouldRunTheMessageReceiver()
        {
            // Arrange
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            // Act
            await _monitor.RunAsync(token);

            // Assert
            _messageReceiver.Verify(r => r.RunAsync(token), Times.Once);
        }
    }
}
