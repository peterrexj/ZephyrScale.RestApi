using System;
using System.Collections.Generic;
using ZephyrScale.RestApi.Dtos.Cloud;

namespace ZephyrScale.RestApi.Service.Cloud;

public interface IZephyrScaleCloudTestCaseService
{
    /// <summary>
        /// Creates a test case. Fields priorityName and statusName will be set to default values if not informed. Default values are usually “Normal” for priorityName and “Draft” for statusName. All required test case custom fields should be present in the request.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createTestCase
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        TestCase TestCaseCreate(TestCaseCreateRequest request);

        /// <summary>
        /// Updates an existing test case. If the project has test case custom fields, all custom fields should be present in the request. To leave any of them blank, please set them null if they are not required custom fields.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/updateTestCase
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        TestCase TestCaseUpdate(TestCase request);

        /// <summary>
        /// Returns a test case for the given key.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getTestCase
        /// </summary>
        /// <param name="testCaseKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        TestCase TestCaseGetById(string testCaseKey);

        /// <summary>
        /// Returns the list of test cases based on the search request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Pagination<TestCase> TestCasesGet(TestSearchRequest request);

        /// <summary>
        /// Retrieves all test cases. Query parameters can be used to filter the results.
        /// Use predicate to streamline your result, for examplev: (t) => t.Name.EqualsIgnoreCase("test") 
        /// Use [breakSearchOnFirstConditionValid] if you want break the search once the match is found. This can improve performance when searching for specific item
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/listTestCases
        /// </summary>
        /// <param name="projectKey">Jira project key filter</param>
        /// <param name="folderId"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        List<TestCase> TestCasesGetFull(string projectKey, long? folderId = null,
            Func<TestCase, bool> predicate = null, bool breakSearchOnFirstConditionValid = true);

        /// <summary>
        /// Returns links for a test case with specified key.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getTestCaseLinks
        /// </summary>
        /// <param name="testCaseKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Links TestCaseLinksGet(string testCaseKey);

        /// <summary>
        /// Creates a link between a test case and a Jira issue
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createTestCaseIssueLinks
        /// </summary>
        /// <param name="testCaseKey"></param>
        /// <param name="issueId"></param>
        /// <exception cref="Exception"></exception>
        void TestCaseLinkCreate(string testCaseKey, long issueId);

        /// <summary>
        /// Provides the custom fields configured on the test case
        /// </summary>
        /// <param name="projectKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        IEnumerable<string> TestCaseCustomFieldNames(string projectKey);

        /// <summary>
        /// Retrieves the custom field information on the first available test case.
        /// This can be used to create a new test case
        /// </summary>
        /// <param name="projectKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        dynamic TestCaseCustomFieldName(string projectKey);

        /// <summary>
        /// Returns the total number of test cases available in the project
        /// </summary>
        /// <param name="projectKey"></param>
        /// <returns></returns>
        long TestCaseCountGet(string projectKey);

        /// <summary>
        /// Returns the total number of execution within a test case
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testCase"></param>
        /// <returns></returns>
        long TestCaseExecutionCountGet(string projectKey, string testCase);

}