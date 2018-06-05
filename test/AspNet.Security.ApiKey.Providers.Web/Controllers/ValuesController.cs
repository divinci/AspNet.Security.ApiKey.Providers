using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.Security.ApiKey.Providers.Web.Controllers
{
    public class ValuesController : Controller
    {
        [HttpGet, Route("api/authenticated/values")]
        [Authorize]
        public IEnumerable<string> Auth() => new[] { "value1", "value2" };

        [HttpGet, Route("api/anonymous/values")]
        public IEnumerable<string> Anon() => new[] { "value1", "value2" };
    }
}
