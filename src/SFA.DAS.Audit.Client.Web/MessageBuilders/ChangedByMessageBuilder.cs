using System.Linq;
using System.Security.Claims;
using System.Web;
using SFA.DAS.Audit.Types;

namespace SFA.DAS.Audit.Client.Web.MessageBuilders
{
    internal class ChangedByMessageBuilder
    {
        private readonly HttpContextBase _httpContext;

        public ChangedByMessageBuilder(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public void Build(AuditMessage message)
        {
            message.ChangedBy = new Actor();
            SetOriginIpAddess(message.ChangedBy);
            SetUserIdAndEmail(message.ChangedBy);
        }

        private void SetOriginIpAddess(Actor actor)
        {
            actor.OriginIpAddress = _httpContext.Request.UserHostAddress == "::1"
                ? "127.0.0.1"
                : _httpContext.Request.UserHostAddress;
        }

        private void SetUserIdAndEmail(Actor actor)
        {
            var user = _httpContext.User as ClaimsPrincipal;
            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return;
            }

            if (!string.IsNullOrEmpty(WebMessageBuilders.UserIdClaim))
            {
                var claim = user.Claims.FirstOrDefault(c => c.Type.Equals(WebMessageBuilders.UserIdClaim, System.StringComparison.CurrentCultureIgnoreCase));
                if (claim == null)
                {
                    throw new InvalidContextException($"User does not have claim {WebMessageBuilders.UserIdClaim} to populate AuditMessage.ChangedBy.Id");
                }
                actor.Id = claim.Value;
            }

            if (!string.IsNullOrEmpty(WebMessageBuilders.UserEmailClaim))
            {
                var claim = user.Claims.FirstOrDefault(c => c.Type.Equals(WebMessageBuilders.UserEmailClaim, System.StringComparison.CurrentCultureIgnoreCase));
                if (claim == null)
                {
                    throw new InvalidContextException($"User does not have claim {WebMessageBuilders.UserEmailClaim} to populate AuditMessage.ChangedBy.EmailAddress");
                }
                actor.EmailAddress = claim.Value;
            }
        }
    }
}
