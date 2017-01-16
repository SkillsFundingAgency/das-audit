using System;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using NLog;
using SFA.DAS.Audit.Application.QueueAuditMessage;
using SFA.DAS.Audit.Application.Validation;
using SFA.DAS.Audit.Types;
using SFA.DAS.Audit.Web.Plumbing.WebApi;

namespace SFA.DAS.Audit.Web.Controllers
{
    public class AuditController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public AuditController(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [VersionedRoute("api/audit", 1)]
        [Route("api/v1/audit")]
        public async Task<IHttpActionResult> WriteAudit(AuditMessage message)
        {
            try
            {
                await _mediator.SendAsync(new QueueAuditMessageCommand
                {
                    Message = message
                });

                return Ok();
            }
            catch (InvalidRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unexpected error writing audit - {ex.Message}");
                return InternalServerError();
            }
        }
    }
}
