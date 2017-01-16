using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Audit.Application.QueueAuditMessage;
using SFA.DAS.Audit.Application.Validation;
using SFA.DAS.Messaging;

namespace SFA.DAS.Audit.Application.UnitTests.QueueAuditMessage.QueueAuditMessageCommandHandlerTests
{
    public class WhenHandlingQueueAuditMessageCommand
    {
        private Mock<IValidator<QueueAuditMessageCommand>> _validator;
        private Mock<IMessagePublisher> _messagePublisher;
        private QueueAuditMessageCommandHandler _handler;
        private QueueAuditMessageCommand _command;

        [SetUp]
        public void Arrange()
        {
            _validator = new Mock<IValidator<QueueAuditMessageCommand>>();
            _validator.Setup(v => v.ValidateAsync(It.IsAny<QueueAuditMessageCommand>()))
                .ReturnsAsync(new ValidationResult(null));

            _messagePublisher = new Mock<IMessagePublisher>();

            _handler = new QueueAuditMessageCommandHandler(_validator.Object, _messagePublisher.Object);

            _command = new QueueAuditMessageCommand();
        }

        [Test]
        public async Task ThenItShouldPushlishAuditMessage()
        {
            // Act
            await _handler.Handle(_command);

            // Assert
            _messagePublisher.Verify(p => p.PublishAsync(_command), Times.Once);
        }

        [Test]
        public void ThenItShouldThrowAnInvalidRequestMessageIfValidatorReturnInvalidResult()
        {
            // Arrange
            _validator.Setup(v => v.ValidateAsync(It.IsAny<QueueAuditMessageCommand>()))
                .ReturnsAsync(new ValidationResult(new[]
                {
                    new ValidationError { Property = "Message.Description", Description = "Description is required" }
                }));

            // Act + Assert
            var ex = Assert.ThrowsAsync<InvalidRequestException>(async () => await _handler.Handle(_command));
            Assert.AreEqual("Message.Description", ex.Errors[0].Property);
            Assert.AreEqual("Description is required", ex.Errors[0].Description);
        }
    }
}
