using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Audit.Application.SaveAuditMessage;
using SFA.DAS.Audit.Domain;
using SFA.DAS.Audit.Domain.Data;

namespace SFA.DAS.Audit.Application.UnitTests.SaveAuditMessage.SaveAuditMessageCommandHandlerTests
{
    public class WhenHandlingSaveAuditMessageCommand
    {
        private Mock<IWritableAuditRepository> _auditRepository;
        private SaveAuditMessageCommandHandler _handler;
        private SaveAuditMessageCommand _command;
        private AuditMessage _auditMessage;

        [SetUp]
        public void Arrange()
        {
            _auditRepository = new Mock<IWritableAuditRepository>();
            _auditRepository.Setup(r => r.StoreAsync(It.IsAny<AuditMessage>()))
                .Returns(Task.FromResult<object>(null));

            _handler = new SaveAuditMessageCommandHandler(_auditRepository.Object);

            _auditMessage = new AuditMessage();
            _command = new SaveAuditMessageCommand
            {
                Message = _auditMessage
            };
        }

        [Test]
        public async Task ThenItShouldStoreAuditMessage()
        {
            // Act
            await _handler.Handle(_command);

            // Assert
            _auditRepository.Verify(r => r.StoreAsync(_auditMessage), Times.Once);
        }

        [Test]
        public void ThenItShouldWrapRepositoryExceptionsInADataStorageException()
        {
            // Arrange
            var repoError = new Exception("Unit test");
            _auditRepository.Setup(r => r.StoreAsync(It.IsAny<AuditMessage>()))
                .Throws(repoError);

            // Act + Assert
            var ex = Assert.ThrowsAsync<DataStorageException>(async () => await _handler.Handle(_command));
            Assert.IsNotNull(ex.InnerException);
            Assert.AreSame(repoError, ex.InnerException);
        }
    }
}
