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
        private readonly SecurityService _securityService;

        /// <summary>
        /// Crea una istanza del provider settando il SecurityService utilizzato per le fasi di autenticazione.
        /// </summary>
        /// <param name="securityService"></param>
        public ApplicationOAuthProvider(SecurityService securityService)
        {
            _publicClientId = "self";
            _securityService = securityService;
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
            try
            {
                UserValidationResult result = _securityService.ValidateUser(context.UserName, context.Password);

                switch (result)
                {
                    case UserValidationResult.InvalidCredentials:
                        context.SetError("invalid_grant", "Credenziali non valide");
                        break;
                    case UserValidationResult.UserNotExtists:
                        context.SetError("invalid_grant", "Utente non esiste");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", "Errore di validazine dell'utente");
            }

            if (context.HasError)
                return;

            var p = _securityService.CreatePrincipalForUsername(context.UserName);
            if (p == null)
            {
                context.SetError("invalid_grant", "Utente non esiste.");        //Teoricamente qua non ci arriva mai (vedi controllo sopra)
                return;
            }

            var claimsIdentity = new ClaimsIdentity(p.FindAll(x => x.Type == ClaimTypes.Name), OAuthDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(context.UserName);
            AuthenticationTicket ticket = new AuthenticationTicket(claimsIdentity, properties);
            context.Validated(ticket);
        }

       
        /// <summary>
        /// Metodo invocato ad autenticazione già avvenuta, prima di invare la risposta con il token al client.
        /// Aggiunge alla risposta (in chiaro) le properties settate nell'<see cref="AuthenticationTicket"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
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