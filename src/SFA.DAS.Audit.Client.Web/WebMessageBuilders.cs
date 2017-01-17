using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using SFA.DAS.Audit.Types;

namespace SFA.DAS.Audit.Client.Web
{
    public static class WebMessageBuilders
    {
        public static string UserIdClaim = ClaimTypes.NameIdentifier;
        public static string UserEmailClaim = ClaimTypes.Email;

        public static void Register()
        {
            AuditMessageFactory.RegisterBuilder(AttachClientIpAddress);
            AuditMessageFactory.RegisterBuilder(AttachUserClaims);
        }

        private static void AttachClientIpAddress(AuditMessage message)
        {
            if (message.ChangedBy == null)
            {
                message.ChangedBy = new Actor();
            }

            var context = HttpContext.Current;
            message.ChangedBy.OriginIpAddress = context.Request.UserHostAddress;
        }
        private static void AttachUserClaims(AuditMessage message)
        {
            if (string.IsNullOrEmpty(UserIdClaim) && string.IsNullOrEmpty(UserEmailClaim))
            {
                return;
            }

            if (message.ChangedBy == null)
            {
                message.ChangedBy = new Actor();
            }

            var user = HttpContext.Current.User as ClaimsPrincipal;
            if (user == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(UserIdClaim))
            {
                var claim = user.Claims.FirstOrDefault(c => c.Type.Equals(UserIdClaim, StringComparison.CurrentCultureIgnoreCase));
                if (claim == null)
                {
                    throw new Exception($"User does not have claim {UserIdClaim} to attach to AuditMessage");
                }
                message.ChangedBy.Id = claim.Value;
            }
            if (!string.IsNullOrEmpty(UserEmailClaim))
            {
                var claim = user.Claims.FirstOrDefault(c => c.Type.Equals(UserEmailClaim, StringComparison.CurrentCultureIgnoreCase));
                if (claim == null)
                {
                    throw new Exception($"User does not have claim {UserEmailClaim} to attach to AuditMessage");
                }
                message.ChangedBy.EmailAddress = claim.Value;
            }
        }
    }
}
