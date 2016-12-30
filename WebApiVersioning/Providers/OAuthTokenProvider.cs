using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Serilog;

namespace WebApi.Binder.Providers
{
    /// <summary>
    /// Utilizzato per settare la scadenza del token "custom" (in questo caso alla mezzanotte del giorno successivo alla creazione del token)
    /// e per ritornare un json di risposta in caso di token non valido o scaduto.
    /// </summary>
    public class OAuthTokenProvider : AuthenticationTokenProvider
    {
        public override void Create(AuthenticationTokenCreateContext context)
        {
            context.Ticket.Properties.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1).Date;
            context.SetToken(context.SerializeTicket());
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);

            string msg = "{{\"Message\": {0}}}";
            if (context.Ticket == null)
            {
                Log.Error("Errore deserializzazione token");
                context.Response.Write(string.Format(msg, "token non corretto"));
                context.Response.StatusCode = 412;
            }   
            else if(DateTimeOffset.Compare(DateTimeOffset.UtcNow, context.Ticket.Properties.ExpiresUtc.Value) > 0)
            {
                Log.Error("token scaduto");
                context.Response.Write(string.Format(msg, "token scaduto"));
                context.Response.StatusCode = 416;
            }
        }
    }
}