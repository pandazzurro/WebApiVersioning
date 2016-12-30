using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Metadata;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using WebApi.Binder.ModelBinder;
using WebApi.Binder.Models;

namespace WebApi.Binder
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            
            var utenteProvider = new SimpleModelBinderProvider(typeof(UtenteViewModel), new UtenteModelBinder());
            var adminProvider = new SimpleModelBinderProvider(typeof(AdminViewModel), new AdminModelBinder());

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
