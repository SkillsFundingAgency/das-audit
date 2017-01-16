using System.Threading.Tasks;
using SFA.DAS.Audit.Types;

namespace SFA.DAS.Audit.Client
{
    public interface IAuditApiClient
    {
        Task Audit(AuditMessage message);
    }
}