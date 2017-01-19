using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Moq;
using NUnit.Framework;
using SFA.DAS.Audit.Client.Web.MessageBuilders;
using SFA.DAS.Audit.Types;

namespace SFA.DAS.Audit.Client.Web.UnitTests.MessageBuilders.ChangedByMessageBuilderTests
{
    public class WhenBuilding
    {
        private const string OriginIpAddress = "1.2.3.4";
        private const string UserId = "USER001";
        private const string UserEmail = "user.one@unit.tests";

        private Mock<HttpContextBase> _httpContext;
        private ChangedByMessageBuilder _builder;
        private AuditMessage _message;

        [SetUp]
        public void Arrange()
        {
            // Need to reset these as they are static and can be changed by tests
            WebMessageBuilders.UserIdClaim = ClaimTypes.NameIdentifier;
            WebMessageBuilders.UserEmailClaim = ClaimTypes.Email;

            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.UserHostAddress)
                .Returns(OriginIpAddress);
            _httpContext = new Mock<HttpContextBase>();
            _httpContext.Setup(c => c.Request)
                .Returns(request.Object);
            _httpContext.Setup(c => c.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, UserId),
                    new Claim(ClaimTypes.Email, UserEmail)
                }, "UnitTests")));

            _builder = new ChangedByMessageBuilder(_httpContext.Object);

            _message = new AuditMessage();
        }


        [Test]
        public void ThenItShouldSetChangedByToAnInstanceOfActor()
        {
            // Act
            _builder.Build(_message);

            // Assert
            Assert.IsNotNull(_message.ChangedBy);
        }


        [Test]
        public void ThenItShouldSetOriginIpAddress()
        {
            // Act
            _builder.Build(_message);

            // Assert
            Assert.AreEqual(OriginIpAddress, _message.ChangedBy.OriginIpAddress);
        }


        [Test]
        public void ThenItShouldSetUserIdFromDefaultClaim()
        {
            // Act
            _builder.Build(_message);

            // Assert
            Assert.AreEqual(UserId, _message.ChangedBy.Id);
        }

        [Test]
        public void ThenItShouldSetUserIdFromNonDefaultClaim()
        {
            // Arrange
            WebMessageBuilders.UserIdClaim = "UserId";
            _httpContext.Setup(c => c.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(WebMessageBuilders.UserIdClaim, UserId),
                    new Claim(ClaimTypes.Email, UserEmail)
                }, "UnitTests")));

            // Act
            _builder.Build(_message);

            // Assert
            Assert.AreEqual(UserId, _message.ChangedBy.Id);
        }

        [Test]
        public void ThenItShouldNotSetUserIdWhenNoUser()
        {
            // Arrange
            _httpContext.Setup(c => c.User)
                .Returns<IPrincipal>(null);

            // Act
            _builder.Build(_message);

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(_message.ChangedBy.Id));
        }

        [Test]
        public void ThenItShouldNotSetUserIdWhenUserIdClaimIsNotSet()
        {
            // Arrange
            WebMessageBuilders.UserIdClaim = null;

            // Act
            _builder.Build(_message);

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(_message.ChangedBy.Id));
        }

        [Test]
        public void ThenItShouldThrowAnExceptionIfAUserExistsButUserIdClaimIsMissing()
        {
            // Arrange
            WebMessageBuilders.UserIdClaim = "NoSuchClaim";

            // Act + Assert
            var ex = Assert.Throws<InvalidContextException>(() => _builder.Build(_message));
            Assert.AreEqual("User does not have claim NoSuchClaim to populate AuditMessage.ChangedBy.Id", ex.Message);
        }


        [Test]
        public void ThenItShouldSetUserEmailFromDefaultClaim()
        {
            // Act
            _builder.Build(_message);

            // Assert
            Assert.AreEqual(UserEmail, _message.ChangedBy.EmailAddress);
        }

        [Test]
        public void ThenItShouldSetUserEmailFromNonDefaultClaim()
        {
            // Arrange
            WebMessageBuilders.UserEmailClaim = "UserEmail";
            _httpContext.Setup(c => c.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, UserId),
                    new Claim(WebMessageBuilders.UserEmailClaim, UserEmail)
                }, "UnitTests")));

            // Act
            _builder.Build(_message);

            // Assert
            Assert.AreEqual(UserEmail, _message.ChangedBy.EmailAddress);
        }

        [Test]
        public void ThenItShouldNotSetUserEmailWhenNoUser()
        {
            // Arrange
            _httpContext.Setup(c => c.User)
                .Returns<IPrincipal>(null);

            // Act
            _builder.Build(_message);

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(_message.ChangedBy.EmailAddress));
        }

        [Test]
        public void ThenItShouldNotSetUserEmailWhenUserIdClaimIsNotSet()
        {
            // Arrange
            WebMessageBuilders.UserEmailClaim = null;

            // Act
            _builder.Build(_message);

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(_message.ChangedBy.EmailAddress));
        }

        [Test]
        public void ThenItShouldThrowAnExceptionIfAUserExistsButUserEmailClaimIsMissing()
        {
            // Arrange
            WebMessageBuilders.UserEmailClaim = "NoSuchClaim";

            // Act + Assert
            var ex = Assert.Throws<InvalidContextException>(() => _builder.Build(_message));
            Assert.AreEqual("User does not have claim NoSuchClaim to populate AuditMessage.ChangedBy.EmailAddress", ex.Message);
        }
    }
}
