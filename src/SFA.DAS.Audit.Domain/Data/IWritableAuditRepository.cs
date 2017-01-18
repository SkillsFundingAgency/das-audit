using System.Threading.Tasks;

namespace SFA.DAS.Audit.Domain.Data
{
    public interface IWritableAuditRepository
    {
        Task StoreAsync(AuditMessage message);
    }
}
