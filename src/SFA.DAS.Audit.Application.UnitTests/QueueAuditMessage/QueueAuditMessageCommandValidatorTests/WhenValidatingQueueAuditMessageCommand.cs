using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Audit.Application.QueueAuditMessage;
using SFA.DAS.Audit.Types;

namespace SFA.DAS.Audit.Application.UnitTests.QueueAuditMessage.QueueAuditMessageCommandValidatorTests
{
    public class WhenValidatingQueueAuditMessageCommand
    {
        private QueueAuditMessageCommandValidator _validator;

        private QueueAuditMessageCommand _command;

        [SetUp]
        public void Arrange()
        {
            _validator = new QueueAuditMessageCommandValidator();

            _command = new QueueAuditMessageCommand
            {
                Message = new AuditMessage
                {
                    AffectedEntity = new Entity
                    {
                        Type = "TestEntity",
                        Id = "TEST-ENTITY-1"
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
                }
            };
        }

        [Test]
        public async Task ThenItShouldReturnAValidResultIfNoIssues()
        {
            // Act
            var acutal = await _validator.ValidateAsync(_command);

            // Assert
            Assert.IsNotNull(acutal);
            Assert.IsTrue(acutal.IsValid);
        }
    }
}
