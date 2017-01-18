using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Audit.Application.SaveAuditMessage;
using SFA.DAS.Audit.Types;
using SFA.DAS.Messaging;

namespace SFA.DAS.Audit.Processor
{
    public class AuditMessageMonitor
    {
        private readonly IEventingMessageReceiver<AuditMessage> _messageReceiver;
        private readonly IMediator _mediator;

        public AuditMessageMonitor(IEventingMessageReceiver<AuditMessage> messageReceiver, IMediator mediator)
        {
            _messageReceiver = messageReceiver;
            _messageReceiver.MessageReceived += MessageReceived;

            _mediator = mediator;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            await _messageReceiver.RunAsync(cancellationToken);
        }

        private async void MessageReceived(object sender, MessageReceivedEventArgs<AuditMessage> e)
        {
            await _mediator.SendAsync(new SaveAuditMessageCommand { Message = e.Message });

            e.Handled = true;
        }
    }
}
