using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NLog;
using SFA.DAS.Audit.Application.SaveAuditMessage;
using SFA.DAS.Audit.Domain;
using SFA.DAS.Messaging;

namespace SFA.DAS.Audit.Processor
{
    public class AuditMessageMonitor
    {
        private readonly IEventingMessageReceiver<AuditMessage> _messageReceiver;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public AuditMessageMonitor(IEventingMessageReceiver<AuditMessage> messageReceiver, IMediator mediator, ILogger logger)
        {
            _messageReceiver = messageReceiver;
            _messageReceiver.MessageReceived += MessageReceived;

            _mediator = mediator;
            _logger = logger;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            await _messageReceiver.RunAsync(cancellationToken);
        }

        private async void MessageReceived(object sender, MessageReceivedEventArgs<AuditMessage> e)
        {
            try
            {
                await _mediator.SendAsync(new SaveAuditMessageCommand {Message = e.Message});

                e.Handled = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Failed to save audit message - {ex.Message}");
            }
        }
    }
}
