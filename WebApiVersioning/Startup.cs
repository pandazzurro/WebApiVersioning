using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using WebApi.Binder.Providers;
using Serilog;

[assembly: OwinStartup(typeof(WebApi.Binder.Startup))]
namespace WebApi.Binder
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var uriDocumentDb = new Uri("https://webapiversioning.documents.azure.com:443/");
            var primaryKey = "zcSgJkaHsKpktdulC6TTADbB7pd568uCm0emZGUF63H2yPGYz7HYhTZgsU5qg6D8fIJUB0jFZU72kOkzZzCHeA==";
            LoggerConfiguration cfg = new LoggerConfiguration()
                .Destructure.ToMaximumDepth(10)
                .MinimumLevel.Verbose()
                .WriteTo
                .AzureDocumentDB(uriDocumentDb, primaryKey);    
            Log.Logger = cfg.CreateLogger();

            Log.Debug("Startup progetto");
            
            var oAuthOptions = new OAuthAuthorizationServerOptions
            {
                AccessTokenProvider = new OAuthTokenProvider(),
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(new SecurityService()),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14), //TODO: da configurazione
                //TODO: In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true
            };
            app.UseOAuthAuthorizationServer(oAuthOptions);

            // Valida i token a ogni richiesta e inserisce i claims dell'utente.
            var oAuthAuthentication = new OAuthBearerAuthenticationOptions
            {
                AccessTokenProvider = new OAuthTokenProvider(),
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                Provider = new AuthenticationOAuthProvider(new SecurityService())
            };
            app.UseOAuthBearerAuthentication(oAuthAuthentication);
            
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            app.UseWebApi(httpConfiguration);
        }
    }
}
