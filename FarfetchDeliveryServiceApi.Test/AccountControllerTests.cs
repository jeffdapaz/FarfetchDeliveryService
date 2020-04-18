using FarfetchDeliveryServiceApi.Controllers;
using FarfetchDeliveryServiceApi.Models;
using FarfetchDeliveryServiceApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FarfetchDeliveryServiceApi.Test
{
    /// <summary>
    /// Tests for the AccountController
    /// </summary>
    public class AccountControllerTests
    {
        private readonly Mock<IUsersServices> _mockUsersService;
        private readonly AccountController _controller;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AccountControllerTests()
        {
            _mockUsersService = new Mock<IUsersServices>();
            _controller = new AccountController(_mockUsersService.Object);
        }

        /// <summary>
        /// Teste the Login method with success
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceApi_AccountController_Login_WithSuccess()
        {
            _mockUsersService.Setup(m => m.Authenticate(It.IsAny<User>())).Returns("teste");

            var result = _controller.Login(null);

            OkObjectResult finalResult = result as OkObjectResult;

            Assert.NotNull(finalResult);
            Assert.Equal(StatusCodes.Status200OK, finalResult.StatusCode);
        }

        /// <summary>
        /// Teste the Login method with fail
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceApi_AccountController_Login_WithFail()
        {
            _mockUsersService.Setup(m => m.Authenticate(It.IsAny<User>())).Returns(string.Empty);

            var result = _controller.Login(null);

            ObjectResult finalResult = result as ObjectResult;

            Assert.NotNull(finalResult);
            Assert.Equal(StatusCodes.Status403Forbidden, finalResult.StatusCode);
        }
    }
}
