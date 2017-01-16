using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Audit.Application.Validation;

namespace SFA.DAS.Audit.Application.QueueAuditMessage
{
    public class QueueAuditMessageCommandValidator : IValidator<QueueAuditMessageCommand>
    {
        public Task<ValidationResult> ValidateAsync(QueueAuditMessageCommand message)
        {
            var errors = new List<ValidationError>();

            return Task.FromResult(new ValidationResult(errors));
        }
    }
}
