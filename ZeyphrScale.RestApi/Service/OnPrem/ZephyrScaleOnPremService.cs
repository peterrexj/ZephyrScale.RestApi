using Newtonsoft.Json;
using Pj.Library;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TestAny.Essentials.Core.Dtos.Api;
using ZeyphrScale.RestApi.Dtos.Server;
using Environment = ZeyphrScale.RestApi.Dtos.Server.Environment;

namespace ZeyphrScale.RestApi.Service.OnPrem
{
    public class ZephyrScaleOnPremService : ZephyrScaleOnPremServiceBase, IZephyrScaleOnPremService
    {
        /// <summary>
        /// Initialize the Zeyphr service with required parameters
        /// </summary>
        /// <param name="appUrl">Zeyphr endpoint</param>
        /// <param name="serviceUsername">Username for connecting to Zeyphr</param>
        /// <param name="servicePassword">Password for connecting to Zeyphr</param>
        /// <param name="zeyphrApiVersion">Zeyphr Scale Api version (default value 1.0)</param>
        /// <param name="jiraApiVersion">Jira Api version where the Zeyphr is hosted (default value: 2)</param>
        /// <param name="folderSeparator">Folder separator string (default value: '/')</param>
        /// <param name="logPrefix">Prefix text that will be added to all the logs generated from this service (default value: 'Zeyphr: ')</param>
        /// <param name="pageSizeSearchResult">Page size for search request (default value: '50')</param>
        /// <param name="requestRetryTimes">Number of time to retry when there is a network failure (default value: '1'). You can increase the number of times to retry based on your infrastructure if there are chance for a request to fail randomly</param>
        /// <param name="timeToSleepBetweenRetryInMilliseconds">Time to sleep in milliseconds between each time a call is retry (default value: '1000'). Applied only when requestRetryTimes is more than 1</param>
        /// <param name="assertResponseStatusOk">True/False whether the response code status from the server needs to be asserted for OK (default value 'true')</param>
        /// <param name="listOfResponseCodeOnFailureToRetry">Any of these status code matched from response will then use for retry the request. For example Proxy Authentication randomly failing can be then used to retry (default value 'null' which means it is not checking any response code for fail retry)</param>
        /// <param name="requestTimeoutInSeconds">Control the total time to wait for any request made to the Zeyphr Scale server. Default time is set to 300 seconds and it can be increased if the data on the server is too many and requires more time to process to respond</param>
        public ZephyrScaleOnPremService(string appUrl,
            string serviceUsername,
            string servicePassword,
            string zeyphrApiVersion = "1.0",
            string jiraApiVersion = "2",
            string folderSeparator = "/",
            string logPrefix = "Zeyphr: ",
            int pageSizeSearchResult = 50,
            int requestRetryTimes = 1,
            int timeToSleepBetweenRetryInMilliseconds = 1000,
            bool assertResponseStatusOk = true,
            HttpStatusCode[] listOfResponseCodeOnFailureToRetry = null,
            int requestTimeoutInSeconds = 300)
                : base(appUrl, serviceUsername, servicePassword, zeyphrApiVersion, jiraApiVersion, 
                    folderSeparator, logPrefix, pageSizeSearchResult,
                    requestRetryTimes, timeToSleepBetweenRetryInMilliseconds, 
                    assertResponseStatusOk, listOfResponseCodeOnFailureToRetry,
                    requestTimeoutInSeconds)
        { }

        /// <summary>
        /// Retrieve the Environments matching the given projectKey. The project must exist
        /// The project must have Zephyr Scale enabled
        /// </summary>
        /// <param name="projectKey">The key of the Project</param>
        /// <returns></returns>
        public IEnumerable<Environment> EnvironmentsGet(string projectKey)
        {
            Log($"Trying to get jira environments for project [{projectKey}]");

            var response = OpenRequest($"/rest/atm/{ZeyphrApiVersion}/environments?projectKey={projectKey}")
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry);

            response.AssertResponseStatusForSuccess();

            return ToType<IEnumerable<Environment>>(response.ResponseBody.ContentString);
        }
        
        public IEnumerable<Project> ProjectsInTestGet()
        {
            Log($"Request to get the list of projects available through {_appName}");

            var response = OpenRequest($"/rest/tests/{ZeyphrApiVersion}/project")
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry);

            response.AssertResponseStatusForSuccess();

            return ToType<IEnumerable<Project>>(response.ResponseBody.ContentJson);
        }
        public IEnumerable<Dtos.Jira.Project> ProjectsInJiraGet()
        {
            Log($"Request to get the list of project available in Jira");

            var response = OpenRequest($"/rest/api/{JiraApiVersion}/project")
                 .GetWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry);

            response.AssertResponseStatusForSuccess();

            return SerializationHelper.ToType<IEnumerable<Dtos.Jira.Project>>(response.ResponseBody.ContentJson);
        }

        /// <summary>
        /// Creates a new folder for test cases, test plans or test runs.
        /// In order to create a new folder you must POST a json with 3 fields: projectKey, name and type.
        /// The field type can be filled with TEST_CASE, TEST_PLAN or TEST_RUN.
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="folderName"></param>
        /// <param name="jiraFolderType"></param>
        public void FolderCreate(string projectKey, string folderName, FolderType jiraFolderType)
        {
            Log($"Request to create folder [{folderName}] of type [{jiraFolderType}] under project [{projectKey}]");

            var testrunfolder = new
            {
                projectKey,
                name = folderName,
                type = jiraFolderType.ToString()
            };

            var response = OpenRequest($"/rest/atm/{ZeyphrApiVersion}/folder")
              .SetJsonBody(testrunfolder)
              .PostWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry);
        }

        #region Test Case
        /// <summary>
        /// Retrieve the Test Case matching the given key
        /// </summary>
        /// <param name="testcaseKey"></param>
        /// <returns></returns>
        public TestCase TestCaseById(string testcaseKey)
        {
            Log($"Request to get test case by id [{testcaseKey}]");

            var response = OpenRequest($"/rest/atm/{ZeyphrApiVersion}/testcase/{testcaseKey}")
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry);

            response.AssertResponseStatusForSuccess();
            return ToType<TestCase>(response.ResponseBody.ContentJson);
        }

        /// <summary>
        /// Retrieve the Test Case matching the project key and test case names
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testcaseNames"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        public IList<TestCase> TestCasesByNames(string projectKey, string[] testcaseNames,
            Func<TestCase, bool> predicate = null, bool breakSearchOnFirstConditionValid = true)
        {
            return TestCaseByJql($"projectKey = \"{projectKey}\" AND name IN ({string.Join(",", testcaseNames.Select(s => $"\"{s}\""))})",
               predicate, breakSearchOnFirstConditionValid);
        }

        /// <summary>
        /// Retrieve the Test Case matching the project key and test case names in a given folder
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testCaseFolder"></param>
        /// <param name="testcaseNames"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        public IList<TestCase> TestCasesByNamesInFolder(string projectKey, string testCaseFolder, string[] testcaseNames,
            Func<TestCase, bool> predicate = null, bool breakSearchOnFirstConditionValid = true) 
            => TestCaseByJql($"projectKey = \"{projectKey}\" AND folder = \"{testCaseFolder}\" AND name IN ({string.Join(",", testcaseNames.Select(s => $"\"{s}\""))})",
                predicate, breakSearchOnFirstConditionValid);

        /// <summary>
        /// Retrieve the Test Case matching the project key and test case keys in a given folder
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testCaseFolder"></param>
        /// <param name="testcaseKeys"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        public IList<TestCase> TestCaseByIdsInFolder(string projectKey, string testCaseFolder, string[] testcaseKeys,
            Func<TestCase, bool> predicate = null, bool breakSearchOnFirstConditionValid = true) 
            => TestCaseByJql($"projectKey = \"{projectKey}\" AND folder = \"{testCaseFolder}\" AND key IN ({string.Join(",", testcaseKeys.Select(s => $"\"{s}\""))})",
                predicate, breakSearchOnFirstConditionValid);

        /// <summary>
        /// Retrieve the Test Case matching the project key and test case keys
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testCaseKeys"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        public IList<TestCase> TestCaseByIds(string projectKey, string[] testCaseKeys,
            Func<TestCase, bool> predicate = null, bool breakSearchOnFirstConditionValid = true) 
            => TestCaseByJql($"projectKey = \"{projectKey}\" AND key IN ({string.Join(",", testCaseKeys.Select(s => $"\"{s}\""))})",
                predicate, breakSearchOnFirstConditionValid);

        /// <summary>
        /// Retrieve the Test Case matching the project key and test case name
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testCaseName"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        public IList<TestCase> TestCaseByTestCaseName(string projectKey, string testCaseName,
            Func<TestCase, bool> predicate = null,
            bool breakSearchOnFirstConditionValid = true) 
            => TestCaseByJql($"projectKey = \"{projectKey}\" AND name = \"{testCaseName}\"", predicate, breakSearchOnFirstConditionValid);

        /// <summary>
        /// Retrieve the Top 'n' Test Case matching the project key in test case folder
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testCaseFolder"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IList<TestCase> TestCaseGetTopNInFolder(string projectKey, string testCaseFolder, int count) 
            => TestCaseGet(new ParameterCollection
                {
                    { "maxResults", $"{count}" },
                    { "query", $"projectKey = \"{projectKey}\" AND folder = \"{testCaseFolder}\"" }
                }.GetPropertyValuesV2());

        /// <summary>
        /// Retrieve the Top 'n' Test Case matching the project key
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IList<TestCase> TestCaseGetTopN(string projectKey, int count) 
            => TestCaseGet(new ParameterCollection
                {
                    { "maxResults", $"{count}" },
                    { "query", $"projectKey = \"{projectKey}\"" }
                }.GetPropertyValuesV2());

        /// <summary>
        /// Retrieve the Test Case matching the JQL
        /// </summary>
        /// <param name="jql"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        public IList<TestCase> TestCaseByJql(string jql, Func<TestCase, bool> predicate = null, bool breakSearchOnFirstConditionValid = true)
        {
            Log($"Request to get the test cases by Jql [{jql}]");

            return SearchFull<TestCase>(new Dictionary<string, string> { { "query", jql } },
                TestCaseGet, predicate, breakSearchOnFirstConditionValid);
        }

        /// <summary>
        /// Internal Test case search
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected IList<TestCase> TestCaseGet(IDictionary<string, string> request)
        {
            Log($"Request to get test case using [{string.Join(",", request?.Where(s => s.Value.HasValue()).Select(s => $"[{s.Key}, {s.Value}]"))}]");

            if (request?.ContainsKey("fields") == false)
            {
                request.Add("fields", "key,name,folder,status,priority,component,owner,estimatedTime,labels,customFields,issueLinks");
            }
            if (request?.ContainsKey("maxResults") == false)
            {
                request.Add("maxResults", PageSizeSearch.ToString());
            }
            var response = OpenRequest($"/rest/atm/{ZeyphrApiVersion}/testcase/search")
               .SetQueryParams(request)
               .GetWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry);

            response.AssertResponseStatusForSuccess();
            return ToType<List<TestCase>>(response.ResponseBody.ContentJson);
        }

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
        public TestCase TestCaseCreate(TestCaseCreateRequest jiraTestData)
        {
            Log($"Request to create a test case under project [{jiraTestData.ProjectKey ?? ""}] and folder [{jiraTestData.Folder ?? ""}] with name [{jiraTestData.Name ?? ""}]");

            var response = OpenRequest($"/rest/atm/{ZeyphrApiVersion}/testcase")
               .SetJsonBody(JsonConvert.SerializeObject(jiraTestData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }))
               .PostWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry);

            response.AssertResponseStatusForSuccess();

            return ToType<TestCase>(response.ResponseBody.ContentJson);
        }

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
        public void TestCaseUpdate(string testCaseKey, TestCaseCreateRequest testCase)
        {
            Log($"Request to update test case with key [{testCaseKey}]");

            var response = OpenRequest($"/rest/atm/{ZeyphrApiVersion}/testcase/{testCaseKey}")
               .SetJsonBody(JsonConvert.SerializeObject(testCase, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }))
               .PutWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry);

            response.AssertResponseStatusForSuccess();
        }

        /// <summary>
        /// Get Test case meta data information
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IEnumerable<TestCaseCustomDataMetadata> TestCaseGetCustomMetadata(int projectId)
        {
            Log($"Request to get test case metadata for the project [{projectId}]");

            var response = OpenRequest($"/rest/tests/{ZeyphrApiVersion}/project/{projectId}/customfields/testcase")
              .GetWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry);

            response.AssertResponseStatusForSuccess();
            return ToType<IEnumerable<TestCaseCustomDataMetadata>>(response.ResponseBody.ContentJson);
        }
        #endregion

        #region Test Cycle
        /// <summary>
        /// Retrieve the Test Run matching the given key
        /// </summary>
        /// <param name="testCycleKey"></param>
        /// <returns></returns>
        public TestCycle TestCycleGetById(string testCycleKey)
        {
            Log($"Request to get test cycle by key [{testCycleKey}]");

            var response = OpenRequest($"/rest/atm/{ZeyphrApiVersion}/testrun/{testCycleKey}")
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry);

            response.AssertResponseStatusForSuccess();
            return ToType<TestCycle>(response.ResponseBody.ContentJson);
        }

        /// <summary>
        /// Retrieve the Test Run matching the project key and test cycle key
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testCycleKey"></param>
        /// <param name="includeItems"></param>
        /// <returns></returns>
        public IList<TestCycle> TestCycleGetByKey(string projectKey, string testCycleKey, bool includeItems = false) 
            => TestCycleGetByJql($"projectKey = \"{projectKey}\"", includeItems, (cy) => cy.Key.EqualsIgnoreCase(testCycleKey),
                breakSearchOnFirstConditionValid: true);

        /// <summary>
        /// Retrieve the Test Run matching the project key and test cycle key in a given folder
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="folderName"></param>
        /// <param name="testCycleKey"></param>
        /// <param name="includeItems"></param>
        /// <returns></returns>
        public IList<TestCycle> TestCycleGetByKey(string projectKey, string folderName, string testCycleKey, bool includeItems = false) 
            => TestCycleGetByJql($"projectKey = \"{projectKey}\" AND folder=\"{folderName}\"", includeItems, (cy) => cy.Key.EqualsIgnoreCase(testCycleKey),
                breakSearchOnFirstConditionValid: true);

        /// <summary>
        /// Retrieve the Test Run matching the project key and test cycle name
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="testCycleName"></param>
        /// <param name="includeItems"></param>
        /// <returns></returns>
        public IList<TestCycle> TestCycleGetByName(string projectKey, string testCycleName, bool includeItems = false) 
            => TestCycleGetByJql($"projectKey = \"{projectKey}\"", includeItems, (cy) => cy.Name.EqualsIgnoreCase(testCycleName),
                breakSearchOnFirstConditionValid: true);

        /// <summary>
        /// Retrieve the Test Run matching the project key and test cycle name in a given folder
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="folderName"></param>
        /// <param name="testCycleName"></param>
        /// <param name="includeItems"></param>
        /// <returns></returns>
        public IList<TestCycle> TestCycleGetByName(string projectKey, string folderName, string testCycleName, bool includeItems = false) 
            => TestCycleGetByJql($"projectKey = \"{projectKey}\" AND folder=\"{folderName}\"", includeItems, (cy) => cy.Name.EqualsIgnoreCase(testCycleName),
                breakSearchOnFirstConditionValid: true);

        /// <summary>
        /// Retrieve the Test Run matching the list of projects within their folder
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="includeItems"></param>
        /// <param name="projectKeys"></param>
        /// <returns></returns>
        public IList<TestCycle> TestCycleGetByProjectInFolder(string folderName, bool includeItems = false, params string[] projectKeys) 
            => TestCycleGetByJql($"projectKey IN ({string.Join(",", projectKeys.Select(s => $"\"{s}\""))}) AND folder=\"{folderName}\"", includeItems);

        /// <summary>
        /// Retrieve the Test Run matching the project key within a folder
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="folderName"></param>
        /// <param name="includeItems"></param>
        /// <returns></returns>
        public IList<TestCycle> TestCycleGetByFolder(string projectKey, string folderName, bool includeItems = false) 
            => TestCycleGetByJql($"projectKey = \"{projectKey}\" AND folder=\"{folderName}\"", includeItems);

        /// <summary>
        /// Retrieve the Test Run matching multiple project key 
        /// </summary>
        /// <param name="includeItems"></param>
        /// <param name="projectKeys"></param>
        /// <returns></returns>
        public IList<TestCycle> TestCycleGetByProjectKeys(bool includeItems = false, params string[] projectKeys) 
            => TestCycleGetByJql($"projectKey IN ({string.Join(",", projectKeys.Select(s => $"\"{s}\""))})", includeItems);

        /// <summary>
        /// Retrieve the Test Run by JQL command
        /// </summary>
        /// <param name="jql"></param>
        /// <param name="includeItems"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        public IList<TestCycle> TestCycleGetByJql(string jql, bool includeItems = false, Func<TestCycle, bool> predicate = null, bool breakSearchOnFirstConditionValid = true)
        {
            Log($"Request to get Test Cycle by jql [{jql}]");

            return SearchFull<TestCycle>(new Dictionary<string, string> { { "query", jql }, { "fields", $"key,name,version,iteration{(includeItems ? ",items" : "")}" } },
                TestCycleGet, predicate, breakSearchOnFirstConditionValid);
        }
        
        protected IList<TestCycle> TestCycleGet(IDictionary<string, string> request)
        {
            Log($"Request to get test cycle using [{string.Join(",", request?.Where(s => s.Value.HasValue()).Select(s => $"[{s.Key}, {s.Value}]"))}]");

            if (request?.ContainsKey("fields") == false)
            {
                request.Add("fields", "key,name,version,iteration,items");
            }
            if (request?.ContainsKey("maxResults") == false)
            {
                request.Add("maxResults", PageSizeSearch.ToString());
            }
            var response = OpenRequest($"/rest/atm/{ZeyphrApiVersion}/testrun/search")
               .SetQueryParams(request)
               .GetWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry);
            response.AssertResponseStatusForSuccess();
            return ToType<List<TestCycle>>(response.ResponseBody.ContentJson);
        }

        /// <summary>
        /// Creates a new Test Run.
        /// The fields plannedStartDate and plannedEndDate will be set to default values if not defined.
        /// The field status will be automatically inferred based on the status of Test Run Items (field items).
        /// The Test Run can be linked to a Test Plan, by setting a valid value on field testPlanKey. Also, it can be linked to an issue, by setting a valid value on field issueKey.
        /// All Test Result fields are allowed for Test Run Items (field items).
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TestCycle TestCycleCreate(TestCycleCreateRequest request)
        {
            Log($"Request to create a test cycle under project [{request.ProjectKey ?? ""}] and folder [{request.Folder ?? ""}] with name [{request.Name ?? ""}]");

            var response = OpenRequest($"/rest/atm/{ZeyphrApiVersion}/testrun")
              .SetJsonBody(JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }))
              .PostWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry);

            response.AssertResponseStatusForSuccess();

            return SerializationHelper.ToType<TestCycle>(response.ResponseBody.ContentJson);
        }
        #endregion

        #region Test Execution
        /// <summary>
        /// Create Test Execution result within a Test Run and for a single test case
        /// </summary>
        /// <param name="testrunIdOrKey"></param>
        /// <param name="testcaseIdOrKey"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public TestExecutionResult TestExecutionResultCreateByTestCaseInTestCycle(string testrunIdOrKey, string testcaseIdOrKey, TestExecutionResultCreate request)
        {
            Log($"Request to update test runs with results for run id [{testrunIdOrKey}] and status [{request.Status}] for test case [{testcaseIdOrKey}]");

            var response = OpenRequest($"/rest/atm/{ZeyphrApiVersion}/testrun/{testrunIdOrKey}/testcase/{testcaseIdOrKey}/testresult")
               .SetJsonBody(JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }))
               .PostWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry);

            response.AssertResponseStatusForSuccess();
            return SerializationHelper.ToType<TestExecutionResult>(response.ResponseBody.ContentJson);
        }

        /// <summary>
        /// Update an existing test execution result
        /// </summary>
        /// <param name="testrunId"></param>
        /// <param name="testcaseId"></param>
        /// <param name="request"></param>
        /// <param name="environment"></param>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public TestExecutionResult TestExecutionResultUpdateByTestCaseInTestCycle(string testrunId, string testcaseId, TestExecutionResultCreate request,
            string environment = null, string userKey = null)
        {
            Log($"Request to update test runs with results for run id [{testrunId}] and status [{request.Status}] for test case [{testcaseId}]");

            ParameterCollection keyValuePairs = new ParameterCollection();
            if (environment.HasValue()) keyValuePairs.Add("environment", environment);
            if (userKey.HasValue()) keyValuePairs.Add("userKey", userKey);

            var response = OpenRequest($"/rest/atm/{ZeyphrApiVersion}/testrun/{testrunId}/testcase/{testcaseId}/testresult")
               .SetJsonBody(JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }))
               .SetQueryParamsAsHeader(keyValuePairs)
               .PutWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry);

            response.AssertResponseStatusForSuccess();
            return SerializationHelper.ToType<TestExecutionResult>(response.ResponseBody.ContentJson);
        }

        /// <summary>
        /// Create one or more (multiple) new Test Execution result under a test cycle
        /// </summary>
        /// <param name="testCycleKey"></param>
        /// <param name="request"></param>
        /// <param name="environment"></param>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public List<TestExecutionResult> TestExecutionResultCreateByTestCycle(string testCycleKey, List<TestExecutionResultCreate> request,
            string environment = null, string userKey = null)
        {
            Log($"Request to create test execution within a test cycle [{testCycleKey}]");

            ParameterCollection keyValuePairs = new ParameterCollection();
            if (environment.HasValue()) keyValuePairs.Add("environment", environment);
            if (userKey.HasValue()) keyValuePairs.Add("userKey", userKey);

            var response = OpenRequest($"/rest/atm/{ZeyphrApiVersion}/testrun/{testCycleKey}/testresults")
               .SetJsonBody(JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }))
               .SetQueryParamsAsHeader(keyValuePairs)
               .PostWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry);

            response.AssertResponseStatusForSuccess();
            return SerializationHelper.ToType<List<TestExecutionResult>>(response.ResponseBody.ContentJson);
        }

        /// <summary>
        /// Retrieve test execution results within a test cycle
        /// </summary>
        /// <param name="testCycleKey"></param>
        /// <returns></returns>
        public IList<TestExecutionResult> TestExecutionResultGetByTestCycle(string testCycleKey)
        {
            Log($"Request to get test execution results within a test cycle [{testCycleKey}]");

            var response = OpenRequest($"/rest/atm/{ZeyphrApiVersion}/testrun/{testCycleKey}/testresults")
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry);

            response.AssertResponseStatusForSuccess();
            return SerializationHelper.ToType<List<TestExecutionResult>>(response.ResponseBody.ContentJson);
        }
        #endregion

        /// <summary>
        /// Perform custom search
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchQuery"></param>
        /// <param name="search"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        protected IList<T> SearchFull<T>(
            IDictionary<string, string> searchQuery,
            Func<IDictionary<string, string>, IList<T>> search,
            Func<T, bool> predicate = null,
            bool breakSearchOnFirstConditionValid = true)
        {
            var results = new ConcurrentBag<T>();
            var tempStore = new List<T>();
            if (searchQuery.ContainsKey("maxResults")) searchQuery["maxResults"] = PageSizeSearch.ToString(); else searchQuery.Add("maxResults", PageSizeSearch.ToString());
            if (searchQuery.ContainsKey("startAt")) searchQuery["startAt"] = "0"; else searchQuery.Add("startAt", "0");
            while ((tempStore = search(searchQuery).ToList()).Any())
            {
                if (predicate != null)
                {
                    foreach (var value in tempStore)
                    {
                        if (predicate(value) == false) continue;
                        results.Add(value);
                        if (breakSearchOnFirstConditionValid) break;
                    }
                }
                else
                {
                    tempStore.ForEach(value => results.Add(value));
                }
                searchQuery["startAt"] = (searchQuery["startAt"].ToInteger() + searchQuery["maxResults"].ToInteger()).ToString();
                if (tempStore.Count < PageSizeSearch) break;
            }
            return results.ToList();
        }
    }
}
