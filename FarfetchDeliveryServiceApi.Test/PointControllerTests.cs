using FarfetchDeliveryServiceApi.Controllers;
using FarfetchDeliveryServiceGraphRepository.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace FarfetchDeliveryServiceApi.Test
{
    /// <summary>
    /// Tests for the PointController
    /// </summary>
    public class PointControllerTests
    {
        private readonly Mock<IPointRepository> _mockRepository;
        private readonly PointController _controller;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PointControllerTests()
        {
            _mockRepository = new Mock<IPointRepository>();
            _controller = new PointController(_mockRepository.Object);
        }

        /// <summary>
        /// Test Get method from PointController with success
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceApi_PointController_Get_WithSuccess()
        {
            List<string> points = new List<string>() { "A", "B", "C" };

            _mockRepository.Setup(m => m.Get()).ReturnsAsync(points);

            var result = _controller.Get().Result;

            OkObjectResult finalResult = result as OkObjectResult;

            Assert.NotNull(finalResult);
            Assert.NotNull(finalResult.Value);
            Assert.Equal(StatusCodes.Status200OK, finalResult.StatusCode);

            List<string> pointsResult = (List<string>)finalResult.Value;

            Assert.Equal(points.Count, pointsResult.Count);
        }

        /// <summary>
        /// Test Post method from PointController with success
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceApi_PointController_Post_WithSuccess()
        {
            _mockRepository.Setup(m => m.CheckIfExists(It.IsAny<string>())).ReturnsAsync(false);

            var result = _controller.Post("").Result;

            _mockRepository.Verify(m => m.Add(It.IsAny<string>()), Times.Once);
            _mockRepository.Verify(m => m.CheckIfExists(It.IsAny<string>()), Times.Once);

            OkResult finalResult = result as OkResult;

            Assert.NotNull(finalResult);
            Assert.Equal(StatusCodes.Status200OK, finalResult.StatusCode);
        }

        /// <summary>
        /// Test Post method from PointController with fail
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceApi_PointController_Post_PointAlreadyExists()
        {
            _mockRepository.Setup(m => m.CheckIfExists("teste")).ReturnsAsync(true);

            var result = _controller.Post("teste").Result;

            _mockRepository.Verify(m => m.Add(It.IsAny<string>()), Times.Never);
            _mockRepository.Verify(m => m.CheckIfExists(It.IsAny<string>()), Times.Once);

            ObjectResult finalResult = result as ObjectResult;

            Assert.NotNull(finalResult);
            Assert.Equal(StatusCodes.Status400BadRequest, finalResult.StatusCode);
        }

        /// <summary>
        /// Test Delete method from PointController with success
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceApi_PointController_Delete_WithSuccess()
        {
            _mockRepository.Setup(m => m.CheckIfExists(It.IsAny<string>())).ReturnsAsync(true);

            var result = _controller.Delete("").Result;

            _mockRepository.Verify(m => m.Delete(It.IsAny<string>()), Times.Once);
            _mockRepository.Verify(m => m.CheckIfExists(It.IsAny<string>()), Times.Once);

            OkResult finalResult = result as OkResult;

            Assert.NotNull(finalResult);
            Assert.Equal(StatusCodes.Status200OK, finalResult.StatusCode);
        }

        /// <summary>
        /// Test Delete method from PointController with fail
        /// </summary>
        [Fact]
        public void FarfetchDeliveryServiceApi_PointController_Delete_PointNotExists()
        {
            _mockRepository.Setup(m => m.CheckIfExists("teste")).ReturnsAsync(false);

            var result = _controller.Delete("teste").Result;

            _mockRepository.Verify(m => m.Delete(It.IsAny<string>()), Times.Never);
            _mockRepository.Verify(m => m.CheckIfExists(It.IsAny<string>()), Times.Once);

            ObjectResult finalResult = result as ObjectResult;

            Assert.NotNull(finalResult);
            Assert.Equal(StatusCodes.Status400BadRequest, finalResult.StatusCode);
        }
    }
}
