﻿using System;
using System.Collections.Generic;
using SFA.DAS.Audit.Types;

namespace SFA.DAS.Audit.Client
{
    public class AuditMessageFactory : IAuditMessageFactory
    {
        private readonly static List<Action<AuditMessage>> MessageBuilders = new List<Action<AuditMessage>>();

        public static Action<AuditMessage>[] Builders => MessageBuilders.ToArray();

        public static void RegisterBuilder(Action<AuditMessage> builder)
        {
            MessageBuilders.Add(builder);
        }

        public AuditMessage Build()
        {
            var message = new AuditMessage
            {
                ChangeAt = DateTime.UtcNow
            };

            foreach (var builder in MessageBuilders)
            {
                builder.Invoke(message);
            }

            return message;
        }
    }
}
