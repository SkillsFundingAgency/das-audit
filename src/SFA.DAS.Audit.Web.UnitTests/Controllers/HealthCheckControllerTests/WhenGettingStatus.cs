using System.Web.Http.Results;
using NUnit.Framework;
using SFA.DAS.Audit.Web.Controllers;

namespace SFA.DAS.Audit.Web.UnitTests.Controllers.HealthCheckControllerTests
{
    public class WhenGettingStatus
    {
        private HealthCheckController _controller;

        [SetUp]
        public void Arrange()
        {
            _controller = new HealthCheckController();
        }

        [Test]
        public void ThenItShouldReturnAnOkResult()
        {
            // Act
            var actual = _controller.GetStatus();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<OkResult>(actual);
        }
    }
}
