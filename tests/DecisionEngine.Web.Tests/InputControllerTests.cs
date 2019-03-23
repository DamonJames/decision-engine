using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System;
using DecisionEngine.Web.Controllers;
using DecisionEngine.Services.Interfaces;
using DecisionEngine.Models;
using DecisionEngine.Web.Models;

namespace DecisionEngine.Web.Tests
{
    public class InputControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly InputController _inputController;

        public InputControllerTests()
        {
            _inputController = new InputController(_userServiceMock.Object);
        }

        [Fact]
        public async void SubmitAsync_InputViewModel_ReturnsResponseModelWithAcceptedStatus()
        {
            _userServiceMock.Setup(x => x.SubmitAsync(It.IsAny<User>()))
                .ReturnsAsync(Status.Accepted);

            var expectedResult = new JsonResult(
                new ResponseViewModel
                {
                    Status = (int)Status.Accepted
                });

            var sut = await _inputController.SubmitAsync(new InputViewModel());

            Assert.IsType<JsonResult>(sut);
            Assert.Equal(expectedResult.Value.ToString(), sut.Value.ToString());
        }

        [Fact]
        public async void SubmitAsync_InvalidInputViewModel_ReturnsResponseModelWithUserError()
        {
            var model = new InputViewModel
            {
                DateOfBirth = new DateTime(1),
                FirstName = null,
                LastName = null
            };

            var expectedResult = new JsonResult(
                new ResponseViewModel
                {
                    Status = (int)Status.UserError
                });

            var sut = await _inputController.SubmitAsync(new InputViewModel());

            Assert.IsType<JsonResult>(sut);
            Assert.Equal(expectedResult.Value.ToString(), sut.Value.ToString());
        }

        [Fact]
        public async void SubmitAsync_InputViewModel_ReturnsResponseModelWithError()
        {
            _userServiceMock.Setup(x => x.SubmitAsync(It.IsAny<User>()))
                .ReturnsAsync(Status.Errored);

            var expectedResult = new JsonResult(
                new ResponseViewModel
                {
                    Status = (int)Status.Errored
                });

            var sut = await _inputController.SubmitAsync(new InputViewModel());
            
            Assert.IsType<JsonResult>(sut);
            Assert.Equal(expectedResult.Value.ToString(), sut.Value.ToString());
        }
    }
}
