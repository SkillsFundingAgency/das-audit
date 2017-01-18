using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Audit.Application.SaveAuditMessage;
using SFA.DAS.Audit.Domain;
using SFA.DAS.Messaging;

namespace SFA.DAS.Audit.Processor.UnitTests.AuditMessageMonitorTests
{
    public class WhenRunningAndAMessageIsReceived
    {
        private CancellationTokenSource _tokenSource;
        private Mock<IEventingMessageReceiver<AuditMessage>> _messageReceiver;
        private AuditMessageMonitor _monitor;
        private Mock<ILogger> _logger;
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

            _logger = new Mock<ILogger>();

            _monitor = new AuditMessageMonitor(_messageReceiver.Object, _mediator.Object, _logger.Object);

            _message = new AuditMessage();
        }

        [Test]
        public async Task ThenItShouldSaveMessagesWhenTheyAreReceived()
        {
            // Arrange
            var monitorTask = _monitor.RunAsync(_tokenSource.Token);

            // Act
            _messageReceiver.Raise(r => r.MessageReceived += null, new MessageReceivedEventArgs<AuditMessage>(_message));

            await Task.Delay(100);
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

            await Task.Delay(100);
            _tokenSource.Cancel();
            await monitorTask;

            // Assert
            Assert.IsTrue(receivedEventArgs.Handled);
        }

        [Test]
        public async Task ThenItShouldNotHandleTheMessageIfSavingTheMessageFails()
        {
            // Arrange
            _mediator.Setup(m => m.SendAsync(It.IsAny<SaveAuditMessageCommand>()))
                .Throws(new Exception("Unit tests"));

            var monitorTask = _monitor.RunAsync(_tokenSource.Token);
            var receivedEventArgs = new MessageReceivedEventArgs<AuditMessage>(_message);

            // Act
            _messageReceiver.Raise(r => r.MessageReceived += null, receivedEventArgs);

            await Task.Delay(100);
            _tokenSource.Cancel();
            await monitorTask;

            // Assert
            Assert.IsFalse(receivedEventArgs.Handled);
        }
    }
}
