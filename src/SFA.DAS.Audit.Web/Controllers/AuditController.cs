using System;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using NLog;
using SFA.DAS.Audit.Application.Mapping;
using SFA.DAS.Audit.Application.QueueAuditMessage;
using SFA.DAS.Audit.Application.Validation;
using SFA.DAS.Audit.Types;
using SFA.DAS.Audit.Web.Plumbing.WebApi;

namespace SFA.DAS.Audit.Web.Controllers
{
    public class AuditController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public AuditController(IMapper mapper, IMediator mediator, ILogger logger)
        {
            _mapper = mapper;
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
                var domainMessage = _mapper.Map<Domain.AuditMessage>(message);

                await _mediator.SendAsync(new QueueAuditMessageCommand
                {
                    Message = domainMessage
                });

                return Ok();
            }
            catch (InvalidRequestException ex)
            {
                _logger.Info(ex, $"Bad request made to WriteAudit endpoint - {ex.Message}");
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
