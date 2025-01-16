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


namespace TestUnit.UnitTests
{
    [TestClass]
    public class UnitTestsEmployeeFunction
    {
        private EmployeeService _employeeService;
        private Mock<IEmployeeRepo> _moqEmployeeRepo;


        [TestInitialize]
        public void Setup()
        {
            _moqEmployeeRepo = new Mock<IEmployeeRepo>();
            _employeeService = new EmployeeService(_moqEmployeeRepo.Object);
        }



        [TestMethod]
        public void GetAllEmployeesPerEmployer_ReturnsCorrectEmployees()
        {
            // Arrange
            string testEmployerEmail = "employer@example.com";

            var expectedEmployees = new List<Employee>
    {
        new Employee
        {
            id = 1,
            user = new User { id = 1, name = "Alice", email = "alice@example.com" },
            employer = new Employer { id = 1, user = new User { id = 101, email = testEmployerEmail } }
        },
        new Employee
        {
            id = 2,
            user = new User { id = 2, name = "Bob", email = "bob@example.com" },
            employer = new Employer { id = 1, user = new User { id = 101, email = testEmployerEmail } }
        }
    };

            _moqEmployeeRepo
                .Setup(repo => repo.GetAllemployeesPerEmployer(testEmployerEmail))
                .Returns(expectedEmployees);

            // Act
            var result = _employeeService.GetAllEmployeesPerEmployer(testEmployerEmail);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployees.Count, result.Count);
            Assert.AreEqual(expectedEmployees[0].user.name, result[0].user.name);
            Assert.AreEqual(expectedEmployees[1].user.email, result[1].user.email);

            _moqEmployeeRepo.Verify(repo => repo.GetAllemployeesPerEmployer(testEmployerEmail), Times.Once);
        }


        [TestMethod]
        public void GetAllEmployeesPerEmployer_ReturnsEmptyList_WhenNoEmployeesFound()
        {
            // Arrange
            string testEmployerEmail = "unknown@example.com";

            _moqEmployeeRepo
                .Setup(repo => repo.GetAllemployeesPerEmployer(testEmployerEmail))
                .Returns(new List<Employee>());

            // Act
            var result = _employeeService.GetAllEmployeesPerEmployer(testEmployerEmail);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);

            _moqEmployeeRepo.Verify(repo => repo.GetAllemployeesPerEmployer(testEmployerEmail), Times.Once);
        }
    }
}
