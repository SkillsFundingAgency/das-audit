using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using SFA.DAS.Audit.Application.QueueAuditMessage;
using SFA.DAS.Audit.Application.Validation;
using SFA.DAS.Audit.Types;

namespace SFA.DAS.Audit.Web.UnitTests.Controllers.AuditControllerTests
{
    public class WhenWritingAudit : AuditControllerTestBase
    {
        private AuditMessage _message;

        [SetUp]
        public override void Arrange()
        {
            base.Arrange();

            _message = new AuditMessage
            {
                AffectedEntity = new Entity
                {
                    Type = "TestEntity",
                    Id = "TEST-ENTITY-1"
                },
                Source = new Source
                {
                    System = "Test",
                    Component = "UnitTests",
                    Version = "1.2.3"
                },
                Description = "CREATED",
                ChangedProperties = new List<PropertyUpdate>
                {
                    new PropertyUpdate
                    {
                        PropertyName = "Title",
                        NewValue = "Unit Test"
                    },
                    new PropertyUpdate
                    {
                        PropertyName = "Description",
                        NewValue = "Test entity for unit testing"
                    }
                },
                ChangeAt = new DateTime(2017, 4, 1, 12, 33, 45),
                ChangedBy = new Actor
                {
                    Id = "User1",
                    EmailAddress = "user.one@unit.tests",
                    OriginIpAddress = "127.0.0.1"
                },
                RelatedEntities = new List<Entity>
                {
                    new Entity
                    {
                        Type = "DemoEntity",
                        Id = "DEMO-1"
                    }
                }
            };
        }


        [Test]
        public async Task ThenItShouldReturnAnOkResult()
        {
            // Act
            var actual = await _controller.WriteAudit(_message);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<OkResult>(actual);
        }

        [Test]
        public async Task ThenItShouldQueueMessage()
        {
            // Act
            await _controller.WriteAudit(_message);

            // Assert
            _mediator.Verify(m => m.SendAsync(It.IsAny<QueueAuditMessageCommand>()), Times.Once);
        }

        [Test]
        public async Task ThenItShouldReturnInternalServerErrorResultWhenUnexpectedErrorIsThrown()
        {
            // Arrange
            _mediator.Setup(m => m.SendAsync(It.IsAny<QueueAuditMessageCommand>()))
                .ThrowsAsync(new Exception("Test"));

            // Act
            var actual = await _controller.WriteAudit(_message);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<InternalServerErrorResult>(actual);
        }

        [Test]
        public async Task ThenItShouldReturnBadRequestResultWhenInvalidRequestExceptionIsThrown()
        {
            // Arrange
            _mediator.Setup(m => m.SendAsync(It.IsAny<QueueAuditMessageCommand>()))
                .ThrowsAsync(new InvalidRequestException(new[]
                {
                    new ValidationError { Property = "Description", Description = "You must specify a description" }
                }));

            // Act
            var actual = await _controller.WriteAudit(_message);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(actual);
            Assert.AreEqual("Invalid request.\n\nYou must specify a description", ((BadRequestErrorMessageResult)actual).Message);
        }
    }
}
