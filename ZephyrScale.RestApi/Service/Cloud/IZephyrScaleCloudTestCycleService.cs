using System;
using System.Collections.Generic;
using ZephyrScale.RestApi.Dtos.Cloud;

namespace ZephyrScale.RestApi.Service.Cloud;

public interface IZephyrScaleCloudTestCycleService
{
    /// <summary>
        /// Creates a Test Cycle. All required test cycle custom fields should be present in the request. Refer the link for more details
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createTestCycle
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        TestCycle TestCycleCreate(TestCycleCreateRequest request);

        /// <summary>
        /// Returns a test cycle for the given key
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getTestCycle
        /// </summary>
        /// <param name="testCycleIdOrKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        TestCycle TestCycleGet(string testCycleIdOrKey);

        /// <summary>
        /// Returns the list of test cycle based on the search request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Pagination<TestCycle> TestCyclesGet(TestSearchRequest request);

        /// <summary>
        /// Returns all test cycles. Query parameters can be used to filter by project and folder. You have option to match with predicate.
        /// Use predicate to streamline your result, for examplev: (t) => t.Name.EqualsIgnoreCase("test") 
        /// Use [breakSearchOnFirstConditionValid] if you want break the search once the match is found. This can improve performance when searching for specific item
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/listTestCycles
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="folderId"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        List<TestCycle> TestCyclesGetFull(string projectKey, long? folderId = null,
            Func<TestCycle, bool> predicate = null, bool breakSearchOnFirstConditionValid = true,
            long? jiraProjectVersionId = null);

        /// <summary>
        /// Updates an existing test cycle. If the project has test cycle custom fields, all custom fields should be present in the request. To leave any of them blank, please set them null if they are not required custom fields.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/updateTestCycle
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        TestCycle TestCycleUpdate(TestCycle request);

        /// <summary>
        /// Returns links for a test cycle with specified key.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getTestCycleLinks
        /// </summary>
        /// <param name="testCycleIdOrKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Links TestCycleLinksGet(string testCycleIdOrKey);

        /// <summary>
        /// Creates a link between a test cycle and a Jira issue
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createTestCycleIssueLink
        /// </summary>
        /// <param name="testCycleIdOrKey"></param>
        /// <param name="issueId"></param>
        /// <exception cref="Exception"></exception>
        void TestCycleLinkCreate(string testCycleIdOrKey, Int64 issueId);

        /// <summary>
        /// Returns the custom field names configured in the test cycle
        /// </summary>
        /// <param name="projectKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        IEnumerable<string> TestCycleCustomFieldNames(string projectKey);

        /// <summary>
        /// Returns the count of cycle available in the project
        /// </summary>
        /// <param name="projectKey">Jira project key</param>
        /// <returns></returns>
        long TestCycleCountGet(string projectKey);

        /// <summary>
        /// Returns the count of execution within a cycle
        /// </summary>
        /// <param name="projectKey">Jira project key</param>
        /// <param name="testCycle">Cycle key</param>
        /// <returns></returns>
        long TestCycleExecutionCountGet(string projectKey, string testCycle);

}