﻿using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text;
using Umbraco.Core;
using Umbraco.Extensions;
using Umbraco.Web.Common.Routing;
using Constants = Umbraco.Core.Constants;

namespace Umbraco.Tests.UnitTests.Umbraco.Web.Common.Routing
{

    [TestFixture]
    public class EndpointRouteBuilderExtensionsTests
    {
        [TestCase("umbraco", Constants.Web.Mvc.BackOfficeApiArea, "test", null, true)]
        [TestCase("umbraco", Constants.Web.Mvc.BackOfficeApiArea, "test", "GetStuff", true)]
        [TestCase("umbraco", Constants.Web.Mvc.BackOfficeApiArea, null, null, true)]
        [TestCase("umbraco", Constants.Web.Mvc.BackOfficeApiArea, null, "GetStuff", true)]
        [TestCase("umbraco", Constants.Web.Mvc.BackOfficeApiArea, "test", null, false)]
        [TestCase("umbraco", Constants.Web.Mvc.BackOfficeApiArea, "test", "GetStuff", false)]
        [TestCase("umbraco", Constants.Web.Mvc.BackOfficeApiArea, null, null, false)]
        [TestCase("umbraco", Constants.Web.Mvc.BackOfficeApiArea, null, "GetStuff", false)]
        [TestCase("umbraco", null, "test", null, true)]
        [TestCase("umbraco", null, "test", "GetStuff", true)]
        [TestCase("umbraco", null, null, null, true)]
        [TestCase("umbraco", null, null, "GetStuff", true)]
        [TestCase("umbraco", null, "test", null, false)]
        [TestCase("umbraco", null, "test", "GetStuff", false)]
        [TestCase("umbraco", null, null, null, false)]
        [TestCase("umbraco", null, null, "GetStuff", false)]
        public void MapUmbracoRoute(string umbracoPath, string area, string prefix, string defaultAction, bool includeControllerName)
        {
            var endpoints = new TestRouteBuilder();
            endpoints.MapUmbracoRoute<Testing1Controller>(umbracoPath, area, prefix, defaultAction, includeControllerName);

            var route = endpoints.DataSources.First();
            var endpoint = (RouteEndpoint)route.Endpoints[0];

            var controllerName = ControllerExtensions.GetControllerName<Testing1Controller>();
            var controllerNamePattern = controllerName.ToLowerInvariant();

            if (includeControllerName)
            {
                if (prefix.IsNullOrWhiteSpace())
                    Assert.AreEqual($"{umbracoPath}/{controllerNamePattern}/{{action}}/{{id?}}", endpoint.RoutePattern.RawText);
                else
                    Assert.AreEqual($"{umbracoPath}/{prefix}/{controllerNamePattern}/{{action}}/{{id?}}", endpoint.RoutePattern.RawText);
            }
            else
            {
                if (prefix.IsNullOrWhiteSpace())
                    Assert.AreEqual($"{umbracoPath}/{{action}}/{{id?}}", endpoint.RoutePattern.RawText);
                else
                    Assert.AreEqual($"{umbracoPath}/{prefix}/{{action}}/{{id?}}", endpoint.RoutePattern.RawText);
            }

            if (!area.IsNullOrWhiteSpace())
                Assert.AreEqual(area, endpoint.RoutePattern.Defaults["area"]);
            if (!defaultAction.IsNullOrWhiteSpace())
                Assert.AreEqual(defaultAction, endpoint.RoutePattern.Defaults["action"]);            
            Assert.AreEqual(controllerName, endpoint.RoutePattern.Defaults["controller"]);
        }

        [TestCase("umbraco", Constants.Web.Mvc.BackOfficeApiArea, true, null)]
        [TestCase("umbraco", Constants.Web.Mvc.BackOfficeApiArea, true, "GetStuff")]
        [TestCase("umbraco", Constants.Web.Mvc.BackOfficeApiArea, false, null)]
        [TestCase("umbraco", Constants.Web.Mvc.BackOfficeApiArea, false, "GetStuff")]
        [TestCase("umbraco", null, true, null)]
        [TestCase("umbraco", null, true, "GetStuff")]
        [TestCase("umbraco", null, false, null)]
        [TestCase("umbraco", null, false, "GetStuff")]
        public void MapUmbracoApiRoute(string umbracoPath, string area, bool isBackOffice, string defaultAction)
        {
            var endpoints = new TestRouteBuilder();
            endpoints.MapUmbracoApiRoute<Testing1Controller>(umbracoPath, area, isBackOffice, defaultAction);

            var route = endpoints.DataSources.First();
            var endpoint = (RouteEndpoint)route.Endpoints[0];

            var controllerName = ControllerExtensions.GetControllerName<Testing1Controller>();
            var controllerNamePattern = controllerName.ToLowerInvariant();
            var areaPattern = area?.ToLowerInvariant();

            if (isBackOffice)
            {
                if (area.IsNullOrWhiteSpace())
                    Assert.AreEqual($"{umbracoPath}/backoffice/api/{controllerNamePattern}/{{action}}/{{id?}}", endpoint.RoutePattern.RawText);
                else
                    Assert.AreEqual($"{umbracoPath}/backoffice/{areaPattern}/{controllerNamePattern}/{{action}}/{{id?}}", endpoint.RoutePattern.RawText);
            }
            else
            {
                if (area.IsNullOrWhiteSpace())
                    Assert.AreEqual($"{umbracoPath}/api/{controllerNamePattern}/{{action}}/{{id?}}", endpoint.RoutePattern.RawText);
                else
                    Assert.AreEqual($"{umbracoPath}/{areaPattern}/{controllerNamePattern}/{{action}}/{{id?}}", endpoint.RoutePattern.RawText);
            }

            if (!area.IsNullOrWhiteSpace())
                Assert.AreEqual(area, endpoint.RoutePattern.Defaults["area"]);
            if (!defaultAction.IsNullOrWhiteSpace())
                Assert.AreEqual(defaultAction, endpoint.RoutePattern.Defaults["action"]);
            Assert.AreEqual(controllerName, endpoint.RoutePattern.Defaults["controller"]);
        }

        private class Testing1Controller : ControllerBase
        {

        }
    }
}
