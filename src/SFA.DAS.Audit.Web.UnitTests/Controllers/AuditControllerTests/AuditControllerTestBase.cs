using MediatR;
using Moq;
using NLog;
using SFA.DAS.Audit.Web.Controllers;

namespace SFA.DAS.Audit.Web.UnitTests.Controllers.AuditControllerTests
{
    public abstract class AuditControllerTestBase
    {
        protected Mock<IMediator> _mediator;
        protected Mock<ILogger> _logger;
        protected AuditController _controller;

        public virtual void Arrange()
        {
            _mediator = new Mock<IMediator>();

            _logger = new Mock<ILogger>();

            _controller = new AuditController(_mediator.Object, _logger.Object);
        }
    }
}
