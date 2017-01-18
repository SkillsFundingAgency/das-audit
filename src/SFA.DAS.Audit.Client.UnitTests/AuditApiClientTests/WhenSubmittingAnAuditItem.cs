using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Audit.Test.Shared.ObjectMothers;
using SFA.DAS.Audit.Types;

namespace SFA.DAS.Audit.Client.UnitTests.AuditApiClientTests
{
    public class WhenSubmittingAnAuditItem
    {
        private Mock<SecureHttpClient> _httpClient;
        private AuditApiClient _apiclient;

        private const string ExpectedApiBaseUrl = "http://test.local.url";

        [SetUp]
        public void Arrange()
        {
            _httpClient = new Mock<SecureHttpClient>();

            _apiclient = new AuditApiClient(new AuditApiConfiguration {ApiBaseUrl = ExpectedApiBaseUrl}, _httpClient.Object);
        }

        [Test]
        public async Task ThenTheApiIsCalledWithTheCorrectUrl()
        {
            //Act
            await _apiclient.Audit(new AuditMessage());

            //Assert
            _httpClient.Verify(x=>x.PostAsync($"{ExpectedApiBaseUrl}/api/audit",It.IsAny<AuditMessage>()));
        }

        [Test]
        public async Task ThenTheAuditMessageIsUsedForThePostObject()
        {
            //Arrange
            var expectedAuditMessage = AuditMessageMother.Create();

            //Act
            await _apiclient.Audit(expectedAuditMessage);

            //Assert
            _httpClient.Verify(x=>x.PostAsync(It.IsAny<string>(),It.Is<AuditMessage>(c=>
                    c.Description.Equals(expectedAuditMessage.Description) &&
                    c.AffectedEntity.Id.Equals(expectedAuditMessage.AffectedEntity.Id) &&
                    c.AffectedEntity.Type.Equals(expectedAuditMessage.AffectedEntity.Type) &&
                    c.ChangeAt.Equals(expectedAuditMessage.ChangeAt) &&
                    c.ChangedBy.EmailAddress.Equals(expectedAuditMessage.ChangedBy.EmailAddress) &&
                    c.ChangedBy.Id.Equals(expectedAuditMessage.ChangedBy.Id) &&
                    c.ChangedBy.OriginIpAddress.Equals(expectedAuditMessage.ChangedBy.OriginIpAddress) &&
                    c.Source.Equals(expectedAuditMessage.Source)
                    )));
        }
    }
}
