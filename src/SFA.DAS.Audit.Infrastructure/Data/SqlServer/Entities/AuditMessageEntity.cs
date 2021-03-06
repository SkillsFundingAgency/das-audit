﻿using System;

namespace SFA.DAS.Audit.Infrastructure.Data.SqlServer.Entities
{
    internal class AuditMessageEntity
    {
        public Guid Id { get; set; }
        public string AffectedEntityType { get; set; }
        public string AffectedEntityId { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string SourceSystem { get; set; }
        public string SourceComponent { get; set; }
        public string SourceVersion { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangedById { get; set; }
        public string ChangedByEmail { get; set; }
        public string ChangedByOriginIp { get; set; }

    }
}
