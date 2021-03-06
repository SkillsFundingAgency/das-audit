﻿using MediatR;
using SFA.DAS.Audit.Domain;

namespace SFA.DAS.Audit.Application.QueueAuditMessage
{
    public class QueueAuditMessageCommand : IAsyncRequest<Unit>
    {
        public AuditMessage Message { get; set; }
    }
}
