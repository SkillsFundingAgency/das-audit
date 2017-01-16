using System.Web.Http;
using SFA.DAS.Audit.Web.Plumbing.WebApi;

namespace SFA.DAS.Audit.Web.Controllers
{
    public class HealthCheckController : ApiController
    {
        [VersionedRoute("api/HealthCheck", 1)]
        [Route("api/v1/HealthCheck")]
        public IHttpActionResult GetStatus()
        {
            return Ok();
        }
    }
}
