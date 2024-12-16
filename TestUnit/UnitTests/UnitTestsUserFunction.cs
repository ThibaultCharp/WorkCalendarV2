using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUnit.FakeRepos;
using LogicLayer.Entitys;
using LogicLayer.Entities;

namespace TestUnit.UnitTests
{
    [TestClass]
    public class UnitTestsUserFunction
    {
        private FakeUserRepo _fakeRepo;

        [TestInitialize]
        public void Setup()
        {
            _fakeRepo = new FakeUserRepo();
        }

        [TestMethod]
        public void LinkUser_AssignsEmployeeToEmployer()
        {
            // Arrange
            string employerEmail = "charlie@example.com";
            string employeeEmail = "bob@example.com";

            // Act
            _fakeRepo.LinkUser(employerEmail, employeeEmail);

            // Assert
            var updatedEmployee = _fakeRepo.GetAllUsersWithCorrespondingRoles("bob@example.com")[0];
            Assert.AreEqual("Employee under Employer", updatedEmployee.role.name);
        }

        [TestMethod]
        public void ChangeUserRole_UpdatesUserRoleToEmployer()
        {
            // Arrange
            string email = "bob@example.com";
            int newRoleId = 3;
            bool makeEmployer = true;

            // Act
            _fakeRepo.ChangeUserRole(email, newRoleId, makeEmployer);

            // Assert
            var updatedUser = _fakeRepo.GetAllUsersWithCorrespondingRoles(email)[0];
            Assert.AreEqual("Employer", updatedUser.role.name);
        }

        [TestMethod]
        public void UserExists_ReturnsTrueForExistingUser()
        {
            // Arrange
            int userId = 1;

            // Act
            bool exists = _fakeRepo.UserExists(userId);

            // Assert
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void UserExists_ReturnsFalseForNonExistentUser()
        {
            // Arrange
            int userId = 999;

            // Act
            bool exists = _fakeRepo.UserExists(userId);

            // Assert
            Assert.IsFalse(exists);
        }

        [TestMethod]
        public void CreateUser_AddsNewUserToRepo()
        {
            // Arrange
            var newUser = new User
            {
                id = 4,
                name = "David",
                email = "david@example.com",
                role = new Role { id = 2, name = "Employee" }
            };

            // Act
            _fakeRepo.CreateUser(newUser);

            // Assert
            var createdUser = _fakeRepo.GetAllUsersWithCorrespondingRoles("david@example.com")[0];
            Assert.AreEqual("David", createdUser.name);
        }

        [TestMethod]
        public void GetAllEmployeesWithoutEmployer_ReturnsCorrectUsers()
        {
            // Arrange
            string input = "bob";

            // Act
            var employees = _fakeRepo.GetAllEmployeesWithoutEmployer(input);

            // Assert
            Assert.AreEqual(1, employees.Count);
            Assert.AreEqual("Bob", employees[0].name);
        }

        [TestMethod]
        public void GetAllUsersWithCorrespondingRoles_FiltersCorrectly()
        {
            // Arrange
            string input = "Admin";

            // Act
            var users = _fakeRepo.GetAllUsersWithCorrespondingRoles(input);

            // Assert
            Assert.AreEqual(1, users.Count);
            Assert.AreEqual("Alice", users[0].name);
        }
    }
}
