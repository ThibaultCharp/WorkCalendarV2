using LogicLayer.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using LogicLayer.Classes;
using LogicLayer.IRepos;
using LogicLayer.Services;
using LogicLayer.Tests;
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
            int userId = 1;

            // Act
            var activities = activityService.GetActivitiesByUserId(userId);

            // Assert
            Assert.AreEqual(1, activities.Count);
            Assert.AreEqual("John Doe", activities.First().employee.user.name);
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
            var updatedActivity = activityService.GetActivitiesByUserId(1).First();
            Assert.AreEqual("Senior Developer", updatedActivity.position.name);
            Assert.AreEqual(TimeOnly.Parse(newBeginTime), updatedActivity.begintime);
            Assert.AreEqual("updated@example.com", updatedActivity.employee.employer.user.email);
        }
    }
}
