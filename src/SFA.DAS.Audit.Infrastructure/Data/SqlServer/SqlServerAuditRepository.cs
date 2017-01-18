using System;
using System.Threading.Tasks;
using SFA.DAS.Audit.Domain;
using SFA.DAS.Audit.Domain.Data;

namespace SFA.DAS.Audit.Infrastructure.Data.SqlServer
{
    public class SqlServerAuditRepository : SqlServerRepository, IWritableAuditRepository
    {
        public SqlServerAuditRepository() 
            : base("AuditRepositoryConnectionString")
        {
        }

        public Task StoreAsync(AuditMessage message)
        {
            throw new NotImplementedException();
        }

    }
}
