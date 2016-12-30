using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace WebApi.Versioning.VersionController
{

    [RoutePrefix("api/route")]
    public class TestV1Controller : ApiController
    {
        public TestV1Controller()
        {

        }

        [Route("v3/test")]
        [Route("v1/test")]
        public async Task<IHttpActionResult> Get()
        {
            return Ok($"Prova - {GetType().FullName}");
        }

        [Route("v2/test")]
        public async Task<IHttpActionResult> GetV2()
        {
            return Ok($"Prova - {GetType().FullName}");
        }
    }

    //public class TestV2Controller : ApiController
    //{
    //    public TestV2Controller()
    //    {
    //    }

    //    [Route("api/route/v2/test")]
    //    public async Task<IHttpActionResult> Get()
    //    {
    //        return Ok($"Prova - {GetType().FullName}");
    //    }
    //}
}
