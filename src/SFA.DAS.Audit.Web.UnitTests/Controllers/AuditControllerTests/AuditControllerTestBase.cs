using System.Linq;
using MediatR;
using Moq;
using NLog;
using SFA.DAS.Audit.Application.Mapping;
using SFA.DAS.Audit.Web.Controllers;

namespace SFA.DAS.Audit.Web.UnitTests.Controllers.AuditControllerTests
{
    public abstract class AuditControllerTestBase
    {
        protected Mock<IMapper> _mapper;
        protected Mock<IMediator> _mediator;
        protected Mock<ILogger> _logger;
        protected AuditController _controller;

        public virtual void Arrange()
        {
            _mapper = new Mock<IMapper>();
            ConfigureDefaultAuditMessageMapping();

            _mediator = new Mock<IMediator>();

            _logger = new Mock<ILogger>();

            _controller = new AuditController(_mapper.Object, _mediator.Object, _logger.Object);
        }

        private void ConfigureDefaultAuditMessageMapping()
        {
            _mapper.Setup(m => m.Map<Domain.AuditMessage>(It.IsAny<Types.AuditMessage>()))
                .Returns((Types.AuditMessage source) =>
                {
                    return new Domain.AuditMessage
                    {
                        AffectedEntity = new Domain.Entity
                        {
                            Id = source.AffectedEntity.Id,
                            Type = source.AffectedEntity.Type
                        },
                        Source = new Domain.Source
                        {
                            System = source.Source.System,
                            Component = source.Source.Component,
                            Version = source.Source.Version
                        },
                        Description = source.Description,
                        ChangedBy = new Domain.Actor
                        {
                            Id = source.ChangedBy.Id,
                            EmailAddress = source.ChangedBy.EmailAddress,
                            OriginIpAddress = source.ChangedBy.OriginIpAddress
                        },
                        ChangeAt = source.ChangeAt,
                        ChangedProperties = source.ChangedProperties == null ? null : source.ChangedProperties.Select(sp => new Domain.PropertyUpdate
                        {
                            PropertyName = sp.PropertyName,
                            NewValue = sp.NewValue
                        }).ToList(),
                        RelatedEntities = source.RelatedEntities == null ? null : source.RelatedEntities.Select(se => new Domain.Entity
                        {
                            Id = se.Id,
                            Type = se.Type
                        }).ToList()
                    };
                });
        }
    }
}
