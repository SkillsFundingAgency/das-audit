using MediatR;
using SFA.DAS.Audit.Domain;

namespace SFA.DAS.Audit.Application.SaveAuditMessage
{
    public class SaveAuditMessageCommand : IAsyncRequest
    {
        public AuditMessage Message { get; set; }
    }
}
