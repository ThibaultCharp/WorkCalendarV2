using LogicLayer.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using LogicLayer.Entitys;
using LogicLayer.IRepos;
using LogicLayer.Services;
using TestUnit.FakeRepos;

namespace TestUnit.UnitTests
{
    [TestClass]
    public class UnitTestActivityFunctions
    {
        private FakeActivityRepo _fakeRepo;

        [TestInitialize]
        public void Setup()
        {
            _fakeRepo = new FakeActivityRepo();
        }

        [TestMethod]
        public void GetActivitiesByUserEmail_ReturnsCorrectActivities()
        {
            // Arrange
            var email = "john.doe@example.com";

            // Act
            var activities = _fakeRepo.GetActivitiesByUserEmail(email);

            // Assert
            Assert.AreEqual(1, activities.Count);
            Assert.AreEqual(email, activities.First().employee.user.email);
        }

        [TestMethod]
        public void GetActivitiesByEmployerEmail_ReturnsCorrectActivities()
        {
            // Arrange
            var email = "emp1@example.com";

            // Act
            var activities = _fakeRepo.GetActivitiesByEmployerEmail(email);

            // Assert
            Assert.AreEqual(1, activities.Count);
            Assert.AreEqual(email, activities.First().employee.employer.user.email);
        }

        [TestMethod]
        public void CreateActivity_AddsNewActivity()
        {
            // Arrange
            int positionId = 2;
            string begintime = "08:00";
            string endtime = "16:00";
            string date = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            int employeeId = 2;
            string employeeEmail = "employee2@example.com";

            // Act
            _fakeRepo.CreateActivity(positionId, begintime, endtime, date, employeeId);

            // Assert
            var createdActivity = _fakeRepo.GetActivitiesByUserEmail(employeeEmail).Last();
            Assert.AreEqual(positionId, createdActivity.position.id);
            Assert.AreEqual(TimeOnly.Parse(begintime), createdActivity.begintime);
            Assert.AreEqual(TimeOnly.Parse(endtime), createdActivity.endtime);
            Assert.AreEqual(DateTime.Parse(date), createdActivity.date);
            Assert.AreEqual(employeeId, createdActivity.employee.id);
        }


        [TestMethod]
        public void UpdateActivity_UpdatesExistingActivity()
        {
            // Arrange
            int activityId = 1;
            string newPosition = "Tester";
            string newBegintime = "10:00";
            string newEndtime = "18:00";
            string newDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string newEmployerName = "New Employer";
            string newEmployerEmail = "new.emp@example.com";

            // Act
            _fakeRepo.UpdateActivity(activityId, newPosition, newBegintime, newEndtime, newDate, newEmployerName, newEmployerEmail);

            // Assert
            var updatedActivity = _fakeRepo.GetActivitiesByUserEmail("john.doe@example.com").First();
            Assert.AreEqual(newPosition, updatedActivity.position.name);
            Assert.AreEqual(TimeOnly.Parse(newBegintime), updatedActivity.begintime);
            Assert.AreEqual(TimeOnly.Parse(newEndtime), updatedActivity.endtime);
            Assert.AreEqual(DateTime.Parse(newDate), updatedActivity.date);
            Assert.AreEqual(newEmployerName, updatedActivity.employee.employer.user.name);
            Assert.AreEqual(newEmployerEmail, updatedActivity.employee.employer.user.email);
        }
    }
}
