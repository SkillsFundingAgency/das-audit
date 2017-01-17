using System.Security.Claims;
using System.Web;
using SFA.DAS.Audit.Client.Web.MessageBuilders;

namespace SFA.DAS.Audit.Client.Web
{
    public static class WebMessageBuilders
    {
        public static string UserIdClaim = ClaimTypes.NameIdentifier;
        public static string UserEmailClaim = ClaimTypes.Email;

        public static void Register()
        {
            AuditMessageFactory.RegisterBuilder(message =>
            {
                var httpContext = new HttpContextWrapper(HttpContext.Current);
                new ChangedByMessageBuilder(httpContext).Build(message);
            });
        }
    }
}
