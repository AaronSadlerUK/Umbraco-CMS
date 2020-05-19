﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Hosting;
using Umbraco.Extensions;
using Umbraco.Web.Common.Install;

namespace Umbraco.Tests.UnitTests.Umbraco.Web.Common.Routing
{
    [TestFixture]
    public class InstallAreaRoutesTests
    {
        [TestCase(RuntimeLevel.BootFailed)]
        [TestCase(RuntimeLevel.Unknown)]
        [TestCase(RuntimeLevel.Boot)]
        public void RuntimeState_No_Routes(RuntimeLevel level)
        {
            var routes = GetInstallAreaRoutes(level);
            var endpoints = new TestRouteBuilder();
            routes.CreateRoutes(endpoints);

            Assert.AreEqual(0, endpoints.DataSources.Count);
        }

        [TestCase(RuntimeLevel.Install)]
        [TestCase(RuntimeLevel.Upgrade)]
        public void RuntimeState_Install(RuntimeLevel level)
        {
            var routes = GetInstallAreaRoutes(level);
            var endpoints = new TestRouteBuilder();
            routes.CreateRoutes(endpoints);

            Assert.AreEqual(2, endpoints.DataSources.Count);
            var route = endpoints.DataSources.First();
            Assert.AreEqual(2, route.Endpoints.Count);

            var endpoint1 = (RouteEndpoint)route.Endpoints[0];
            Assert.AreEqual($"install/api/{{action}}/{{id?}}", endpoint1.RoutePattern.RawText);
            Assert.AreEqual(Constants.Web.Mvc.InstallArea, endpoint1.RoutePattern.Defaults["area"]);
            Assert.AreEqual("Index", endpoint1.RoutePattern.Defaults["action"]);
            Assert.AreEqual(ControllerExtensions.GetControllerName<InstallApiController>(), endpoint1.RoutePattern.Defaults["controller"]);
            Assert.AreEqual(endpoint1.RoutePattern.Defaults["area"], typeof(InstallApiController).GetCustomAttribute<AreaAttribute>(false).RouteValue);

            var endpoint2 = (RouteEndpoint)route.Endpoints[1];
            Assert.AreEqual($"install/{{action}}/{{id?}}", endpoint2.RoutePattern.RawText);
            Assert.AreEqual(Constants.Web.Mvc.InstallArea, endpoint2.RoutePattern.Defaults["area"]);
            Assert.AreEqual("Index", endpoint2.RoutePattern.Defaults["action"]);
            Assert.AreEqual(ControllerExtensions.GetControllerName<InstallController>(), endpoint2.RoutePattern.Defaults["controller"]);
            Assert.AreEqual(endpoint2.RoutePattern.Defaults["area"], typeof(InstallController).GetCustomAttribute<AreaAttribute>(false).RouteValue);

            var fallbackRoute = endpoints.DataSources.Last();
            Assert.AreEqual(1, fallbackRoute.Endpoints.Count);

            Assert.AreEqual("Fallback {*path:nonfile}", fallbackRoute.Endpoints[0].ToString());
        }

        [Test]
        public void RuntimeState_Run()
        {
            var routes = GetInstallAreaRoutes(RuntimeLevel.Run);
            var endpoints = new TestRouteBuilder();
            routes.CreateRoutes(endpoints);

            Assert.AreEqual(1, endpoints.DataSources.Count);
            var route = endpoints.DataSources.First();
            Assert.AreEqual(1, route.Endpoints.Count);

            Assert.AreEqual("install/{controller?}/{action?} HTTP: GET", route.Endpoints[0].ToString());

        }

        private InstallAreaRoutes GetInstallAreaRoutes(RuntimeLevel level)
        {
            var routes = new InstallAreaRoutes(                
                Mock.Of<IRuntimeState>(x => x.Level == level),
                Mock.Of<IHostingEnvironment>(x => x.ToAbsolute(It.IsAny<string>()) == "/install" && x.ApplicationVirtualPath == string.Empty),
                Mock.Of<LinkGenerator>());

            return routes;
        }
    }
}
