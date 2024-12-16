using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUnit.FakeRepos;
using LogicLayer.Entitys;

namespace TestUnit.UnitTests
{
    [TestClass]
    public class UnitTestsEmployeeFunction
    {
        private FakeEmployeeRepo _fakeRepo;

        [TestInitialize]
        public void Setup()
        {
            _fakeRepo = new FakeEmployeeRepo();
        }

        [TestMethod]
        public void GetAllemployeesPerEmployer_ReturnsCorrectEmployees()
        {
            // Arrange
            string employerEmail = "employer1@example.com";

            // Act
            var employees = _fakeRepo.GetAllemployeesPerEmployer(employerEmail);

            // Assert
            Assert.AreEqual(2, employees.Count);
            Assert.IsTrue(employees.All(e => e.employer.user.email == employerEmail));
        }

        [TestMethod]
        public void GetAllemployeesPerEmployer_ReturnsEmptyListForUnknownEmail()
        {
            // Arrange
            string employerEmail = "unknown@example.com";

            // Act
            var employees = _fakeRepo.GetAllemployeesPerEmployer(employerEmail);

            // Assert
            Assert.AreEqual(0, employees.Count);
        }

        [TestMethod]
        public void GetAllemployeesPerEmployer_ReturnsCorrectEmployeeDetails()
        {
            // Arrange
            string employerEmail = "employer1@example.com";

            // Act
            var employees = _fakeRepo.GetAllemployeesPerEmployer(employerEmail);
            var firstEmployee = employees.First();

            // Assert
            Assert.AreEqual(1, firstEmployee.id);
            Assert.AreEqual("John Doe", firstEmployee.user.name);
            Assert.AreEqual("john.doe@example.com", firstEmployee.user.email);
            Assert.AreEqual("Employer1", firstEmployee.employer.user.name);
        }
    }
}
