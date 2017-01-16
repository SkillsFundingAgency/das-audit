using System.Threading.Tasks;
using System.Web.Http.Results;
using NUnit.Framework;

namespace SFA.DAS.Audit.Web.UnitTests.Controllers.AuditControllerTests
{
    public class WhenWritingAudit : AuditControllerTestBase
    {
        [SetUp]
        public override void Arrange()
        {
            base.Arrange();
        }

        [Test]
        public async Task ThenItShouldReturnAnOkResult()
        {
            // Act
            var actual = await _controller.WriteAudit(_goodMessage);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<OkResult>(actual);
        }
    }
}
