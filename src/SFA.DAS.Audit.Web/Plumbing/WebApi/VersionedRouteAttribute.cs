using System.Collections.Generic;
using System.Web.Http.Routing;

namespace SFA.DAS.Audit.Web.Plumbing.WebApi
{
    // Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
    // Source https://aspnet.codeplex.com/SourceControl/latest#Samples/WebApi/RoutingConstraintsSample/RoutingConstraints.Server/VersionedRoute.cs
    public class VersionedRouteAttribute : RouteFactoryAttribute
    {
        public VersionedRouteAttribute(string template, int allowedVersion)
            : base(template)
        {
            AllowedVersion = allowedVersion;
        }

        public int AllowedVersion { get; }

        public override IDictionary<string, object> Constraints
        {
            get
            {
                var constraints = new HttpRouteValueDictionary();
                constraints.Add("version", new VersionConstraint(AllowedVersion));
                return constraints;
            }
        }
    }

}