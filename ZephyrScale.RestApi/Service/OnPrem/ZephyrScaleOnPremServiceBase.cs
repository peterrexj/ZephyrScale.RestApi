using Pj.Library;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TestAny.Essentials.Api;
using TestAny.Essentials.Core;

namespace ZephyrScale.RestApi.Service.OnPrem
{
    public abstract class ZephyrScaleOnPremServiceBase : ZephyrScaleServiceBase
    {
        protected readonly object _lock = new object();
        private static bool _loginVerified = false;
        private string _username;
        private string _password;
        protected ZephyrScaleOnPremServiceBase(string appUrl,
           string serviceUsername,
           string servicePassword,
           string zephyrApiVersion,
           string jiraApiVersion,
           string folderSeparator,
           string logPrefix,
           int pageSizeSearchResult,
           int requestRetryTimes,
           int timeToSleepBetweenRetryInMilliseconds,
           bool assertResponseStatusOk,
           HttpStatusCode[] listOfResponseCodeOnFailureToRetry,
           int requestTimeoutInSeconds)
        {
            SetBaseValues(appUrl, serviceUsername, servicePassword, zephyrApiVersion, 
                jiraApiVersion, folderSeparator, logPrefix, pageSizeSearchResult,
                requestRetryTimes, timeToSleepBetweenRetryInMilliseconds, 
                assertResponseStatusOk, listOfResponseCodeOnFailureToRetry,
                requestTimeoutInSeconds);
        }

        private void SetBaseValues(string appUrl,
            string serviceUsername,
            string servicePassword,
            string zephyrApiVersion,
            string jiraApiVersion,
            string folderSeparator,
            string logPrefix,
            int pageSizeSearchResult,
            int requestRetryTimes,
            int timeToSleepBetweenRetryInMilliseconds,
            bool assertResponseStatusOk,
            HttpStatusCode[] listOfResponseCodeOnFailureToRetry,
            int requestTimeoutInSeconds)
        {
            if (appUrl.IsEmpty())
            {
                throw new Exception($"The url to the {_appName} is required");
            }
            if (appUrl.ContainsDomain() == false)
            {
                throw new Exception($"The url to the {_appName} is not in the correct format");
            }

            ZeypherUrl = appUrl.GetDomain();
            ZephyrApiVersion = zephyrApiVersion.ReplaceMultiple("", "/", @"\");
            JiraApiVersion = jiraApiVersion.ReplaceMultiple("", "/", @"\");
            _username = serviceUsername;
            _password = servicePassword;
            FolderSeparator = folderSeparator;

            _appFullEndpoint = ZeypherUrl;
            _logPrefix = logPrefix;
            PageSizeSearch = pageSizeSearchResult;
            RequestRetryTimes = requestRetryTimes;
            TimeToSleepBetweenRetryInMilliseconds = timeToSleepBetweenRetryInMilliseconds;
            AssertResponseStatusOk = assertResponseStatusOk;
            ListOfResponseCodeOnFailureToRetry = listOfResponseCodeOnFailureToRetry;
            TestAnyAppConfig.DefaultApiResponseTimeoutWaitPeriodInSeconds = requestTimeoutInSeconds;
        }

        public virtual bool CanConnect
        {
            get
            {
                if (_loginVerified == false)
                {
                    _loginVerified = CanLogin();
                }
                return _loginVerified;
            }
        }

        protected virtual bool CanLogin()
        {
            TestApiResponse testResponse = null;
            for (int i = 0; i < 10; i++)
            {
                Log($"Zephyr check health status to {_appFullEndpoint}");
                testResponse = new TestApiHttp()
                      .SetEnvironment(_appFullEndpoint)
                      .PrepareRequest($"/rest/api/{JiraApiVersion}/serverInfo")
                      .AddBasicAuthorizationHeader(_username, _password)
                      .SetNtmlAuthentication()
                      .Get();

                if (testResponse?.ResponseCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Log($"Failed to communicate with {_appName} as the user pin is incorrect");
                    throw new Exception($"Failed to communicate with {_appName} as the user pin is incorrect");
                }
                else if (testResponse?.ResponseCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    Log($"{_appName} health status to {_appFullEndpoint} check failed with response code: {testResponse?.ResponseCode.ToString()}");
                    System.Threading.Thread.Sleep(2000);
                }
            }

            Log($"Could not communicate with the {_appName} server. Response code: {testResponse?.ResponseCode.ToString()}, Response Body: {testResponse?.ResponseBody?.ContentString}");
            throw new Exception($"Could not communicate with the {_appName} server. Response code: {testResponse?.ResponseCode.ToString()}, Response Body: {testResponse?.ResponseBody?.ContentString}");
        }
        protected virtual void CheckConnection()
        {
            if (!CanConnect)
            {
                throw new Exception($"Cannot communicate to the {_appName}");
            }
        }

        protected TestApiRequest OpenRequest(string requestUrl)
        {
            CheckConnection();

            return new TestApiHttp()
                .SetEnvironment(_appFullEndpoint)
                .PrepareRequest(requestUrl)
                .AddBasicAuthorizationHeader(_username, _password)
                .SetNtmlAuthentication();
        }
    }
}
