using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using WebApi.Versioning.Attributes;

namespace WebApi.Versioning.CustomAttribute
{
    [ApiVersion("1")]
    public class CustomTestV1Controller : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> Prova()
        {
            return Ok($"Prova - {GetType().FullName}");
        }
    }

    [ApiVersion("2")]
    public class CustomTestV2Controller : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> Prova()
        {
            return Ok($"Prova - {GetType().FullName}");
        }
    }
}
