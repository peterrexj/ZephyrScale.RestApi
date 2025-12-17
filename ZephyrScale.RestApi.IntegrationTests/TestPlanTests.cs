using NUnit.Framework;
using Pj.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using ZephyrScale.RestApi.Dtos.Cloud;

namespace ZephyrScale.RestApi.IntegrationTests
{
    public class TestPlanTests : TestCaseBase
    {
        private List<string> _createdTestPlanKeys = new List<string>();

        [TearDown]
        public void TestTearDown()
        {
            // Clean up created test plans
            foreach (var testPlanKey in _createdTestPlanKeys)
            {
                Console.WriteLine($"WARNING: Test plan {testPlanKey} was created during integration tests. Please manually delete it from Zephyr Scale as DELETE operations are not supported by the API.");
            }
            _createdTestPlanKeys.Clear();
        }

        [Test]
        public void Should_Create_TestPlan()
        {
            // Arrange
            var request = CreateValidTestPlanRequest();

            // Act
            var createdTestPlan = _zephyrService.TestPlanCreate(request);

            // Assert
            Assert.IsNotNull(createdTestPlan);
            Assert.IsNotNull(createdTestPlan.Key);
            
            // Track for cleanup
            _createdTestPlanKeys.Add(createdTestPlan.Key);
            
            // Verification fetch to get complete object properties
            var verificationTestPlan = _zephyrService.TestPlanGet(createdTestPlan.Key);
            Assert.IsNotNull(verificationTestPlan);
            Assert.AreEqual(request.Name, verificationTestPlan.Name);
            Assert.IsNotNull(verificationTestPlan.Project, "Project should not be null");
            Assert.IsNotNull(verificationTestPlan.Project.Id, "Project ID should not be null");
        }

        [Test]
        public void Should_Get_TestPlan_By_Key()
        {
            // Arrange
            var request = CreateValidTestPlanRequest();
            var createdTestPlan = _zephyrService.TestPlanCreate(request);
            _createdTestPlanKeys.Add(createdTestPlan.Key);

            // Act
            var retrievedTestPlan = _zephyrService.TestPlanGet(createdTestPlan.Key);

            // Assert
            Assert.IsNotNull(retrievedTestPlan);
            Assert.AreEqual(createdTestPlan.Key, retrievedTestPlan.Key);
            Assert.AreEqual(request.Name, retrievedTestPlan.Name); // Compare with original request name
            Assert.IsNotNull(retrievedTestPlan.Project, "Project should not be null");
            Assert.IsNotNull(retrievedTestPlan.Project.Id, "Project ID should not be null");
        }

        // NOTE: DELETE operations are not supported by the Zephyr Scale Cloud API
        // Test plans created during integration tests must be manually deleted

        [Test]
        public void Should_Get_TestPlans_Full()
        {
            // Arrange
            var request = CreateValidTestPlanRequest();
            var createdTestPlan = _zephyrService.TestPlanCreate(request);
            _createdTestPlanKeys.Add(createdTestPlan.Key);

            // Act
            var testPlans = _zephyrService.TestPlansGetFull(_jiraProjectKey);

            // Assert
            Assert.IsNotNull(testPlans);
            Assert.IsTrue(testPlans.Count > 0);
            Assert.IsTrue(testPlans.Any(tp => tp.Key == createdTestPlan.Key));
        }

        [Test]
        public void Should_Search_TestPlans_With_Predicate()
        {
            // Arrange
            var uniqueName = $"SearchTest_{DateTimeEx.GetDateTimeReadable()}";
            var request = CreateValidTestPlanRequest(uniqueName);
            var createdTestPlan = _zephyrService.TestPlanCreate(request);
            _createdTestPlanKeys.Add(createdTestPlan.Key);

            // Act
            var searchResults = _zephyrService.TestPlansGetFull(
                _jiraProjectKey, 
                predicate: tp => tp.Name.Contains("SearchTest_"),
                breakSearchOnFirstConditionValid: false);

            // Assert
            Assert.IsNotNull(searchResults);
            Assert.IsTrue(searchResults.Count > 0);
            Assert.IsTrue(searchResults.Any(tp => tp.Key == createdTestPlan.Key));
        }

        [Test]
        public void Should_Get_TestPlan_Count()
        {
            // Arrange
            var request = CreateValidTestPlanRequest();
            var createdTestPlan = _zephyrService.TestPlanCreate(request);
            _createdTestPlanKeys.Add(createdTestPlan.Key);

            // Act
            var count = _zephyrService.TestPlanCountGet(_jiraProjectKey);

            // Assert
            Assert.IsTrue(count > 0);
        }

        private TestPlanCreateRequest CreateValidTestPlanRequest(string name = null)
        {
            // Get available statuses and priorities for the project
            var statuses = _zephyrService.StatusesGetFull(_jiraProjectKey);
            var priorities = _zephyrService.PrioritiesGetFull(_jiraProjectKey);

            return new TestPlanCreateRequest
            {
                ProjectKey = _jiraProjectKey,
                Name = name ?? $"Integration Test Plan {DateTimeEx.GetDateTimeReadable()}",
                Description = $"Integration test plan created at {DateTimeEx.GetDateTimeReadable()}",
                StatusId = statuses?.FirstOrDefault()?.Id,
                PriorityId = priorities?.FirstOrDefault()?.Id,
                PlannedStartDate = DateTime.Now.Date,
                PlannedEndDate = DateTime.Now.Date.AddDays(30),
                TestCycleKeys = new List<string>(), // Empty for now
                WebLinks = new List<WebLink>(),
                CustomFields = new Dictionary<string, object>()
            };
        }
    }
}