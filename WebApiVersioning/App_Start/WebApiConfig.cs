using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using WebApiVersioning.ModelBinder;
using WebApiVersioning.Models;

namespace WebApiVersioning
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var utenteProvider = new SimpleModelBinderProvider(typeof(UtenteViewModel), new UtenteModelBinder());
            var adminProvider = new SimpleModelBinderProvider(typeof(UtenteViewModel), new AdminModelBinder());

            config.Services.InsertRange(typeof(ModelBinderProvider), 0, new[] { utenteProvider, adminProvider });

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
