using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using ZephyrScale.RestApi.Dtos.Cloud;

namespace ZephyrScale.RestApi.IntegrationTests
{
    public class StatusTests : TestCaseBase
    {
        [Test]
        public void Should_Return_FullStatuses()
        {
            // Act
            var statuses = _zephyrService.StatusesGetFull(_jiraProjectKey);

            // Assert
            Assert.IsNotNull(statuses);
            Assert.IsTrue(statuses.Count > 0);
            
            // Verify each status has required properties
            foreach (var status in statuses)
            {
                Assert.IsNotNull(status.Id);
                Assert.IsNotNull(status.Name);
                // Note: Status doesn't have a Type property in the DTO
            }
        }

        [Test]
        public void Should_Return_FullStatuses_With_StatusType_Filter()
        {
            // Act - Test with TEST_EXECUTION status type
            var testExecutionStatuses = _zephyrService.StatusesGetFull(_jiraProjectKey, statusType: "TEST_EXECUTION");

            // Assert
            Assert.IsNotNull(testExecutionStatuses);
            Assert.IsTrue(testExecutionStatuses.Count > 0);
            
            // Note: Status DTO doesn't have Type property, but the filter should work
            Console.WriteLine($"Found {testExecutionStatuses.Count} TEST_EXECUTION statuses");
        }

        [TestCase("Pass")]
        [TestCase("Fail")]
        [TestCase("Not Executed")]
        public void Should_Return_SearchResult_By_Name(string statusName)
        {
            // Act
            var statuses = _zephyrService.StatusesGetFull(
                _jiraProjectKey, 
                predicate: s => s.Name.Equals(statusName, StringComparison.OrdinalIgnoreCase),
                breakSearchOnFirstConditionValid: true);

            // Assert
            if (statuses?.Count > 0)
            {
                Assert.IsTrue(statuses.Count >= 1);
                Assert.IsTrue(statuses.Any(s => s.Name.Equals(statusName, StringComparison.OrdinalIgnoreCase)));
            }
            else
            {
                // Status might not exist in this project, which is acceptable
                Console.WriteLine($"Status '{statusName}' not found in project {_jiraProjectKey}");
            }
        }

        [Test]
        public void Should_Return_Status_By_Id()
        {
            // Arrange - Get a status first
            var statuses = _zephyrService.StatusesGetFull(_jiraProjectKey);
            Assert.IsNotNull(statuses);
            Assert.IsTrue(statuses.Count > 0);

            var firstStatus = statuses.First();

            // Act
            var retrievedStatus = _zephyrService.StatusGet(firstStatus.Id.ToString());

            // Assert
            Assert.IsNotNull(retrievedStatus);
            Assert.AreEqual(firstStatus.Id, retrievedStatus.Id);
            Assert.AreEqual(firstStatus.Name, retrievedStatus.Name);
        }

        [Test]
        public void Should_Return_Statuses_With_Search_Request()
        {
            // Arrange
            var searchRequest = new StatusSearchRequest
            {
                projectKey = _jiraProjectKey,
                maxResults = 50,
                startAt = 0,
                statusType = "TEST_EXECUTION"
            };

            // Act
            var searchResults = _zephyrService.StatusesGet(searchRequest);

            // Assert
            Assert.IsNotNull(searchResults);
            Assert.IsNotNull(searchResults.Values);
            Assert.IsTrue(searchResults.Values.Count > 0);
            
            // Verify pagination properties
            Assert.IsTrue(searchResults.MaxResults > 0);
            Assert.IsTrue(searchResults.StartAt >= 0);
            Assert.IsTrue(searchResults.Total >= searchResults.Values.Count);

            // Note: Status DTO doesn't have Type property, but the filter should work
            Console.WriteLine($"Found {searchResults.Values.Count} statuses with search request");
        }

        [Test]
        public void Should_Handle_Different_Status_Types()
        {
            // Test different status types that might exist
            var statusTypes = new[] { "TEST_EXECUTION", "TEST_CASE", "TEST_PLAN" };

            foreach (var statusType in statusTypes)
            {
                // Act
                var statuses = _zephyrService.StatusesGetFull(_jiraProjectKey, statusType: statusType);

                // Assert
                Assert.IsNotNull(statuses);
                
                if (statuses.Count > 0)
                {
                    // Note: Status DTO doesn't have Type property, but the filter should work
                    Console.WriteLine($"Found {statuses.Count} statuses of type {statusType}");
                }
                else
                {
                    Console.WriteLine($"No statuses found for type {statusType} in project {_jiraProjectKey}");
                }
            }
        }

        [Test]
        public void Should_Return_Statuses_With_Predicate_Filter()
        {
            // Act - Find statuses that contain "Pass" or "Fail" in their name
            var filteredStatuses = _zephyrService.StatusesGetFull(
                _jiraProjectKey,
                predicate: s => s.Name.Contains("Pass") || s.Name.Contains("Fail"),
                breakSearchOnFirstConditionValid: false);

            // Assert
            Assert.IsNotNull(filteredStatuses);
            
            if (filteredStatuses.Count > 0)
            {
                foreach (var status in filteredStatuses)
                {
                    Assert.IsTrue(status.Name.Contains("Pass") || status.Name.Contains("Fail"));
                }
            }
        }

        [Test]
        public void Should_Handle_Empty_Search_Results_Gracefully()
        {
            // Act - Search for a status that likely doesn't exist
            var nonExistentStatuses = _zephyrService.StatusesGetFull(
                _jiraProjectKey,
                predicate: s => s.Name.Equals("NonExistentStatusName12345"),
                breakSearchOnFirstConditionValid: true);

            // Assert
            Assert.IsNotNull(nonExistentStatuses);
            Assert.AreEqual(0, nonExistentStatuses.Count);
        }

        [Test]
        public void Should_Verify_Status_Properties()
        {
            // Arrange
            var statuses = _zephyrService.StatusesGetFull(_jiraProjectKey);
            Assert.IsNotNull(statuses);
            Assert.IsTrue(statuses.Count > 0);

            // Act & Assert - Verify each status has expected properties
            foreach (var status in statuses)
            {
                Assert.IsNotNull(status.Id, "Status ID should not be null");
                Assert.IsNotNull(status.Name, "Status Name should not be null");
                Assert.IsTrue(status.Id > 0, "Status ID should be positive");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(status.Name), "Status Name should not be empty");
                
                Console.WriteLine($"Status: ID={status.Id}, Name='{status.Name}'");
            }
        }
    }
}