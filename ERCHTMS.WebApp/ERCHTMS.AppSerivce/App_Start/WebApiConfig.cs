using System.Web.Http;

namespace ERCHTMS.AppSerivce
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
             routeTemplate: "api/{controller}/{action}/{id}",
             defaults: new { id = System.Web.Http.RouteParameter.Optional }
           );
            config.Filters.Add(new WebAPIHandlerErrorAttribute());
        }
    }
}   
