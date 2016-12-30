using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApi.Binder.Filter
{
    public sealed class GrantAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
            var content = new MessageResponseUnAuthorize
            {
                Message = "Utente non autorizzato",
                Motivation = "Non è stato inserito l'access_token di tipo Bearer",
                RequestId = Guid.NewGuid().ToString("D")
            };

            Log.Error("Utente non autenticato {@error}", content);

            MediaTypeFormatter formatterToUse = GlobalConfiguration.Configuration.Formatters.FirstOrDefault();
            if (actionContext.ControllerContext.Request.Headers.Accept.Any(x => x.MediaType == "application/xml"))
                formatterToUse = new XmlMediaTypeFormatter();
            if (actionContext.ControllerContext.Request.Headers.Accept.Any(x => x.MediaType == "application/json"))
                formatterToUse = new JsonMediaTypeFormatter();
            
            actionContext.Response.Content = new System.Net.Http.ObjectContent<MessageResponseUnAuthorize>(content, formatterToUse);
        }
    }

    public class MessageResponseUnAuthorize
    {
        public string Message { get; set; }
        public string Motivation { get; set; }
        public string RequestId { get; set; }
    }
}