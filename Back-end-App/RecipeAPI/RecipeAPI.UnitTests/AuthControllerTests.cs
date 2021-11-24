using Microsoft.AspNetCore.Mvc;
using Moq;
using RecipeAPI.Controllers;
using RecipeAPIModel.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RecipeAPI.UnitTests
{
    public class AuthControllerTests
    {

        private readonly Mock<IDALUser> loginStub = new();
        //Formato de nombre de test UnitOfWork_StateUnderTest_SpectedBehavior
        private readonly Random rand = new();
        private LoginModel CreateRandomUser()
        {
            return new()
            {
                email = "ejemplo@gmail.com",
                password = "carlos25",
                returnSecureToken = true
            };
        }
        [Fact]
        public async Task Login_UnexistingUser_ReturnsUnauthorized()
        {
            //Arrange
            //var loginStub = new Mock<IDALUser>();

            loginStub.Setup(signin => signin.GetUser(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var controller = new AuthController(loginStub.Object);
            //Act
            var result = await controller.Login(new LoginModel());
            //Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Register_IncorrectModel_ReturnsBadRequest()
        {
            //Arrange
            var loginStub = new Mock<IDALUser>();

            loginStub.Setup(signup => signup.InsertUser(It.IsAny<User>()))
                .ReturnsAsync((bool)false);

            var controller = new AuthController(loginStub.Object);
            controller.ModelState.AddModelError("Title", "Required");
            //Act
            var result = await controller.SignUp(new LoginModel());
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Register_NullModel_ReturnsBadRequest()
        {
            //Arrange
            var loginStub = new Mock<IDALUser>();

            loginStub.Setup(signup => signup.InsertUser(It.IsAny<User>()))
                .ReturnsAsync((bool)false);

            var controller = new AuthController(loginStub.Object);

            //Act
            var result = await controller.SignUp(null);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Register_EmptyModel_ReturnsBadRequest()
        {
            //Arrange
            var loginStub = new Mock<IDALUser>();

            loginStub.Setup(signup => signup.InsertUser(It.IsAny<User>()))
                .ReturnsAsync((bool)false);

            var controller = new AuthController(loginStub.Object);

            //Act
            var result = await controller.SignUp(new LoginModel());
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Register_CorrectParam_ReturnsCreated()
        {
            //Arrange
            var loginStub = new Mock<IDALUser>();

            loginStub.Setup(signup => signup.InsertUser(It.IsAny<User>()))
                .ReturnsAsync((bool)true);

            var controller = new AuthController(loginStub.Object);
            var user = CreateRandomUser();
            //Act
            var result = await controller.SignUp(user);
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

    }
}
