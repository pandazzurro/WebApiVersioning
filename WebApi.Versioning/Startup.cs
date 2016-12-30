using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(WebApi.Versioning.Startup))]
namespace WebApi.Versioning
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            app.UseWebApi(httpConfiguration);
        }
    }
}
