using Microsoft.AspNetCore.Mvc;
using Photos.API.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Photos.Tests.ApiTests
{
    public class AuthorizeControllerTests
    {
        private readonly TwilioController _sut;

        public AuthorizeControllerTests()
        {
            _sut = new TwilioController();
        }

        [Fact]
        public void Get_WithNoParams_ReturnsValidAuthorizeResult()
        {
            // ARRANGE
            var accountSid = "AC24504e9f04164744a970e26bb81cb2e8";

            // ACT
            var result = _sut.Authorize(accountSid) as NoContentResult;

            // ASSERT
            Assert.True(typeof(NoContentResult) == result.GetType());
        }

        [Fact]
        public void Get_WithParams_ReturnsValidDeauthroizeResult()
        {
            // ARRANGE

            // ACT
            var result = _sut.Deauthorize() as NoContentResult;

            // ASSERT
            Assert.True(typeof(NoContentResult) == result.GetType());
        }
    }
}
