using System;
using System.Collections.Generic;
using ZephyrScale.RestApi.Dtos.Cloud;
using Environment = ZephyrScale.RestApi.Dtos.Cloud.Environment;

namespace ZephyrScale.RestApi.Service.Cloud
{
    public interface IZephyrScaleCloudService
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
        /// Returns a folder for the given ID
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getFolder
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Folder FolderGetById(string folderId);

        /// <summary>
        /// Creates a folder
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createFolder
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Folder FolderCreate(FolderCreateRequest request);

        /// <summary>
        /// Creates the full folder structure
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createFolder
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Folder FolderCreateRecursive(FolderCreateRequest request);

        /// <summary>
        /// Returns the list of the folders based on the search request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Pagination<Folder> FoldersGet(FolderSearchRequest request);

        /// <summary>
        /// Returns all folder.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/listFolders
        /// Use predicate to streamline your result, for examplev: (t) => t.Name.EqualsIgnoreCase("test") 
        /// Use [breakSearchOnFirstConditionValid] if you want break the search once the match is found. This can improve performance when searching for specific item
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="folderType"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        List<Folder> FoldersGetFull(string projectKey, FolderType folderType, Func<Folder, bool> predicate = null, bool breakSearchOnFirstConditionValid = true);

        /// <summary>
        /// Returns all folders and build the full path
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/listFolders
        /// </summary>
        /// <param name="folders"></param>
        /// <returns></returns>
        List<Folder> FolderWithFullPath(List<Folder> folders);

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
        /// Returns a status for the given ID.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getStatus
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Status StatusGet(string statusId);

        /// <summary>
        /// Returns all statuses.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/listStatuses
        /// Use predicate to streamline your result, for examplev: (t) => t.Name.EqualsIgnoreCase("test") 
        /// Use [breakSearchOnFirstConditionValid] if you want break the search once the match is found. This can improve performance when searching for specific item
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="statusType"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        List<Status> StatusesGetFull(string projectKey, string statusType = null, Func<Status, bool> predicate = null, bool breakSearchOnFirstConditionValid = true);

        /// <summary>
        /// Returns the list of statuses based on the search request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Pagination<Status> StatusesGet(StatusSearchRequest request);

        /// <summary>
        /// Returns the list of environment based on the search request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Pagination<Environment> EnvironmentsGet(SearchRequestBase request);

        /// <summary>
        /// Returns all environments.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/listEnvironments
        /// Use predicate to streamline your result, for examplev: (t) => t.Name.EqualsIgnoreCase("test") 
        /// Use [breakSearchOnFirstConditionValid] if you want break the search once the match is found. This can improve performance when searching for specific item
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        List<Environment> EnvironmentsGetFull(string projectKey,
            Func<Environment, bool> predicate = null,
            bool breakSearchOnFirstConditionValid = true);

        /// <summary>
        /// Returns an environment for the given ID.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getEnvironment
        /// </summary>
        /// <param name="environmentId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Environment EnvironmentGet(string environmentId);

        /// <summary>
        /// Returns a project for the given ID or key.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getProject
        /// </summary>
        /// <param name="projectIdOrKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Project ProjectGet(string projectIdOrKey);

        /// <summary>
        /// Returns the project based on the search query
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Pagination<Project> ProjectsGet(SearchRequestBase request = null);

        /// <summary>
        /// Returns all projects.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/listProjects
        /// Use predicate to streamline your result, for examplev: (t) => t.Name.EqualsIgnoreCase("test") 
        /// Use [breakSearchOnFirstConditionValid] if you want break the search once the match is found. This can improve performance when searching for specific item
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        List<Project> ProjectsGetFull(Func<Project, bool> predicate = null,
            bool breakSearchOnFirstConditionValid = true);

        void LinkDelete(string linkId);
        string ZeypherUrl { get; set; }
        string ZephyrApiVersion { get; set; }
        string JiraApiVersion { get; set; }
        string FolderSeparator { get; set; }
        bool CanConnect { get; }
    }
}