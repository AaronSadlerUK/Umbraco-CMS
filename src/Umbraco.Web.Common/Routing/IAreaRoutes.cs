﻿using Microsoft.AspNetCore.Routing;

namespace Umbraco.Web.Common.Routing
{
    /// <summary>
    /// Used to create routes for a route area
    /// </summary>
    public interface IAreaRoutes
    {
        // TODO: It could be possible to just get all collections of IAreaRoutes and route them all instead of relying
        // on individual ext methods. This would reduce the amount of code in Startup, but could also mean there's less control over startup
        // if someone wanted that. Maybe we can just have both.

        void CreateRoutes(IEndpointRouteBuilder endpoints);
    }
}
