using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Audit.Application.SaveAuditMessage;
using SFA.DAS.Audit.Types;
using SFA.DAS.Messaging;

namespace SFA.DAS.Audit.Processor.UnitTests.AuditMessageMonitorTests
{
    public class WhenRunningAndAMessageIsReceived
    {
        private CancellationTokenSource _tokenSource;
        private Mock<IEventingMessageReceiver<AuditMessage>> _messageReceiver;
        private AuditMessageMonitor _monitor;
        private Mock<IMediator> _mediator;
        private AuditMessage _message;

        [SetUp]
        public void Arrange()
        {
            _tokenSource = new CancellationTokenSource();
            _messageReceiver = new Mock<IEventingMessageReceiver<AuditMessage>>();
            _messageReceiver.Setup(r => r.RunAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.Factory.StartNew(async () =>
                {
                    while (!_tokenSource.Token.IsCancellationRequested)
                    {
                        await Task.Delay(1000);
                    }
                }, _tokenSource.Token));

            _mediator = new Mock<IMediator>();

            _monitor = new AuditMessageMonitor(_messageReceiver.Object, _mediator.Object);

            _message = new AuditMessage();
        }

        [Test]
        public async Task ThenItShouldSaveMessagesWhenTheyAreReceived()
        {
            // Arrange
            var monitorTask = _monitor.RunAsync(_tokenSource.Token);

            // Act
            _messageReceiver.Raise(r => r.MessageReceived += null, new MessageReceivedEventArgs<AuditMessage>(_message));

            _tokenSource.Cancel();
            await monitorTask;

            // Assert
            _mediator.Verify(m => m.SendAsync(It.Is<SaveAuditMessageCommand>(c => c.Message == _message)), Times.Once);
        }

        [Test]
        public async Task ThenItShouldSetTheMessageAsHandled()
        {
            // Arrange
            var monitorTask = _monitor.RunAsync(_tokenSource.Token);
            var receivedEventArgs = new MessageReceivedEventArgs<AuditMessage>(_message);

            // Act
            _messageReceiver.Raise(r => r.MessageReceived += null, receivedEventArgs);

            _tokenSource.Cancel();
            await monitorTask;

            // Assert
            Assert.IsTrue(receivedEventArgs.Handled);
        }
    }
}
