using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Audit.Domain.Data;

namespace SFA.DAS.Audit.Application.SaveAuditMessage
{
    public class SaveAuditMessageCommandHandler : IAsyncRequestHandler<SaveAuditMessageCommand, Unit>
    {
        private readonly IWritableAuditRepository _auditRepository;

        public SaveAuditMessageCommandHandler(IWritableAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task<Unit> Handle(SaveAuditMessageCommand message)
        {
            try
            {
                await _auditRepository.StoreAsync(message.Message);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new DataStorageException(ex.Message, ex);
            }
        }
    }
}
