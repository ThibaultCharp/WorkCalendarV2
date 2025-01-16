using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogicLayer.Entitys;
using LogicLayer.Entities;
using LogicLayer.Classes;
using LogicLayer.IRepos;
using Moq;

namespace TestUnit.UnitTests
{
    [TestClass]
    public class UnitTestsUserFunction
    {
        private UserService _userService;
        private Mock<IUserRepo> _moqUserRepo;

        [TestInitialize]
        public void Setup()
        {
            _moqUserRepo = new Mock<IUserRepo>();
            _userService = new UserService(_moqUserRepo.Object);
        }



        [TestMethod]
        public void CreateUserIfNotExisting_CreatesUser_WhenUserDoesNotExist()
        {
            // Arrange
            var testUser = new User
            {
                id = 1,
                name = "Test User",
                email = "test@example.com",
                role = new Role { id = 1, name = "Employee" }
            };

            _moqUserRepo.Setup(repo => repo.UserExists(testUser.id)).Returns(false);

            // Act
            _userService.CreateUserIfNotExisting(testUser);

            // Assert
            _moqUserRepo.Verify(repo => repo.CreateUser(testUser), Times.Once);
        }




        [TestMethod]
        public void CreateUserIfNotExisting_DoesNotCreateUser_WhenUserExists()
        {
            // Arrange
            var testUser = new User
            {
                id = 1,
                name = "Test User",
                email = "test@example.com",
                role = new Role { id = 1, name = "Employee" }
            };

            _moqUserRepo.Setup(repo => repo.UserExists(testUser.id)).Returns(true);

            // Act
            _userService.CreateUserIfNotExisting(testUser);

            // Assert
            _moqUserRepo.Verify(repo => repo.CreateUser(It.IsAny<User>()), Times.Never);
        }




        [TestMethod]
        public void GetAllUsersWithoutEmployer_ReturnsUsersWithoutEmployer()
        {
            // Arrange
            string testInput = "test@example.com";
            var expectedUsers = new List<User>
    {
        new User { id = 1, name = "Alice" },
        new User { id = 2, name = "Bob" }
    };

            _moqUserRepo.Setup(repo => repo.GetAllEmployeesWithoutEmployer(testInput)).Returns(expectedUsers);

            // Act
            var result = _userService.GetAllUsersWithoutEmployer(testInput);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUsers.Count, result.Count);
            Assert.AreEqual(expectedUsers[0].name, result[0].name);
        }



        [TestMethod]
        public void GetAllUsersWithCorrespondingRole_ReturnsUsersWithSpecificRole()
        {
            // Arrange
            string testRole = "Employee";
            var expectedUsers = new List<User>
    {
        new User { id = 1, name = "Alice", role = new Role { id = 1, name = "Employee" } },
        new User { id = 2, name = "Bob", role = new Role { id = 1, name = "Employee" } }
    };

            _moqUserRepo.Setup(repo => repo.GetAllUsersWithCorrespondingRoles(testRole)).Returns(expectedUsers);

            // Act
            var result = _userService.GetAllUsersWithCorrespondingRole(testRole);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUsers.Count, result.Count);
            Assert.AreEqual(expectedUsers[0].role.name, result[0].role.name);
        }


        [TestMethod]
        public void LinkUser_LinksUsersSuccessfully()
        {
            // Arrange
            string loggedInUserEmail = "admin@example.com";
            string targetUserEmail = "user@example.com";

            // Act
            _userService.LinkUser(loggedInUserEmail, targetUserEmail);

            // Assert
            _moqUserRepo.Verify(repo => repo.LinkUser(loggedInUserEmail, targetUserEmail), Times.Once);
        }



        [TestMethod]
        public void ChangeUserRole_ChangesRoleToEmployer_WhenMakeEmployerIsTrue()
        {
            // Arrange
            string testEmail = "test@example.com";
            int testUserId = 1;
            bool makeEmployer = true;

            // Act
            _userService.ChangeUserRole(testEmail, testUserId, makeEmployer);

            // Assert
            _moqUserRepo.Verify(repo => repo.ChangeUserRole(testEmail, testUserId, makeEmployer), Times.Once);
        }


        [TestMethod]
        public void ChangeUserRole_SetsMakeEmployerTrue_WhenUserIdIsTwo()
        {
            // Arrange
            string testEmail = "test@example.com";
            int testUserId = 2;
            bool makeEmployer = false;

            // Act
            _userService.ChangeUserRole(testEmail, testUserId, makeEmployer);

            // Assert
            _moqUserRepo.Verify(repo => repo.ChangeUserRole(testEmail, testUserId, true), Times.Once);
        }



    }
}
