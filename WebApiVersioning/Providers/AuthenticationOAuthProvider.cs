using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;


namespace WebApiVersioning.Providers
{
    /// <summary>
    /// Provider per la verifica del token e le operazioni preliminari.
    /// </summary>
    public class AuthenticationOAuthProvider : OAuthBearerAuthenticationProvider
    {
        
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
            //TODO: validare la data di scadenza del token
            if (context.IsValidated)
            {
                //var p = await _securityService.CreatePrincipalForUsernameAsync(context.Ticket.Identity.Name);
                //foreach(var claim in context.Ticket.Identity.Claims)
                //{
                //    context.Ticket.Identity.RemoveClaim(claim);
                //}                
                //context.Ticket.Identity.AddClaims(((ClaimsIdentity)p.Identity).Claims);
            }
            await base.ValidateIdentity(context);
        }
    }
}