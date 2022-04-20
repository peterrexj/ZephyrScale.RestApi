using System;
using System.Net;
using Pj.Library;
using TestAny.Essentials.Api;
using TestAny.Essentials.Core;
using TestAny.Essentials.Core.Dtos.Api;

namespace ZeyphrScale.RestApi.Service.Cloud
{
    public abstract class ZephyrScaleCloudServiceBase : ZephyrScaleServiceBase
    {
        private static bool _loginVerified = false;
        private string _apiKey;
        protected readonly object _lock = new object();

        protected ZephyrScaleCloudServiceBase(string appUrl,
            string passwordAuthKey,
            string restApiVersion,
            string folderSeparator,
            string logPrefix,
            int pageSizeSearchResult,
            int requestRetryTimes,
            int timeToSleepBetweenRetryInMilliseconds,
            bool assertResponseStatusOk,
            HttpStatusCode[] listOfResponseCodeOnFailureToRetry,
            int requestTimeoutInSeconds)
        {
            SetBaseValues(appUrl, passwordAuthKey, restApiVersion, folderSeparator, 
                logPrefix, pageSizeSearchResult, requestRetryTimes, 
                timeToSleepBetweenRetryInMilliseconds, assertResponseStatusOk, 
                listOfResponseCodeOnFailureToRetry, requestTimeoutInSeconds);
        }

        private void SetBaseValues(string appUrl,
            string passwordAuthKey,
            string restApiVersion,
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
            ZeyphrApiVersion = restApiVersion.ReplaceMultiple("", "/", @"\");
            _apiKey = passwordAuthKey;
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

        public bool CanConnect
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
            for (var i = 0; i < 10; i++)
            {
                Log($"Checking health status to {_appFullEndpoint}");
                testResponse = new TestApiHttp()
                    .SetEnvironment(_appFullEndpoint)
                    .PrepareRequest($"/{ZeyphrApiVersion}/healthcheck")
                    .SetQueryParamsAsHeader(new ParameterCollection { { "Authorization", $"Bearer {_apiKey}" } })
                    .SetNtmlAuthentication()
                    .Get();

                if (testResponse?.ResponseCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Log($"Failed to communicate with {_appName} as the user pin is incorrect");
                    throw new Exception($"Failed to communicate with {_appName} as the user pin is incorrect");
                }
                else if (testResponse?.ResponseCode == System.Net.HttpStatusCode.OK)
                {
                    Log($"health status to {_appFullEndpoint} passed");
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
                .SetQueryParamsAsHeader(new ParameterCollection { { "Authorization", $"Bearer {_apiKey}" } })
                .SetNtmlAuthentication();
        }
    }
}
