using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Text.RegularExpressions;
using System.Security;

namespace WebApiVersioning.Providers
{
    /// <summary>
    /// Provider OAuth utilizzato da Owin per gestire l'autenticazione dei client che si connettono alla WebApi.
    /// </summary>
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        /// <summary>
        /// Crea una istanza del provider settando il SecurityService utilizzato per le fasi di autenticazione.
        /// </summary>
        /// <param name="securityService"></param>
        public ApplicationOAuthProvider()
        {
            _publicClientId = "self";
        }

        /// <summary>
        /// Invocato quando una richiesta all'endpoint di richiesta di un nuovo token ("grant_type" == "password").
        /// In corrispondenza dell'invocazione sono inviati username e password da utilizzare per l'utenticazione
        /// e ottenere un nuovo token di autenticazione.
        /// </summary>
        /// <param name="context">Contesto iniettato da Owin contenente anche le credenziali di autenticazione ricevute dal client.</param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
             if(context.UserName != context.Password){
                throw new SecurityException("Utente non validato: username e password diversi!!!");
            }

            //try
            //{
                var p = new ClaimsPrincipal();
                if (p == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                var claimsIdentity = new ClaimsIdentity(p.FindAll(x => x.Type == System.Security.Claims.ClaimTypes.Name), OAuthDefaults.AuthenticationType);

                AuthenticationProperties properties = CreateProperties(context.UserName);
                AuthenticationTicket ticket = new AuthenticationTicket(claimsIdentity, properties);
                context.Validated(ticket);
            //}
            //catch (SecurityException se)
            //{
            //    // espongo l'errore di validazione con uno spazio prima di ogni lettera maiuscola.
            //    context.SetError("invalid_grant", Regex.Replace(se.ValidationResult.ToString(), "([a-z])([A-Z])", "$1 $2"));
            //}
        }

        /// <summary>
        /// Metodo invocato ad autenticazione già avvenuta, prima di invare la risposta con il token al client.
        /// Aggiunge alla risposta (in chiaro) le properties settate nell'<see cref="AuthenticationTicket"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
        
        /// <summary>
        /// Implemetato per evitare che il mancato inserimento dell'header clientid nella richiesta impedisca
        /// l'autenticazione fatta nel <see cref="GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext)"/> 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Valida il client_id passato e la redirectUri richiesta dove il client chiede di essere reindirizzato
        /// ad autenticazione avvenuta.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }
            context.Validated();
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Crea le <see cref="AuthenticationProperties"/> a partire dello username del cliente
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}