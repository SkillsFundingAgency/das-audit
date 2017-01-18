using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Audit.Types;

namespace SFA.DAS.Audit.Test.Shared.ObjectMothers
{
    public static class AuditMessageMother
    {
        public static AuditMessage Create()
        {
            return new AuditMessage
            {
                AffectedEntity = new Entity
                {
                    Type = "TestEntity",
                    Id = "TEST-ENTITY-1"
                },
                Description = "CREATED",
                Source = new Source
                {
                    Component = "Test",
                    System = "More Test",
                    Version = "2"
                },
                ChangedProperties = new List<PropertyUpdate>
                {
                    new PropertyUpdate
                    {
                        PropertyName = "Title",
                        NewValue = "Unit Test"
                    },
                    new PropertyUpdate
                    {
                        PropertyName = "Description",
                        NewValue = "Test entity for unit testing"
                    }
                },
                ChangeAt = new DateTime(2017, 4, 1, 12, 33, 45),
                ChangedBy = new Actor
                {
                    Id = "User1",
                    EmailAddress = "user.one@unit.tests",
                    OriginIpAddress = "127.0.0.1"
                },
                RelatedEntities = new List<Entity>
                {
                    new Entity
                    {
                        Type = "DemoEntity",
                        Id = "DEMO-1"
                    }
                }
            };
        } 
    }
}
