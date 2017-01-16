using System;
using System.Collections.Generic;
using MediatR;
using Moq;
using NLog;
using SFA.DAS.Audit.Types;
using SFA.DAS.Audit.Web.Controllers;

namespace SFA.DAS.Audit.Web.UnitTests.Controllers.AuditControllerTests
{
    public abstract class AuditControllerTestBase
    {
        protected Mock<IMediator> _mediator;
        protected Mock<ILogger> _logger;
        protected AuditController _controller;

        protected AuditMessage _goodMessage;

        public virtual void Arrange()
        {
            _mediator = new Mock<IMediator>();

            _logger = new Mock<ILogger>();

            _controller = new AuditController(_mediator.Object, _logger.Object);

            _goodMessage = new AuditMessage
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
            };
        }
    }
}
