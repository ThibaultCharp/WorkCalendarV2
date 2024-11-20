using LogicLayer.Entitys;
using LogicLayer.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using LogicLayer.Classes;
namespace LogicLayer.UnitTests
{
    [TestClass]
    public class UnitTestActivityFunctions
    {
        [TestMethod]
        public void TestGetActivitiesByUserId_ReturnCorrectActivities()
        {

            // Arrange
            ActivityService activityService = new ActivityService(new FakeActivityRepo());
            string userId = "user1";

            // Act
            var activities = activityService.GetActivitiesByUserId(userId);

            // Assert
            Assert.AreEqual(1, activities.Count);
            Assert.AreEqual("John Doe", activities.First().employee.user.name);
        }

        [TestMethod]
        public void TestCreateActivity_AddNewActivity()
        {
            // Arrange
            ActivityService activityService = new ActivityService(new FakeActivityRepo());

            int positionId = 2;
            string begintime = "10:00";
            string endtime = "18:00";
            string date = DateTime.Now.Date.ToString("yyyy-MM-dd");
            int employeeId = 1;

            // Act
            activityService.CreateActivity(positionId, begintime, endtime, date, employeeId);
            var activities = activityService.GetActivitiesByUserId("user1");

            // Assert
            Assert.AreEqual(2, activities.Count);
        }

        [TestMethod]
        public void TestUpdateActivity_ModifyExistingActivity()
        {
            // Arrange
            ActivityService activityService = new ActivityService(new FakeActivityRepo());

            int activityId = 1;
            string newPosition = "Senior Developer";
            string newBeginTime = "08:00";
            string newEndTime = "16:00";
            string newDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string newEmployerName = "Updated Employer";
            string newEmployerEmail = "updated@example.com";

            // Act
            activityService.UpdateActivity(activityId, newPosition, newBeginTime, newEndTime, newDate, newEmployerName, newEmployerEmail);

            // Assert
            var updatedActivity = activityService.GetActivitiesByUserId("user1").First();
            Assert.AreEqual("Senior Developer", updatedActivity.position.name);
            Assert.AreEqual(TimeOnly.Parse(newBeginTime), updatedActivity.begintime);
            Assert.AreEqual("updated@example.com", updatedActivity.employee.employer.user.email);
        }
    }
}
