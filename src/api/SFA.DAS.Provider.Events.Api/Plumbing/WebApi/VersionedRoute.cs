﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
// Source: https://aspnet.codeplex.com/SourceControl/latest#Samples/WebApi/RoutingConstraintsSample/RoutingConstraints.Server/VersionedRoute.cs
using System.Collections.Generic;
using System.Web.Http.Routing;

namespace SFA.DAS.Provider.Events.Api.Plumbing.WebApi
{
    /// <summary>
    /// Provides an attribute route that's restricted to a specific version of the api.
    /// </summary>
    internal class VersionedRoute : RouteFactoryAttribute
    {
        public VersionedRoute(string template, int allowedVersion)
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