using System;
using System.Collections.Generic;
using System.Text;
using ZephyrScale.RestApi.Dtos.Server;
using Environment = ZephyrScale.RestApi.Dtos.Server.Environment;


namespace ZephyrScale.RestApi.Service.OnPrem
{
    public interface IZephyrScaleOnPremService
    {
        /// <summary>
        /// Retrieve the Environments matching the given projectKey. The project must exist
        /// The project must have Zephyr Scale enabled
        /// </summary>
        /// <param name="projectKey">The key of the Project</param>
        /// <returns></returns>
        IEnumerable<Environment> EnvironmentsGet(string projectKey);

        IEnumerable<Project> ProjectsInTestGet();
        IEnumerable<Dtos.Jira.Project> ProjectsInJiraGet();

        /// <summary>
        /// Creates a new folder for test cases, test plans or test runs.
        /// In order to create a new folder you must POST a json with 3 fields: projectKey, name and type.
        /// The field type can be filled with TEST_CASE, TEST_PLAN or TEST_RUN.
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="folderName"></param>
        /// <param name="jiraFolderType"></param>
        void FolderCreate(string projectKey, string folderName, FolderType jiraFolderType);

        /// <summary>
        /// Retrieve the Test Case matching the given key
        /// </summary>
        /// <param name="testcaseKey"></param>
        /// <returns></returns>
        TestCase TestCaseById(string testcaseKey);

        /// <summary>
        /// Retrieve the Test Case matching the project key and test case names
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testcaseNames"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        IList<TestCase> TestCasesByNames(string projectKey, string[] testcaseNames,
            Func<TestCase, bool> predicate = null, bool breakSearchOnFirstConditionValid = true);

        /// <summary>
        /// Retrieve the Test Case matching the project key and test case names in a given folder
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testCaseFolder"></param>
        /// <param name="testcaseNames"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        IList<TestCase> TestCasesByNamesInFolder(string projectKey, string testCaseFolder, string[] testcaseNames,
            Func<TestCase, bool> predicate = null, bool breakSearchOnFirstConditionValid = true);

        /// <summary>
        /// Retrieve the Test Case matching the project key and test case keys in a given folder
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testCaseFolder"></param>
        /// <param name="testcaseKeys"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        IList<TestCase> TestCaseByIdsInFolder(string projectKey, string testCaseFolder, string[] testcaseKeys,
            Func<TestCase, bool> predicate = null, bool breakSearchOnFirstConditionValid = true);

        /// <summary>
        /// Retrieve the Test Case matching the project key and test case keys
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testCaseKeys"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        IList<TestCase> TestCaseByIds(string projectKey, string[] testCaseKeys,
            Func<TestCase, bool> predicate = null, bool breakSearchOnFirstConditionValid = true);

        /// <summary>
        /// Retrieve the Test Case matching the project key and test case name
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testCaseName"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        IList<TestCase> TestCaseByTestCaseName(string projectKey, string testCaseName,
            Func<TestCase, bool> predicate = null,
            bool breakSearchOnFirstConditionValid = true);

        /// <summary>
        /// Retrieve the Top 'n' Test Case matching the project key in test case folder
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testCaseFolder"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IList<TestCase> TestCaseGetTopNInFolder(string projectKey, string testCaseFolder, int count);

        /// <summary>
        /// Retrieve the Top 'n' Test Case matching the project key
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IList<TestCase> TestCaseGetTopN(string projectKey, int count);

        /// <summary>
        /// Retrieve the Test Case matching the JQL
        /// </summary>
        /// <param name="jql"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        IList<TestCase> TestCaseByJql(string jql, Func<TestCase, bool> predicate = null, bool breakSearchOnFirstConditionValid = true);

        /// <summary>
        /// Creates a new Test Case.
        /// Whitespace is not allowed for labels, and it will be replaced by an underscore character.
        /// The field type of Test Script can have the values PLAIN_TEXT, STEP_BY_STEP or BDD.The field text describes the content of the plain text or BDD test script; otherwise, the steps can be described as objects using the field steps.
        /// Call To Tests can be added to the steps list by using the field testCaseKey with a Test Case key as value.
        /// The optional field folder, if defined, must contain an existent folder name.No folder will be created.
        /// The fields status and priority will be set to default values if not defined.
        /// The optional field parameters has two attributes: variables and entries.For attribute variables, two types are allowed: FREE_TEXT and DATA_SET.If using DATA_SET, an extra field should be informed, having the name of the dataset.If the dataset doesn’t exist, it will be automatically created.Attribute entries must only have values matching the informed variables.If a value of a dataset doesn’t exist, it will be automatically created for that dataset. Check the examples below for more details.
        /// </summary>
        /// <param name="jiraTestData"></param>
        /// <returns></returns>
        TestCase TestCaseCreate(TestCaseCreateRequest jiraTestData);

        /// <summary>
        /// Updates a Test Case.
        /// Whitespace is not allowed for labels, and it will be replaced by an underscore character.
        /// The field type of Test Script can have the values PLAIN_TEXT, STEP_BY_STEP or BDD.The field text describes the content of the plain text or BDD test script; otherwise, the steps can be described as objects using the field steps.
        /// The field folder, if defined, must contain an existent folder name. No folder will be created.
        /// Only fields present on the body will be updated.The field projectKey cannot be changed.
        /// Call To Tests can be added to the steps list by using the field testCaseKey with a Test Case key as value.
        /// The optional field parameters has two attributes: variables and entries.For attribute variables, two types are allowed: FREE_TEXT and DATA_SET.If using DATA_SET, an extra field should be informed, having the name of the dataset.If the dataset doesn’t exist, it will be automatically created.Attribute entries must only have values matching the informed variables.If a value of a dataset doesn’t exist, it will be automatically created for that dataset. Check the examples below for more details.
        /// For the field testScript, when it is a step-by-step script:
        /// If some step is missing in comparison to the target Test Case, it will be deleted.
        /// Steps not having id will be considered as a new step and will be created.
        /// Steps having id will be considered as existing steps and will be updated.
        /// </summary>
        /// <param name="testCaseKey"></param>
        /// <param name="testCase"></param>
        void TestCaseUpdate(string testCaseKey, TestCaseCreateRequest testCase);

        /// <summary>
        /// Get Test case meta data information
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        IEnumerable<TestCaseCustomDataMetadata> TestCaseGetCustomMetadata(int projectId);

        /// <summary>
        /// Retrieve the Test Run matching the given key
        /// </summary>
        /// <param name="testCycleKey"></param>
        /// <returns></returns>
        TestCycle TestCycleGetById(string testCycleKey);

        /// <summary>
        /// Retrieve the Test Run matching the project key and test cycle key
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testCycleKey"></param>
        /// <param name="includeItems"></param>
        /// <returns></returns>
        IList<TestCycle> TestCycleGetByKey(string projectKey, string testCycleKey, bool includeItems = false);

        /// <summary>
        /// Retrieve the Test Run matching the project key and test cycle key in a given folder
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="folderName"></param>
        /// <param name="testCycleKey"></param>
        /// <param name="includeItems"></param>
        /// <returns></returns>
        IList<TestCycle> TestCycleGetByKey(string projectKey, string folderName, string testCycleKey, bool includeItems = false);

        /// <summary>
        /// Retrieve the Test Run matching the project key and test cycle name
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testCycleName"></param>
        /// <param name="includeItems"></param>
        /// <returns></returns>
        IList<TestCycle> TestCycleGetByName(string projectKey, string testCycleName, bool includeItems = false);

        /// <summary>
        /// Retrieve the Test Run matching the project key and test cycle name in a given folder
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="folderName"></param>
        /// <param name="testCycleName"></param>
        /// <param name="includeItems"></param>
        /// <returns></returns>
        IList<TestCycle> TestCycleGetByName(string projectKey, string folderName, string testCycleName, bool includeItems = false);

        /// <summary>
        /// Retrieve the Test Run matching the list of projects within their folder
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="includeItems"></param>
        /// <param name="projectKeys"></param>
        /// <returns></returns>
        IList<TestCycle> TestCycleGetByProjectInFolder(string folderName, bool includeItems = false, params string[] projectKeys);

        /// <summary>
        /// Retrieve the Test Run matching the project key within a folder
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="folderName"></param>
        /// <param name="includeItems"></param>
        /// <returns></returns>
        IList<TestCycle> TestCycleGetByFolder(string projectKey, string folderName, bool includeItems = false);

        /// <summary>
        /// Retrieve the Test Run matching multiple project key 
        /// </summary>
        /// <param name="includeItems"></param>
        /// <param name="projectKeys"></param>
        /// <returns></returns>
        IList<TestCycle> TestCycleGetByProjectKeys(bool includeItems = false, params string[] projectKeys);

        /// <summary>
        /// Retrieve the Test Run by JQL command
        /// </summary>
        /// <param name="jql"></param>
        /// <param name="includeItems"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        IList<TestCycle> TestCycleGetByJql(string jql, bool includeItems = false, Func<TestCycle, bool> predicate = null, bool breakSearchOnFirstConditionValid = true);

        /// <summary>
        /// Creates a new Test Run.
        /// The fields plannedStartDate and plannedEndDate will be set to default values if not defined.
        /// The field status will be automatically inferred based on the status of Test Run Items (field items).
        /// The Test Run can be linked to a Test Plan, by setting a valid value on field testPlanKey. Also, it can be linked to an issue, by setting a valid value on field issueKey.
        /// All Test Result fields are allowed for Test Run Items (field items).
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        TestCycle TestCycleCreate(TestCycleCreateRequest request);

        /// <summary>
        /// Create Test Execution result within a Test Run and for a single test case
        /// </summary>
        /// <param name="testrunIdOrKey"></param>
        /// <param name="testcaseIdOrKey"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        TestExecutionResult TestExecutionResultCreateByTestCaseInTestCycle(string testrunIdOrKey, string testcaseIdOrKey, TestExecutionResultCreate request);

        /// <summary>
        /// Update an existing test execution result
        /// </summary>
        /// <param name="testrunId"></param>
        /// <param name="testcaseId"></param>
        /// <param name="request"></param>
        /// <param name="environment"></param>
        /// <param name="userKey"></param>
        /// <returns></returns>
        TestExecutionResult TestExecutionResultUpdateByTestCaseInTestCycle(string testrunId, string testcaseId, TestExecutionResultCreate request,
            string environment = null, string userKey = null);

        /// <summary>
        /// Create one or more (multiple) new Test Execution result under a test cycle
        /// </summary>
        /// <param name="testCycleKey"></param>
        /// <param name="request"></param>
        /// <param name="environment"></param>
        /// <param name="userKey"></param>
        /// <returns></returns>
        List<TestExecutionResult> TestExecutionResultCreateByTestCycle(string testCycleKey, List<TestExecutionResultCreate> request,
            string environment = null, string userKey = null);

        /// <summary>
        /// Retrieve test execution results within a test cycle
        /// </summary>
        /// <param name="testCycleKey"></param>
        /// <returns></returns>
        IList<TestExecutionResult> TestExecutionResultGetByTestCycle(string testCycleKey);

        string ZeypherUrl { get; set; }
        string ZephyrApiVersion { get; set; }
        string JiraApiVersion { get; set; }
        string FolderSeparator { get; set; }
        bool CanConnect { get; }
    }
}
