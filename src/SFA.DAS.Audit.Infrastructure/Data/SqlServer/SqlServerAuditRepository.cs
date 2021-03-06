﻿using System;
using System.Threading.Tasks;
using SFA.DAS.Audit.Domain;
using SFA.DAS.Audit.Domain.Data;
using SFA.DAS.Audit.Infrastructure.Data.SqlServer.Entities;

namespace SFA.DAS.Audit.Infrastructure.Data.SqlServer
{
    public class SqlServerAuditRepository : SqlServerRepository, IWritableAuditRepository
    {
        public SqlServerAuditRepository()
            : base("AuditRepositoryConnectionString")
        {
        }

        public async Task StoreAsync(AuditMessage message)
        {
            using (var unitOfWork = await StartUnitOfWork())
            {
                var messageEntity = MapDomainToEntity(message);
                messageEntity.Id = Guid.NewGuid();
                await unitOfWork.ExecuteAsync("[dbo].[CreateAuditMessage] @Id, @AffectedEntityType, @AffectedEntityId, @Category, @Description, @SourceSystem, @SourceComponent, " +
                                              "@SourceVersion, @ChangedAt, @ChangedById, @ChangedByEmail, @ChangedByOriginIp", messageEntity);

                foreach (var entity in message.RelatedEntities)
                {
                    await unitOfWork.ExecuteAsync("[dbo].[CreateRelatedEntity] @EntityType, @EntityId, @MessageId",
                        new { EntityType = entity.Type, EntityId = entity.Id, MessageId = messageEntity.Id });
                }

                foreach (var update in message.ChangedProperties)
                {
                    await unitOfWork.ExecuteAsync("[dbo].[CreateChangedProperty] @PropertyName, @NewValue, @MessageId",
                        new { PropertyName = update.PropertyName, NewValue = update.NewValue, MessageId = messageEntity.Id });
                }

                unitOfWork.CommitChanges();
            }
        }



        private AuditMessageEntity MapDomainToEntity(AuditMessage message)
        {
            return new AuditMessageEntity
            {
                AffectedEntityId = message.AffectedEntity.Id,
                AffectedEntityType = message.AffectedEntity.Type,
                Category = message.Category,
                Description = message.Description,
                SourceSystem = message.Source.System,
                SourceComponent = message.Source.Component,
                SourceVersion = message.Source.Version,
                ChangedAt = message.ChangeAt,
                ChangedById = message.ChangedBy?.Id,
                ChangedByEmail = message.ChangedBy?.EmailAddress,
                ChangedByOriginIp = message.ChangedBy?.OriginIpAddress
            };
        }

    }
}
