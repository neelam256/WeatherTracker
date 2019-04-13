using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WeatherTracker
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
          
            //Filter Handler
            RegisterFilters(config);

        }

        private static void RegisterFilters(HttpConfiguration config)
        {
            // one filter must be added in order for the Exception to work            
            config.Filters.Add(new ApiExceptionFilterAttribute());
            
        }
      
    }
}
