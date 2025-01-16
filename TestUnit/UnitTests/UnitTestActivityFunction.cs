using LogicLayer.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using LogicLayer.Entitys;
using LogicLayer.IRepos;
using LogicLayer.Services;
using Moq;

namespace TestUnit.UnitTests
{
    [TestClass]
    public class UnitTestActivityFunctions
    {
        private ActivityService _activityService;
        private Mock<IActivityRepo> _moqActivityRepo;

        [TestInitialize]
        public void Setup()
        {
            _moqActivityRepo = new Mock<IActivityRepo>();
            _activityService = new ActivityService(_moqActivityRepo.Object);
        }

        [TestMethod]
        public void GetActivitiesByUserEmail_ReturnsCorrectActivities()
        {
            // Arrange
            string testEmail = "testuser@example.com";
            string testRole = "User";

            var expectedActivities = new List<Activity>
    {
        new Activity
        {
            id = 1,
            begintime = new TimeOnly(9, 0),
            endtime = new TimeOnly(17, 0),
            date = new DateTime(2025, 1, 10),
            employee = new Employee { id = 1 },
            position = new Position { id = 1, name = "Developer" }
        },
        new Activity
        {
            id = 2,
            begintime = new TimeOnly(10, 0),
            endtime = new TimeOnly(18, 0),
            date = new DateTime(2025, 1, 11),
            employee = new Employee { id = 2 },
            position = new Position { id = 2, name = "Tester" }
        }
    };

            _moqActivityRepo
                .Setup(repo => repo.GetActivitiesByUserEmail(testEmail))
                .Returns(expectedActivities);

            // Act
            var result = _activityService.GetActivitiesByUserEmail(testEmail, testRole);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedActivities.Count, result.Count);
            Assert.AreEqual(expectedActivities[0].id, result[0].id);
            Assert.AreEqual(expectedActivities[1].position.name, result[1].position.name);

            _moqActivityRepo.Verify(repo => repo.GetActivitiesByUserEmail(testEmail), Times.Once);
        }



        [TestMethod]
        public void CreateActivity_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            int testPositionId = 1;
            string testBeginTime = "09:00";
            string testEndTime = "17:00";
            string testDate = "2025-01-10";
            int testEmployeeId = 101;

            // Act
            _activityService.CreateActivity(testPositionId, testBeginTime, testEndTime, testDate, testEmployeeId);

            // Assert
            _moqActivityRepo.Verify(
                repo => repo.CreateActivity(testPositionId, testBeginTime, testEndTime, testDate, testEmployeeId),
                Times.Once
            );
        }


        [TestMethod]
        public void UpdateActivity_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            int testActivityId = 1;
            int testPositionId = 2;
            string testBeginTime = "10:00";
            string testEndTime = "18:00";
            string testDate = "2025-01-11";

            // Act
            _activityService.UpdateActivity(testActivityId, testPositionId, testBeginTime, testEndTime, testDate);

            // Assert
            _moqActivityRepo.Verify(
                repo => repo.UpdateActivity(testActivityId, testPositionId, testBeginTime, testEndTime, testDate),
                Times.Once
            );
        }


        [TestMethod]
        public void CreateActivity_AllowsBoundaryTimeValues()
        {
            // Arrange
            int testPositionId = 1;
            string testBeginTime = "00:00"; // Midnight
            string testEndTime = "23:59";  // One minute before midnight
            string testDate = "2025-01-10";
            int testEmployeeId = 101;

            // Act
            _activityService.CreateActivity(testPositionId, testBeginTime, testEndTime, testDate, testEmployeeId);

            // Assert
            _moqActivityRepo.Verify(
                repo => repo.CreateActivity(testPositionId, testBeginTime, testEndTime, testDate, testEmployeeId),
                Times.Once
            );
        }


        [TestMethod]
        public void GetActivitiesByUserEmail_CallsCorrectRepositoryMethod_BasedOnRole()
        {
            // Arrange
            string testEmail = "employer@example.com";
            string employerRole = "Employer";

            var expectedActivities = new List<Activity>
    {
        new Activity
        {
            id = 1,
            date = new DateTime(2025, 1, 10),
            position = new Position { id = 1, name = "Manager" }
        }
    };

            _moqActivityRepo
                .Setup(repo => repo.GetActivitiesByEmployerEmail(testEmail))
                .Returns(expectedActivities);

            // Act
            var result = _activityService.GetActivitiesByUserEmail(testEmail, employerRole);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedActivities.Count, result.Count);
            Assert.AreEqual(expectedActivities[0].position.name, result[0].position.name);

            _moqActivityRepo.Verify(repo => repo.GetActivitiesByEmployerEmail(testEmail), Times.Once);
            _moqActivityRepo.Verify(repo => repo.GetActivitiesByUserEmail(It.IsAny<string>()), Times.Never);
        }




        [TestMethod]
        public void GetActivitiesByUserEmail_ReturnsEmptyList_WhenNoActivitiesFound()
        {
            // Arrange
            string testEmail = "nonexistentuser@example.com";
            string testRole = "User";

            _moqActivityRepo
                .Setup(repo => repo.GetActivitiesByUserEmail(testEmail))
                .Returns(new List<Activity>());

            // Act
            var result = _activityService.GetActivitiesByUserEmail(testEmail, testRole);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);

            _moqActivityRepo.Verify(repo => repo.GetActivitiesByUserEmail(testEmail), Times.Once);
        }

    }
}
