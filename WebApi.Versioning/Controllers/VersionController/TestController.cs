using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace WebApi.Versioning.QueryStringVersion
{
    public class NameTestV1Controller : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> Prova()
        {
            return Ok($"Prova - {GetType().FullName}");
        }
    }

    public class NameTestV2Controller : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> Prova()
        {
            return Ok($"Prova - {GetType().FullName}");
        }
    }
}
