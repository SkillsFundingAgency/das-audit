using System;
using System.Threading.Tasks;
using SFA.DAS.Audit.Types;

namespace SFA.DAS.Audit.Client
{
    public class AuditApiClient : IAuditApiClient
    {
        private readonly SecureHttpClient _httpClient;
        private readonly AuditApiConfiguration _configuration;

        public AuditApiClient(AuditApiConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new SecureHttpClient(configuration);
        }

        internal AuditApiClient(AuditApiConfiguration configuration, SecureHttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task Audit(AuditMessage message)
        {
            var baseUrl = _configuration.ApiBaseUrl.EndsWith("/")
                ? _configuration.ApiBaseUrl
                : _configuration.ApiBaseUrl + "/";
            var url = $"{baseUrl}api/audit";

            await _httpClient.PostAsync(url, message);
        }
    }
}
