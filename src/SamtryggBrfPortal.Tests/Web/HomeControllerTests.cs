using System;
using System.Diagnostics;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SamtryggBrfPortal.Web.Controllers;
using SamtryggBrfPortal.Web.Models;

namespace SamtryggBrfPortal.Tests.Web
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_mockLogger.Object);
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName); // Default view
        }

        [Fact]
        public void About_ReturnsViewResult()
        {
            // Act
            var result = _controller.About();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName); // Default view
        }

        [Fact]
        public void Contact_ReturnsViewResult()
        {
            // Act
            var result = _controller.Contact();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName); // Default view
        }

        [Fact]
        public void Error_ReturnsViewResultWithErrorViewModel()
        {
            // Act
            var result = _controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
            Assert.NotNull(model.RequestId);
        }
    }
}
