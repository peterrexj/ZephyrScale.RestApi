using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pj.Library;
using TestAny.Essentials.Api;
using TestAny.Essentials.Core.Dtos.Api;
using ZephyrScale.RestApi.Dtos.Cloud;
using Environment = ZephyrScale.RestApi.Dtos.Cloud.Environment;

namespace ZephyrScale.RestApi.Service.Cloud
{
    public class ZephyrScaleCloudService : ZephyrScaleCloudServiceBase, IZephyrScaleCloudService
    {
        /// <summary>
        /// Initialize the Zephyr service with required parameters
        /// </summary>
        /// <param name="appUrl">Zephyr endpoint, for example: https://app.tm4j.smartbear.com</param>
        /// <param name="passwordAuthKey">API token used to connect the service</param>
        /// <param name="restApiVersion">Rest API version for the Zephyr Service (default value: 'v2')</param>
        /// <param name="folderSeparator">Folder separator string (default value: '/')</param>
        /// <param name="logPrefix">Prefix text that will be added to all the logs generated from this service (default value: 'Zephyr: ')</param>
        /// <param name="pageSizeSearchResult">Page size for search request (default value: '50')</param>
        /// <param name="requestRetryTimes">Number of time to retry when there is a network failure (default value: '1'). You can increase the number of times to retry based on your infrastructure if there are chance for a request to fail randomly</param>
        /// <param name="timeToSleepBetweenRetryInMilliseconds">Time to sleep in milliseconds between each time a call is retry (default value: '1000'). Applied only when requestRetryTimes is more than 1</param>
        /// <param name="assertResponseStatusOk">True/False whether the response code status from the server needs to be asserted for OK (default value 'true')</param>
        /// <param name="listOfResponseCodeOnFailureToRetry">Any of these status code matched from response will then use for retry the request. For example Proxy Authentication randomly failing can be then used to retry (default value 'null' which means it is not checking any response code for fail retry)</param>
        /// <param name="requestTimeoutInSeconds">Control the total time to wait for any request made to the Zephyr Scale server. Default time is set to 300 seconds and it can be increased if the data on the server is too many and requires more time to process to respond</param>
        /// <param name="retryOnRequestTimeout">True/False whether the request should retry on when the server fails to respond within the timeout period, retry on when server timeouts for a request</param>
        /// <param name="proxyKeyName">Key to the proxy details. Refer readme for more information on how to set the custom proxy for every request</param>
        public ZephyrScaleCloudService(string appUrl,
            string passwordAuthKey,
            string restApiVersion = "v2",
            string folderSeparator = "/",
            string logPrefix = "Zephyr: ",
            int pageSizeSearchResult = 50,
            int requestRetryTimes = 1,
            int timeToSleepBetweenRetryInMilliseconds = 1000,
            bool assertResponseStatusOk = true,
            HttpStatusCode[] listOfResponseCodeOnFailureToRetry = null,
            int requestTimeoutInSeconds = 300,
            bool retryOnRequestTimeout = false,
            string proxyKeyName = "")
                : base(appUrl, passwordAuthKey, restApiVersion, folderSeparator,
                     logPrefix, pageSizeSearchResult, requestRetryTimes,
                     timeToSleepBetweenRetryInMilliseconds, assertResponseStatusOk,
                     listOfResponseCodeOnFailureToRetry, requestTimeoutInSeconds,
                     retryOnRequestTimeout, proxyKeyName)
        { }

        #region Test Case
        /// <summary>
        /// Creates a test case. Fields priorityName and statusName will be set to default values if not informed. Default values are usually “Normal” for priorityName and “Draft” for statusName. All required test case custom fields should be present in the request.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createTestCase
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public TestCase TestCaseCreate(TestCaseCreateRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.ProjectKey.IsEmpty()) throw new Exception($"The request to create a test case does not contain Project key information");
            if (request.Name.IsEmpty()) throw new Exception($"The request to create a test case does not contain Test Case name information");

            Log($"Request to create test case with {request.Name} under project {request.ProjectKey}");

            var response = OpenRequest($"/{ZephyrApiVersion}/testcases")
               .SetJsonBody(request)
               .SetTimeout(RequestTimeoutInSeconds)
               .PostWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                   retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<TestCase>(response.ResponseBody.ContentString);
        }

        /// <summary>
        /// Updates an existing test case. If the project has test case custom fields, all custom fields should be present in the request. To leave any of them blank, please set them null if they are not required custom fields.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/updateTestCase
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public TestCase TestCaseUpdate(TestCase request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.Project == null) throw new ArgumentNullException(nameof(request.Project));
            if (request.Project.Id.HasValue == false || request.Project.Id.Value == 0) throw new Exception($"The request to update a test case does not contain Project Id");
            if (request.Id.HasValue == false || request.Id.Value == 0) throw new Exception($"The request to update a test case does not contain Test Case Id");
            if (request.Key.IsEmpty()) throw new Exception($"The request to update a test case does not contain Test Case key");
            if (request.Name.IsEmpty()) throw new Exception($"The request to update a test case does not contain Test Case name");
            if (request.Priority == null) throw new ArgumentNullException(nameof(request.Priority));
            if (request.Priority.Id.HasValue == false || request.Priority.Id.Value == 0) throw new Exception($"The request to update a test case does not contain Priority Id");
            if (request.Status == null) throw new ArgumentNullException(nameof(request.Status));
            if (request.Status.Id.HasValue == false || request.Status.Id.Value == 0) throw new Exception($"The request to update a test case does not contain Status Id");

            Log($"Request to update test case with key {request.Key}");

            var response = OpenRequest($"/{ZephyrApiVersion}/testcases/{request.Key}")
                .SetJsonBody(request)
                .SetTimeout(RequestTimeoutInSeconds)
                .PutWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<TestCase>(response.ResponseBody.ContentString);
        }

        /// <summary>
        /// Returns a test case for the given key.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getTestCase
        /// </summary>
        /// <param name="testCaseKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public TestCase TestCaseGetById(string testCaseKey)
        {
            if (testCaseKey.IsEmpty()) throw new Exception($"The request to search a test case does not contain test case key");

            Log($"Request to get test case by key [{testCaseKey}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/testcases/{testCaseKey}")
                .SetTimeout(RequestTimeoutInSeconds)
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<TestCase>(response.ResponseBody.ContentString);
        }

        /// <summary>
        /// Returns the list of test cases based on the search request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Pagination<TestCase> TestCasesGet(TestSearchRequest request) => TestCasesGet(request.GetPropertyValuesV2());
        protected Pagination<TestCase> TestCasesGet(IDictionary<string, string> request)
        {
            Log($"Request to get test case list using [{string.Join(",", request?.Where(s => s.Value.HasValue()).Select(s => $"[{s.Key}, {s.Value}]") ?? Array.Empty<string>())}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/testcases")
                .SetQueryParams(request)
                .SetTimeout(RequestTimeoutInSeconds)
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<Pagination<TestCase>>(response.ResponseBody.ContentString);
        }

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
        public List<TestCase> TestCasesGetFull(string projectKey, long? folderId = null,
            Func<TestCase, bool> predicate = null, bool breakSearchOnFirstConditionValid = true)
            => SearchFull(new TestSearchRequest { projectKey = projectKey, folderId = folderId }.GetPropertyValuesV2(),
                TestCasesGet, predicate, breakSearchOnFirstConditionValid).ToList();

        /// <summary>
        /// Returns links for a test case with specified key.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getTestCaseLinks
        /// </summary>
        /// <param name="testCaseKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Links TestCaseLinksGet(string testCaseKey)
        {
            if (testCaseKey.IsEmpty()) throw new Exception($"The request to search a test case link does not contain test case id");

            Log($"Request to get test case link by key [{testCaseKey}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/testcases/{testCaseKey}/links")
               .SetTimeout(RequestTimeoutInSeconds)
               .GetWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                   retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<Links>(response.ResponseBody.ContentString);
        }

        /// <summary>
        /// Creates a link between a test case and a Jira issue
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createTestCaseIssueLinks
        /// </summary>
        /// <param name="testCaseKey"></param>
        /// <param name="issueId"></param>
        /// <exception cref="Exception"></exception>
        public void TestCaseLinkCreate(string testCaseKey, long issueId)
        {
            if (testCaseKey.IsEmpty()) throw new Exception($"The request to create a test case link does not contain test case id");
            if (issueId <= 0) throw new Exception($"The request to create a test case link does not contain issue id");

            Log($"Request to create test case link between test case key [{testCaseKey}] and issue id [{issueId}]");

            var body = new
            {
                issueId
            };
            var response = OpenRequest($"/{ZephyrApiVersion}/testcases/{testCaseKey}/links/issues")
                .SetTimeout(RequestTimeoutInSeconds)
                .SetJsonBody(body)
                .PostWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);
            response.AssertResponseStatusForSuccess();
        }

        /// <summary>
        /// Provides the custom fields configured on the test case
        /// </summary>
        /// <param name="projectKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<string> TestCaseCustomFieldNames(string projectKey)
        {
            Log($"Request to get test case custom field names by project key [{projectKey}]");

            var testCases = TestCasesGet(new TestSearchRequest { projectKey = projectKey, startAt = 0, maxResults = 1 }).Values?.FirstOrDefault();
            if (testCases == null)
            {
                throw new Exception($"There are no test cases available in the project [{projectKey}]. Create at least one Test case and try again!");
            }
            return testCases.CustomFields.GetPropertyNamesV2();
        }

        /// <summary>
        /// Retrieves the custom field information on the first available test case.
        /// This can be used to create a new test case
        /// </summary>
        /// <param name="projectKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public dynamic TestCaseCustomFieldName(string projectKey)
        {
            Log($"Request to get test case custom field(s) by project key [{projectKey}]");

            var testCases = TestCasesGet(new TestSearchRequest { projectKey = projectKey, startAt = 0, maxResults = 1 }).Values?.FirstOrDefault();
            if (testCases == null)
            {
                throw new Exception($"There are no test cases available in the project [{projectKey}]. Create at least one Test case and try again!");
            }
            return testCases.CustomFields;
        }
        #endregion

        #region Folder
        /// <summary>
        /// Returns a folder for the given ID
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getFolder
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Folder FolderGetById(string folderId)
        {
            Log($"Request to get folder by id [{folderId}]");

            if (folderId.IsEmpty()) throw new Exception($"The request to search a folder does not contain folder id");

            var response = OpenRequest($"/{ZephyrApiVersion}/folders/{folderId}")
               .SetTimeout(RequestTimeoutInSeconds)
               .GetWithRetry(assertOk: AssertResponseStatusOk,
                   timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                   retryOption: RequestRetryTimes,
                   httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                   retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<Folder>(response.ResponseBody.ContentString);
        }

        /// <summary>
        /// Creates a folder
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createFolder
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Folder FolderCreate(FolderCreateRequest request)
        {
            Log($"Request to create a folder [{request?.Name}]");

            var validate = FolderCreateValidate(request);
            if (validate.HasValue())
            {
                Log(validate);
                return null;
            }

            var response = OpenRequest($"/{ZephyrApiVersion}/folders")
                .SetJsonBody(request)
                .SetTimeout(RequestTimeoutInSeconds)
                .PostWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<Folder>(response.ResponseBody.ContentString);
        }

        /// <summary>
        /// Creates the full folder structure
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createFolder
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Folder FolderCreateRecursive(FolderCreateRequest request)
        {
            Log($"Request to create folder(s) [{request?.Name}]");

            var validate = FolderCreateValidate(request);
            if (validate.HasValue())
            {
                Log(validate);
                return null;
            }

            //Check if the Name property contains path
            var folderPathBreakdown = request.Name
                .ReplaceMultiple(FolderSeparator, "/", "//", @"\", @"\\", ",", ";")
                .SplitAndTrim(FolderSeparator).ToList();
            var fullPath = $"{FolderSeparator}{string.Join(FolderSeparator, folderPathBreakdown)}";
            var existingFolders = FoldersGetFull(request.ProjectKey, (FolderType)Enum.Parse(typeof(FolderType), request.FolderType));
            if (existingFolders.Any(a => a.FullPath.EqualsIgnoreCase(fullPath)))
            {
                return existingFolders.FirstOrDefault(a => a.FullPath.EqualsIgnoreCase(fullPath));
            }
            string pathToCheck = string.Empty;
            long? parentId = null;
            Folder newFolder = null;
            for (int i = 0; i < folderPathBreakdown.Count(); i++)
            {
                pathToCheck += $"{FolderSeparator}{folderPathBreakdown[i]}";
                if (existingFolders.Any(f => f.FullPath.EqualsIgnoreCase(pathToCheck)))
                {
                    parentId = existingFolders.FirstOrDefault(f => f.FullPath.EqualsIgnoreCase(pathToCheck))?.Id.Value;
                }
                else
                {
                    newFolder = FolderCreate(new FolderCreateRequest
                    {
                        ProjectKey = request.ProjectKey,
                        FolderType = request.FolderType,
                        Name = folderPathBreakdown[i],
                        ParentId = parentId,
                    });
                    if (newFolder == null)
                    {
                        throw new Exception($"Folder create failed for [{folderPathBreakdown[i]}] under project [{request.ProjectKey}]");
                    }
                    parentId = newFolder.Id.Value;
                }
            }
            return newFolder;
        }

        /// <summary>
        /// Returns the list of the folders based on the search request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Pagination<Folder> FoldersGet(FolderSearchRequest request) => FoldersGet(request.GetPropertyValues());
        protected Pagination<Folder> FoldersGet(IDictionary<string, string> request)
        {
            Log($"Request to get folder list using [{string.Join(",", request?.Where(s => s.Value.HasValue()).Select(s => $"[{s.Key}, {s.Value}]"))}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/folders")
                .SetQueryParams(request)
                .SetTimeout(RequestTimeoutInSeconds)
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<Pagination<Folder>>(response.ResponseBody.ContentString);
        }

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
        public List<Folder> FoldersGetFull(string projectKey, FolderType folderType, Func<Folder, bool> predicate = null, bool breakSearchOnFirstConditionValid = true)
            => FolderWithFullPath(SearchFull(new FolderSearchRequest { projectKey = projectKey, folderType = folderType }.GetPropertyValuesV2(),
                FoldersGet, predicate, breakSearchOnFirstConditionValid).ToList());

        /// <summary>
        /// Returns all folders and build the full path
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/listFolders
        /// </summary>
        /// <param name="folders"></param>
        /// <returns></returns>
        public List<Folder> FolderWithFullPath(List<Folder> folders)
        {
            Log($"Request to retrieve folders with full path");

            foreach (var folder in folders)
            {
                if (folder.ParentId.HasValue && folder.ParentId.Value > 0)
                {
                    folder.FullPath = $@"{FolderSeparator}{string.Join(FolderSeparator, FolderBuildChilds(folders, folder.ParentId, folder.Name)
                            .SplitAndTrim(FolderSeparator)
                            .Reverse())}";
                }
                else
                {
                    folder.FullPath = $"{FolderSeparator}{folder.Name}";
                }
            }
            return folders;
        }
        private string FolderBuildChilds(List<Folder> folders, Int64? parentId, string path)
        {
            var hasMoreParent = folders.Where(f => parentId != null && f.Id == parentId.Value).FirstOrDefault(f => f.ParentId != null && f.ParentId.Value > 0);
            if (hasMoreParent != null)
            {
                path += $"{FolderSeparator}{FolderBuildChilds(folders, hasMoreParent.ParentId, hasMoreParent.Name)}";
            }
            else
            {
                if (parentId.HasValue && parentId.Value > 0)
                {
                    path += $"{FolderSeparator}{folders.FirstOrDefault(f => f.Id == parentId.Value)?.Name}";
                }
            }
            return path;
        }
        private string FolderCreateValidate(FolderCreateRequest request)
        {
            if (request == null)
            {
                return "Folder request not processed as the parameter [request] object is null";
            }
            if (request?.ProjectKey.IsEmpty() == true)
            {
                return "Folder request not processed as the parameter [projectKey] within [request] object is empty";
            }
            if (request?.Name.IsEmpty() == true)
            {
                return "Folder request not processed as the parameter [Name] within [request] object is empty";
            }
            if (request?.FolderType.IsEmpty() == true)
            {
                return "Folder request not processed as the parameter [FolderType] within [request] object is empty";
            }
            var validFolderTypes = new[] { "TEST_CASE", "TEST_PLAN", "TEST_CYCLE" };
            if (validFolderTypes.Contains(request?.FolderType) == false)
            {
                return $"Folder request not processed as the parameter [FolderType] within [request] object has an invalid value. Current value [{request?.FolderType}], valid values [{string.Join(", ", validFolderTypes)}]";
            }

            return string.Empty;
        }
        //public void DeleteFolder(string folderId)
        //{
        //    //if (folderId.IsEmpty()) return null;
        //    var jiraResponse = OpenRequest($"/v2/folders/{folderId}")
        //       .Delete();
        //    jiraResponse.AssertResponseStatusForSuccess();
        //}
        #endregion

        #region Test Cycle
        /// <summary>
        /// Creates a Test Cycle. All required test cycle custom fields should be present in the request. Refer the link for more details
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createTestCycle
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public TestCycle TestCycleCreate(TestCycleCreateRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.ProjectKey.IsEmpty()) throw new Exception($"The request to create a test cycle does not contain project name");
            if (request.Name.IsEmpty()) throw new Exception($"The request to create a test cycle does not contain test cycle name");

            Log($"Request to create Test Cycle in project [{request.ProjectKey}] with name [{request.Name}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/testcycles")
                .SetJsonBody(request)
                .SetTimeout(RequestTimeoutInSeconds)
                .PostWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<TestCycle>(response.ResponseBody.ContentString);
        }

        /// <summary>
        /// Returns a test cycle for the given key
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getTestCycle
        /// </summary>
        /// <param name="testCycleIdOrKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public TestCycle TestCycleGet(string testCycleIdOrKey)
        {
            if (testCycleIdOrKey.IsEmpty()) throw new Exception($"The request to search a test cycle does not contain test cycle id");

            Log($"Request to get Test Cycle by test cycle key [{testCycleIdOrKey}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/testcycles/{testCycleIdOrKey}")
              .SetTimeout(RequestTimeoutInSeconds)
              .GetWithRetry(assertOk: AssertResponseStatusOk,
                  timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                  retryOption: RequestRetryTimes,
                  httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                  retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<TestCycle>(response.ResponseBody.ContentString);
        }

        /// <summary>
        /// Returns the list of test cycle based on the search request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Pagination<TestCycle> TestCyclesGet(TestSearchRequest request) => TestCyclesGet(request.GetPropertyValuesV2());
        protected Pagination<TestCycle> TestCyclesGet(IDictionary<string, string> request)
        {
            Log($"Request to get test cycle list using [{string.Join(",", request?.Where(s => s.Value.HasValue()).Select(s => $"[{s.Key}, {s.Value}]") ?? Array.Empty<string>())}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/testcycles")
                .SetQueryParams(request)
                .SetTimeout(RequestTimeoutInSeconds)
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<Pagination<TestCycle>>(response.ResponseBody.ContentString);
        }

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
        /// <param name="jiraProjectVersionId"></param>
        /// <returns></returns>
        public List<TestCycle> TestCyclesGetFull(string projectKey, long? folderId = null,
            Func<TestCycle, bool> predicate = null, 
            bool breakSearchOnFirstConditionValid = true,
            long? jiraProjectVersionId = null)
            => SearchFull(new TestSearchRequest { projectKey = projectKey, folderId = folderId, jiraProjectVersionId = jiraProjectVersionId }.GetPropertyValuesV2(),
                TestCyclesGet, predicate, breakSearchOnFirstConditionValid).ToList();

        /// <summary>
        /// Updates an existing test cycle. If the project has test cycle custom fields, all custom fields should be present in the request. To leave any of them blank, please set them null if they are not required custom fields.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/updateTestCycle
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public TestCycle TestCycleUpdate(TestCycle request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.Project == null) throw new ArgumentNullException(nameof(request.Project));
            if (request.Project.Id.HasValue == false || request.Project.Id.Value == 0) throw new Exception($"The request to update a test cycle does not contain Project Id");
            if (request.Id.HasValue == false || request.Id.Value == 0) throw new Exception($"The request to update a test cycle does not contain test cyclease Id");
            if (request.Key.IsEmpty()) throw new Exception($"The request to update a test case does not contain Test Case key");
            if (request.Name.IsEmpty()) throw new Exception($"The request to update a test case does not contain Test Case name");
            if (request.Status == null) throw new ArgumentNullException(nameof(request.Status));
            if (request.Status.Id.HasValue == false || request.Status.Id.Value == 0) throw new Exception($"The request to update a test case does not contain Status Id");

            Log($"Request to update test cycle by key [{request.Key}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/testcycles/{request.Key}")
                .SetJsonBody(request)
                .SetTimeout(RequestTimeoutInSeconds)
                .PutWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<TestCycle>(response.ResponseBody.ContentString);
        }

        /// <summary>
        /// Returns links for a test cycle with specified key.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getTestCycleLinks
        /// </summary>
        /// <param name="testCycleIdOrKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Links TestCycleLinksGet(string testCycleIdOrKey)
        {
            if (testCycleIdOrKey.IsEmpty()) throw new Exception($"The request to search a test cycle link does not contain test cycle id");

            Log($"Request to get test cycle links by key [{testCycleIdOrKey}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/testcycles/{testCycleIdOrKey}/links")
                .SetTimeout(RequestTimeoutInSeconds)
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<Links>(response.ResponseBody.ContentString);
        }

        /// <summary>
        /// Creates a link between a test cycle and a Jira issue
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createTestCycleIssueLink
        /// </summary>
        /// <param name="testCycleIdOrKey"></param>
        /// <param name="issueId"></param>
        /// <exception cref="Exception"></exception>
        public void TestCycleLinkCreate(string testCycleIdOrKey, Int64 issueId)
        {
            if (testCycleIdOrKey.IsEmpty()) throw new Exception($"The request to create a test cycle link does not contain test cycle id");
            if (issueId <= 0) throw new Exception($"The request to create a test cycle link does not contain issue id");

            Log($"Request to link test cycle [{testCycleIdOrKey}] and issue [{issueId}]");

            var body = new
            {
                issueId = issueId
            };
            var response = OpenRequest($"/{ZephyrApiVersion}/testcycles/{testCycleIdOrKey}/links/issues")
                .SetJsonBody(body)
                .SetTimeout(RequestTimeoutInSeconds)
                .PostWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);
            response.AssertResponseStatusForSuccess();
        }

        /// <summary>
        /// Returns the custom field names configured in the test cycle
        /// </summary>
        /// <param name="projectKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<string> TestCycleCustomFieldNames(string projectKey)
        {
            Log($"Request to get test cycle custom field(s) by project key [{projectKey}]");

            var testCycles = TestCyclesGet(new TestSearchRequest { projectKey = projectKey, startAt = 0, maxResults = 1 }).Values?.FirstOrDefault();
            if (testCycles == null)
            {
                throw new Exception($"There are no test cycle available in the project [{projectKey}]. Create at least one Test cycle and try again!");
            }
            return testCycles.CustomFields.GetPropertyNamesV2();
        }
        #endregion

        #region Test Execution
        /// <summary>
        /// Creates a test execution. All required test execution custom fields should be present in the request
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createTestExecution
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public void TestExecutionCreate(TestExecutionCreateRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.ProjectKey.IsEmpty()) throw new Exception($"The request to create a test execution does not contain project key");
            if (request.TestCaseKey.IsEmpty()) throw new Exception($"The request to create a test execution does not contain test case key");
            if (request.TestCycleKey.IsEmpty()) throw new Exception($"The request to create a test execution does not contain test cycle key");
            if (request.StatusName.IsEmpty()) throw new Exception($"The request to create a test execution does not contain status name");

            Log($"Request to create new execution between test case [{request.TestCaseKey}] test cycle [{request.TestCycleKey}] with status [{request.StatusName}] in project [{request.ProjectKey}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/testexecutions")
                .SetJsonBody(request)
                .SetTimeout(RequestTimeoutInSeconds)
                .PostWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);
            response.AssertResponseStatusForSuccess();
        }

        /// <summary>
        /// Returns a test execution for the given ID
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getTestExecution
        /// </summary>
        /// <param name="testExecutionIdOrKey"></param>
        /// <param name="includeStepLinks"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public TestExecution TestExecutionGet(string testExecutionIdOrKey, bool includeStepLinks = false)
        {
            if (testExecutionIdOrKey.IsEmpty()) throw new Exception($"The request to search a test execution does not contain test execution id");

            Log($"Request to get test execution by key [{testExecutionIdOrKey}] with include steps as [{includeStepLinks}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/testexecutions/{testExecutionIdOrKey}")
                .SetTimeout(RequestTimeoutInSeconds)
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<TestExecution>(response.ResponseBody.ContentString);
        }

        /// <summary>
        /// Returns the list of test execution based on the search request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Pagination<TestExecution> TestExecutionsGet(TestExecutionSearchRequest request) =>
            TestExecutionsGet(request.GetPropertyValuesV2());
        protected Pagination<TestExecution> TestExecutionsGet(IDictionary<string, string> request)
        {
            Log($"Request to get test execution list using [{string.Join(",", request?.Where(s => s.Value.HasValue()).Select(s => $"[{s.Key}, {s.Value}]"))}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/testexecutions")
                .SetQueryParams(request)
                .SetTimeout(RequestTimeoutInSeconds)
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<Pagination<TestExecution>>(response.ResponseBody.ContentString);
        }

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
        /// <param name="jiraProjectVersionId"></param>
        /// <param name="onlyLastExecutions"></param>
        /// <returns></returns>
        public List<TestExecution> TestExecutionsGetFull(
            string projectKey,
            string testCase = null,
            string testCycle = null,
            DateTime? actualEndDateAfter = null,
            DateTime? actualEndDateBefore = null,
            Func<TestExecution, bool> predicate = null, 
            bool breakSearchOnFirstConditionValid = true,
            long? jiraProjectVersionId = null,
            bool? onlyLastExecutions = null)
            => SearchFull(
                new
                {
                    projectKey,
                    testCase,
                    testCycle,
                    actualEndDateBefore = actualEndDateBefore.HasValue ? actualEndDateBefore.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") : null,
                    actualEndDateAfter = actualEndDateAfter.HasValue ? actualEndDateAfter.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") : null,
                    jiraProjectVersionId = jiraProjectVersionId,
                    onlyLastExecutions = onlyLastExecutions,
                }.GetPropertyValuesV2(),
                TestExecutionsGet, predicate, breakSearchOnFirstConditionValid).ToList();
        #endregion

        #region Status
        /// <summary>
        /// Returns a status for the given ID.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getStatus
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Status StatusGet(string statusId)
        {
            if (statusId.IsEmpty()) throw new Exception($"The request to search a status does not contain status id");

            Log($"Request to get status by id [{statusId}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/statuses/{statusId}")
                .SetTimeout(RequestTimeoutInSeconds)
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<Status>(response.ResponseBody.ContentString);
        }

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
        public List<Status> StatusesGetFull(string projectKey, string statusType = null, Func<Status, bool> predicate = null, bool breakSearchOnFirstConditionValid = true)
            => SearchFull(new StatusSearchRequest { projectKey = projectKey, statusType = statusType }.GetPropertyValues(),
                StatusesGet, predicate, breakSearchOnFirstConditionValid).ToList();

        /// <summary>
        /// Returns the list of statuses based on the search request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Pagination<Status> StatusesGet(StatusSearchRequest request) =>
            StatusesGet(request.GetPropertyValuesV2());
        protected Pagination<Status> StatusesGet(IDictionary<string, string> request)
        {
            Log($"Request to get status list using [{string.Join(",", request?.Where(s => s.Value.HasValue()).Select(s => $"[{s.Key}, {s.Value}]"))}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/statuses")
                .SetQueryParams(request)
                .SetTimeout(RequestTimeoutInSeconds)
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<Pagination<Status>>(response.ResponseBody.ContentString);
        }
        #endregion

        #region Environment
        /// <summary>
        /// Returns the list of environment based on the search request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Pagination<Environment> EnvironmentsGet(SearchRequestBase request) => EnvironmentsGet(request.GetPropertyValuesV2());
        protected Pagination<Environment> EnvironmentsGet(IDictionary<string, string> request)
        {
            Log($"Request to get environment list using [{string.Join(",", request?.Where(s => s.Value.HasValue()).Select(s => $"[{s.Key}, {s.Value}]"))}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/environments")
                .SetQueryParams(request)
                .SetTimeout(RequestTimeoutInSeconds)
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<Pagination<Environment>>(response.ResponseBody.ContentString);
        }

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
        public List<Environment> EnvironmentsGetFull(string projectKey,
            Func<Environment, bool> predicate = null,
            bool breakSearchOnFirstConditionValid = true)
            => SearchFull(new SearchRequestBase { projectKey = projectKey }.GetPropertyValuesV2(),
                EnvironmentsGet, predicate, breakSearchOnFirstConditionValid).ToList();

        /// <summary>
        /// Returns an environment for the given ID.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getEnvironment
        /// </summary>
        /// <param name="environmentId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Environment EnvironmentGet(string environmentId)
        {
            if (environmentId.IsEmpty()) throw new Exception($"The request to search a environment does not contain environment id");

            Log($"Request to get environment by id [{environmentId}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/environments/{environmentId}")
                .SetTimeout(RequestTimeoutInSeconds)
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();
            return ToType<Environment>(response.ResponseBody.ContentString);
        }
        #endregion

        #region Project
        /// <summary>
        /// Returns a project for the given ID or key.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getProject
        /// </summary>
        /// <param name="projectIdOrKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Project ProjectGet(string projectIdOrKey)
        {
            if (projectIdOrKey.IsEmpty()) throw new Exception($"The request to search a project does not contain project id");

            Log($"Request to get the project by key [{projectIdOrKey}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/projects/{projectIdOrKey}")
                .SetTimeout(RequestTimeoutInSeconds)
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();

            return ToType<Project>(response.ResponseBody.ContentString);
        }

        /// <summary>
        /// Returns the project based on the search query
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Pagination<Project> ProjectsGet(SearchRequestBase request = null) => ProjectsGet(request.GetPropertyValuesV2());
        protected Pagination<Project> ProjectsGet(IDictionary<string, string> request)
        {
            if (request == null) request = new Dictionary<string, string>();

            Log($"Request to get project list using [{string.Join(",", request?.Where(s => s.Value.HasValue()).Select(s => $"[{s.Key}, {s.Value}]"))}]");

            var response = OpenRequest($"/{ZephyrApiVersion}/projects")
                .SetQueryParams(request)
                .SetTimeout(RequestTimeoutInSeconds)
                .GetWithRetry(assertOk: AssertResponseStatusOk,
                    timeToSleepBetweenRetryInMilliseconds: TimeToSleepBetweenRetryInMilliseconds,
                    retryOption: RequestRetryTimes,
                    httpStatusCodes: ListOfResponseCodeOnFailureToRetry,
                    retryOnRequestTimeout: RetryOnRequestTimeout);

            response.AssertResponseStatusForSuccess();

            return ToType<Pagination<Project>>(response.ResponseBody.ContentString);
        }

        /// <summary>
        /// Returns all projects.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/listProjects
        /// Use predicate to streamline your result, for examplev: (t) => t.Name.EqualsIgnoreCase("test") 
        /// Use [breakSearchOnFirstConditionValid] if you want break the search once the match is found. This can improve performance when searching for specific item
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        public List<Project> ProjectsGetFull(Func<Project, bool> predicate = null,
            bool breakSearchOnFirstConditionValid = true)
            => SearchFull(new SearchRequestBase { }.GetPropertyValuesV2(), ProjectsGet, predicate, breakSearchOnFirstConditionValid).ToList();
        #endregion

        #region Links
        public void LinkDelete(string linkId)
        {
            Log($"Warning: Trying to delete the link {linkId}");

            var response = OpenRequest($"/{ZephyrApiVersion}/links/{linkId}").Delete();

            response.AssertResponseStatusForSuccess();
        }
        #endregion

        #region Search
        protected IList<T> SearchFull<T>(
            IDictionary<string, string> searchQuery,
            Func<IDictionary<string, string>, Pagination<T>> search,
            Func<T, bool> predicate = null,
            bool breakSearchOnFirstConditionValid = true)
        {
            var results = new ConcurrentBag<T>();
            if (searchQuery.ContainsKey("maxResults")) searchQuery["maxResults"] = PageSizeSearch.ToString(); else searchQuery.Add("maxResults", PageSizeSearch.ToString());
            if (searchQuery.ContainsKey("startAt")) searchQuery["startAt"] = "0"; else searchQuery.Add("startAt", "0");

            var resp = search(searchQuery);

            if (predicate != null)
            {
                foreach (var value in resp.Values)
                {
                    if (predicate(value) != true) continue;
                    results.Add(value);
                    if (breakSearchOnFirstConditionValid)
                    {
                        return results.ToList();
                    }
                }
            }
            else
            {
                resp.Values.Iter(r => results.Add(r));
            }

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            if (!resp.IsLast)
            {
                var totalPages = new PagingModel { TotalItems = (int)resp.Total, PageSize = (int)resp.MaxResults }.TotalPagesAvailable;
                int count = 0;

                try
                {
                    Parallel.For(1, totalPages, new ParallelOptions { MaxDegreeOfParallelism = 5, CancellationToken = cancellationToken }, i =>
                    {
                        lock (_lock) { count++; }
                        var currentSearchQry = searchQuery.DeepClone();
                        currentSearchQry["startAt"] = (i * PageSizeSearch).ToString();

                        PjUtility.Log($"Trying to read {count} of {totalPages} starting at {currentSearchQry["startAt"]}");
                        var values = search(currentSearchQry).Values;

                        if (predicate != null)
                        {
                            foreach (var value in values)
                            {
                                if (predicate(value) != true) continue;
                                results.Add(value);
                                if (breakSearchOnFirstConditionValid)
                                {
                                    cancellationTokenSource.Cancel();
                                }
                            }
                        }
                        else
                        {
                            values.Iter(r => results.Add(r));
                        }
                    });
                }
                catch (OperationCanceledException)
                {
                }
                finally
                {
                    cancellationTokenSource.Dispose();
                }
            }
            return results.ToList();
        }
        #endregion

        #region IssueLinks
        public List<TestCycle> IssueLinksTestCycles(string issueKey)
        {
            if (issueKey.IsEmpty()) throw new ArgumentNullException(nameof(issueKey));

            var response = OpenRequest($"/{ZephyrApiVersion}/issuelinks/{issueKey}/testcycles").Get();
            
            response.AssertResponseStatusForSuccess();

            return ToType<List<TestCycle>>(response.ResponseBody.ContentString);
        }

        public List<TestCase> IssueLinksTestCases(string issueKey)
        {
            if (issueKey.IsEmpty()) throw new ArgumentNullException(nameof(issueKey));

            var response = OpenRequest($"/{ZephyrApiVersion}/issuelinks/{issueKey}/testcases").Get();

            response.AssertResponseStatusForSuccess();

            return ToType<List<TestCase>>(response.ResponseBody.ContentString);
        }
        #endregion
    }
}
