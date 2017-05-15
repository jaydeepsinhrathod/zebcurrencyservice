using Microsoft.Web.Http.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using zebCurrencyService.Customization;

namespace zebCurrencyService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // added to the web api configuration in the application setup
            var constraintResolver = new DefaultInlineConstraintResolver()
            {
                ConstraintMap =
                    {
                        ["apiVersion"] = typeof(ApiVersionRouteConstraint )
                    }
            };
            // Web API routes
            config.MapHttpAttributeRoutes(constraintResolver);
            //register versioning service
            config.AddApiVersioning();
            // Web API configuration and services

            config.Formatters.Insert(0, new MyDecimalFormatter());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
