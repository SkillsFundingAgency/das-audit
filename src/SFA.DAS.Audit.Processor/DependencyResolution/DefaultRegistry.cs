// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MediatR;
using Microsoft.Azure;
using SFA.DAS.Audit.Domain;
using SFA.DAS.Messaging;
using SFA.DAS.Messaging.AzureStorageQueue;
using StructureMap;
using StructureMap.Graph;

namespace SFA.DAS.Audit.Processor.DependencyResolution {
    
    public class DefaultRegistry : Registry {
        
        private const string ServiceNamespace = "SFA.DAS";

        public DefaultRegistry() {
            Scan(
                scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith(ServiceNamespace));
                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                });

            AddMessageService();
            
            RegisterMediator();
        }

        private void AddMessageService()
        {
            For<IPollingMessageReceiver>().Use<AzureStorageQueueService>()
                .Ctor<string>("connectionString").Is(CloudConfigurationManager.GetSetting("AzureStorageQueueServiceConnectionString"))
                .Ctor<string>("queueName").Is("auditmessages");

            For<IEventingMessageReceiver<AuditMessage>>().Use<EventingMessageReceiverPollingWrapper<AuditMessage>>();
        }

        private void RegisterMediator()
        {
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
            For<IMediator>().Use<Mediator>();
        }
        
    }
}