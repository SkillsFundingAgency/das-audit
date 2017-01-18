using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Audit.Application.Validation;
using SFA.DAS.Messaging;

namespace SFA.DAS.Audit.Application.QueueAuditMessage
{
    public class QueueAuditMessageCommandHandler : IAsyncRequestHandler<QueueAuditMessageCommand, Unit>
    {
        private readonly IValidator<QueueAuditMessageCommand> _validator;
        private readonly IMessagePublisher _messagePublisher;

        public QueueAuditMessageCommandHandler(IValidator<QueueAuditMessageCommand> validator, IMessagePublisher messagePublisher)
        {
            _validator = validator;
            _messagePublisher = messagePublisher;
        }

        public async Task<Unit> Handle(QueueAuditMessageCommand message)
        {
            var validationResult = await _validator.ValidateAsync(message);
            if (!validationResult.IsValid)
            {
                throw new InvalidRequestException(validationResult.Errors);
            }

            await _messagePublisher.PublishAsync(message.Message);

            return Unit.Value;
        }
    }
}