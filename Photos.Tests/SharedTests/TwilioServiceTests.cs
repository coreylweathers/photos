using Microsoft.Extensions.Options;
using Moq;
using Photos.Shared.Models;
using Photos.Shared.Services;
using System.Threading.Tasks;
using Xunit;

namespace Photos.Tests.SharedTests
{
    public class TwilioServiceTests
    {
        private ITwilioService _sut;
        private Mock<IOptionsMonitor<TwilioOptions>> _mockOptions = new Mock<IOptionsMonitor<TwilioOptions>>();

        public TwilioServiceTests()
        {
        }

        [Fact]
        public async Task PurchasePhoneNumber_WithValidAreaCode_ReturnsPhoneNumber()
        {
            // Arrange
            _mockOptions.SetupGet(x => x.CurrentValue).Returns(new TwilioOptions
            {
                AccountSid = "AC0629FB275E074F7FB143C9B39F1AFDE0",
                AuthToken = "0629FB275E074F7FB143C9B39F1AFDE0"
            });
            _sut = new TwilioService(_mockOptions.Object);

            var areaCode = "510";

            // Act
            var result = await _sut.SearchPhoneNumber(areaCode);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Matches(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$", result);
        }

    }
}
