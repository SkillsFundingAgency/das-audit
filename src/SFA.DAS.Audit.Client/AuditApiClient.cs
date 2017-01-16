using System;
using System.Threading.Tasks;
using SFA.DAS.Audit.Types;

namespace SFA.DAS.Audit.Client
{
    public class AuditApiClient : IAuditApiClient
    {
        public Task Audit(AuditMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
