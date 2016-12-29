using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace WebApiVersioning.Controllers
{
    public class BaseController : ApiController
    {
        public BaseController()
        {
            Log.Debug("Test Controller");
        }
        
        protected override UnauthorizedResult Unauthorized(IEnumerable<AuthenticationHeaderValue> challenges)
        {
            challenges.ToList().ForEach(c => 
            {
                Log.Verbose("Utente non autorizzato {@Scheme} - {@Parameter}. {@RequestMessage}", c.Scheme, c.Parameter, new
                {
                    Content = Request.Content,
                    Headers = Request.Headers,
                    Uri = Request.RequestUri,
                    Method = Request.Method,
                    Properties = Request.Properties
                });
            });

            return base.Unauthorized(challenges);
        }
    }
}
