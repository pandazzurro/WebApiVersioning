using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace WebApi.Versioning
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
                routeTemplate: "api/{area}/{version}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Da eseguire solo per utilizzare il custom controller selector
            //config.Services.Replace(typeof(IHttpControllerSelector), new VersionControllerSelector((config)));        
        }
    }
}
