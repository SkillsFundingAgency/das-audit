using System;
using System.Threading.Tasks;
using SFA.DAS.Audit.Application.Validation;

namespace SFA.DAS.Audit.Application.QueueAuditMessage
{
    public class QueueAuditMessageCommandValidator : IValidator<QueueAuditMessageCommand>
    {
        public Task<ValidationResult> ValidateAsync(QueueAuditMessageCommand message)
        {
            var validationResult = new ValidationResult();


            if (message.Message == null)
            {
                validationResult.AddError(nameof(message.Message));
                return Task.FromResult(validationResult);
            }

            if (string.IsNullOrEmpty(message.Message.Description))
            {
                validationResult.AddError(nameof(message.Message.Description));
            }
            if (string.IsNullOrEmpty(message.Message.Source))
            {
                validationResult.AddError(nameof(message.Message.Source));
            }
            if (string.IsNullOrEmpty(message.Message.AffectedEntity?.Type) || string.IsNullOrEmpty(message.Message.AffectedEntity?.Id))
            {
                validationResult.AddError(nameof(message.Message.AffectedEntity));
            }
            if (string.IsNullOrEmpty(message.Message.ChangedBy?.Id) && string.IsNullOrEmpty(message.Message.ChangedBy?.EmailAddress))
            {
                validationResult.AddError(nameof(message.Message.ChangedBy));
            }
            if (message.Message.ChangeAt == DateTime.MinValue)
            {
                validationResult.AddError(nameof(message.Message.ChangeAt));
            }


            return Task.FromResult(validationResult);
        }
    }
}
