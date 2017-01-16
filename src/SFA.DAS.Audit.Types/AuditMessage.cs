using System;
using System.Collections.Generic;

namespace SFA.DAS.Audit.Types
{
    public class AuditMessage
    {
        public Entity AffectedEntity { get; set; }
        public string Description { get; set; }
        public List<PropertyUpdate> ChangedProperties { get; set; }
        public DateTime ChangeAt { get; set; }
        public Actor ChangedBy { get; set; }
        public List<Entity> RelatedEntities { get; set; }
    }

    public class Actor
    {
        public string Id { get; set; }
        public string EmailAddress { get; set; }
        public string OriginIpAddress { get; set; }
    }

    public class Entity
    {
        public string Type { get; set; }
        public string Id { get; set; }
    }

    public class PropertyUpdate
    {
        public string PropertyName { get; set; }
        public string NewValue { get; set; }
    }
}
