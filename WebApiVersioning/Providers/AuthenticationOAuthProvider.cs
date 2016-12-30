using Microsoft.Owin.Security.OAuth;
using Serilog;
using System.Security.Claims;
using System.Threading.Tasks;


namespace WebApi.Binder.Providers
{
    /// <summary>
    /// Provider per la verifica del token e le operazioni preliminari.
    /// </summary>
    public class AuthenticationOAuthProvider : OAuthBearerAuthenticationProvider
    {
        private readonly SecurityService _securityService;

        public AuthenticationOAuthProvider(SecurityService securityService)
        {
            _securityService = securityService;
        }

        /// <summary>
        /// Valida il token per una richiesta. Il token viene generato da: <see cref="ApplicationOAuthProvider"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <remarks>
        /// Verifica il token rilasciato e inserisce i claims corretti per l'utente autenticato.
        /// Verifico la scadenza del token.
        /// La procedura avviene ad ogni richiesta del client.
        /// </remarks>
        public override async Task ValidateIdentity(OAuthValidateIdentityContext context)
        {
            if (context.IsValidated)
            {
                string username = context.Ticket.Identity.Name;
                var p = _securityService.CreatePrincipalForUsername(username);
                if (p == null)
                {
                    Log.Warning($"Errore creazione principal per la richiesta: {username} non esiste");
                    context.SetError("invalid_token", "Il token fornito non è valido");
                    return;
                }

                foreach (var claim in context.Ticket.Identity.Claims)
                {
                    context.Ticket.Identity.RemoveClaim(claim);
                }
                context.Ticket.Identity.AddClaims(((ClaimsIdentity)p.Identity).Claims);
            }
            await base.ValidateIdentity(context);
        }        
    }
}