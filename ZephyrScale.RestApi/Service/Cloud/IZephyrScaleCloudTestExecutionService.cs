using System;
using System.Collections.Generic;
using ZephyrScale.RestApi.Dtos.Cloud;

namespace ZephyrScale.RestApi.Service.Cloud;

public interface IZephyrScaleCloudTestExecutionService
{
    /// <summary>
        /// Creates a test execution. All required test execution custom fields should be present in the request
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createTestExecution
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        void TestExecutionCreate(TestExecutionCreateRequest request);

        /// <summary>
        /// Returns a test execution for the given ID
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getTestExecution
        /// </summary>
        /// <param name="testExecutionIdOrKey"></param>
        /// <param name="includeStepLinks"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        TestExecution TestExecutionGet(string testExecutionIdOrKey, bool includeStepLinks = false);

        /// <summary>
        /// Updates an existing test execution. If the project has test execution custom fields, all custom fields should be present in the request. To leave any of them blank, please set them null if they are not required custom fields.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/updateTestExecution
        /// </summary>
        /// <param name="request">The test execution object with updated values</param>
        /// <returns>The updated test execution</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        TestExecution TestExecutionUpdate(TestExecution request);

        /// <summary>
        /// Returns the list of test execution based on the search request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Pagination<TestExecution> TestExecutionsGet(TestExecutionSearchRequest request);

        /// <summary>
        /// Returns all test executions. Query parameters can be used to filter by project and folder
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/listTestExecutions
        /// Use predicate to streamline your result, for examplev: (t) => t.Name.EqualsIgnoreCase("test") 
        /// Use [breakSearchOnFirstConditionValid] if you want break the search once the match is found. This can improve performance when searching for specific item
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testCase"></param>
        /// <param name="testCycle"></param>
        /// <param name="actualEndDateAfter"></param>
        /// <param name="actualEndDateBefore"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        List<TestExecution> TestExecutionsGetFull(
            string projectKey,
            string testCase = null,
            string testCycle = null,
            DateTime? actualEndDateAfter = null,
            DateTime? actualEndDateBefore = null,
            Func<TestExecution, bool> predicate = null, 
            bool breakSearchOnFirstConditionValid = true,
            long? jiraProjectVersionId = null,
            bool? onlyLastExecutions = null);

        /// <summary>
        /// Get the total count of execution within a project
        /// </summary>
        /// <param name="projectKey">Jira project key</param>
        /// <returns></returns>
        long TestExecutionCountGet(string projectKey);

}