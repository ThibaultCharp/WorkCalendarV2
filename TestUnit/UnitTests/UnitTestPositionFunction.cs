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
    public class UnitTestPositionFunction
    {
        private PositionService _positionService;
        private Mock<IPositionRepo> _moqPositionRepo;


        [TestInitialize]
        public void Setup()
        {
            _moqPositionRepo = new Mock<IPositionRepo>();
            _positionService = new PositionService(_moqPositionRepo.Object);
        }

        [TestMethod]
        public void GetAllPositions_ReturnsAllPositions()
        {
            // Arrange
            var expectedPositions = new List<Position>
            {
                new Position { id = 1, name = "Manager" },
                new Position { id = 2, name = "Developer" }
            };

            _moqPositionRepo.Setup(repo => repo.GetAllPositions()).Returns(expectedPositions);

            // Act
            var result = _positionService.GetAllPositions();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPositions.Count, result.Count);
            CollectionAssert.AreEqual(expectedPositions, result);
        }

        [TestMethod]
        public void GetAllRoles_ReturnsAllRoles()
        {
            // Arrange
            var expectedRoles = new List<Role>
            {
                new Role { id = 1, name = "Admin" },
                new Role { id = 2, name = "Employee" }
            };

            _moqPositionRepo.Setup(repo => repo.GetAllRoles()).Returns(expectedRoles);

            // Act
            var result = _positionService.GetAllRoles();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedRoles.Count, result.Count);
            CollectionAssert.AreEqual(expectedRoles, result);
        }
    }
}
