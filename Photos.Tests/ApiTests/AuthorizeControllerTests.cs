using Microsoft.AspNetCore.Mvc;
using Moq;
using Photos.API.Controllers;
using Photos.Shared.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Photos.Tests.ApiTests
{
    public class AuthorizeControllerTests
    {
        private TwilioController _sut;
        private Mock<ITwilioService> mockTwilioService;
        private Mock<IStorageService> mockStorageService;

        public AuthorizeControllerTests()
        {
        }

        [Fact]
        public void Get_WithNoParams_ReturnsValidAuthorizeResult()
        {
            // ARRANGE
            var accountSid = "AC24504e9f04164744a970e26bb81cb2e8";
            mockTwilioService = new Mock<ITwilioService>();
            mockStorageService = new Mock<IStorageService>();
            _sut = new TwilioController(mockTwilioService.Object, mockStorageService.Object);

            // ACT
            var result = _sut.Authorize(accountSid) as NoContentResult;

            // ASSERT
            Assert.True(typeof(NoContentResult) == result.GetType());
        }

        [Fact]
        public void Get_WithParams_ReturnsValidDeauthroizeResult()
        {
            // ARRANGE
            mockTwilioService = new Mock<ITwilioService>();
            mockStorageService = new Mock<IStorageService>();
            _sut = new TwilioController(mockTwilioService.Object, mockStorageService.Object);

            // ACT
            var result = _sut.Deauthorize() as NoContentResult;

            // ASSERT
            Assert.True(typeof(NoContentResult) == result.GetType());
        }
    }
}
