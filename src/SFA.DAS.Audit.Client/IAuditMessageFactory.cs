using SFA.DAS.Audit.Types;

namespace SFA.DAS.Audit.Client
{
    public interface IAuditMessageFactory
    {
        AuditMessage Build();
    }
}