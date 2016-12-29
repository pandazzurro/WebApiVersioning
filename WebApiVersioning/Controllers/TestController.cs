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
    [Authorize]
    public class TestController : ApiController
    {
        protected override UnauthorizedResult Unauthorized(IEnumerable<AuthenticationHeaderValue> challenges)
        {
            challenges.ToList().ForEach(c => 
            {
                Log.Verbose("Utente non autorizzato {@Scheme} - {@Parameter}", c.Scheme, c.Parameter);
            });            
            return base.Unauthorized(challenges);
        }
        [HttpGet]
        public async Task<IHttpActionResult> Prova()
        {
            return Ok();
        }
    }
}
