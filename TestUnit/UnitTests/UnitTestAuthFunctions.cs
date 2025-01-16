using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogicLayer.Entitys;
using Moq;
using LogicLayer.Classes;
using LogicLayer.IRepos;
using LogicLayer.Services;
using Microsoft.Extensions.Configuration;
using LogicLayer.Entities;


namespace TestUnit.UnitTests
{
    [TestClass]
    public class UnitTestAuthFunctions
    {
        private AuthService _authService;
        private Mock<IAuthRepo> _moqAuthRepo;
        private Mock<IConfiguration> _moqConfiguration;


        [TestInitialize]
        public void Setup()
        {
            _moqAuthRepo = new Mock<IAuthRepo>();
            _moqConfiguration = new Mock<IConfiguration>();

            _authService = new AuthService(_moqAuthRepo.Object, _moqConfiguration.Object);
        }

        [TestMethod]
        public void RegisterUser_ThrowsException_WhenUserAlreadyExists()
        {
            // Arrange
            string email = "existinguser@example.com";
            _moqAuthRepo.Setup(repo => repo.GetUserByEmail(email)).Returns(new User { email = email });

            // Act & Assert
            Assert.ThrowsException<Exception>(() => _authService.RegisterUser("John Doe", email, "password123"));
        }

        [TestMethod]
        public void RegisterUser_SavesUser_WhenUserDoesNotExist()
        {
            // Arrange
            string email = "newuser@example.com";
            _moqAuthRepo.Setup(repo => repo.GetUserByEmail(email)).Returns((User)null);

            // Act
            _authService.RegisterUser("John Doe", email, "password123");

            // Assert
            _moqAuthRepo.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Once);
        }


        [TestMethod]
        public void LoginUser_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            string email = "nonexistentuser@example.com";
            _moqAuthRepo.Setup(repo => repo.GetUserByEmail(email)).Returns((User)null);

            // Act
            var result = _authService.LoginUser(email, "password123");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void LoginUser_ReturnsNull_WhenPasswordIsInvalid()
        {
            // Arrange
            string email = "validuser@example.com";
            string password = "wrongpassword";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword("correctpassword");
            var user = new User
            {
                name = "Valid User",
                email = email,
                password = hashedPassword,
                role = new Role { id = 1, name = "Employee" }
            };

            _moqAuthRepo.Setup(repo => repo.GetUserByEmail(email)).Returns(user);

            // Act
            var result = _authService.LoginUser(email, password);

            // Assert
            Assert.IsNull(result);
        }
    }
}
