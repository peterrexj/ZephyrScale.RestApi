using Jira.Rest.Sdk;
using System;
using System.Collections.Generic;
using System.Text;
using ZephyrScale.RestApi.Service.Cloud;

namespace ZephyrScale.RestApi.IntegrationTests.Configs
{
    internal class ServiceProvider
    {
        public static ZephyrScaleCloudService ZephyrScaleCloudService =>
           new ZephyrScaleCloudService(
               Configurations.ZephyrEndpoint,
               Configurations.ZephyrUserPin,
               restApiVersion: "v2",
               pageSizeSearchResult: Configurations.RequestPageDefaultSize,
               requestRetryTimes: Configurations.RequestTotalNumberOfTimesToRetry,
               listOfResponseCodeOnFailureToRetry: new System.Net.HttpStatusCode[] { System.Net.HttpStatusCode.ProxyAuthenticationRequired, System.Net.HttpStatusCode.GatewayTimeout },
               assertResponseStatusOk: Configurations.RequestAssertOk,
               timeToSleepBetweenRetryInMilliseconds: Configurations.RequestSleepTimeBetweenRetryInMilliseconds,
               requestTimeoutInSeconds: Configurations.RequestTimeoutInSeconds,
               retryOnRequestTimeout: false,
               proxyKeyName: Configurations.RequestProxyIsEnabled ? Configurations.RequestProxyName : string.Empty);

        public static JiraService JiraService => new JiraService(
             appUrl: Configurations.JiraEndpoint,
             serviceUsername: Configurations.JiraUsername,
             servicePassword: Configurations.JiraPassword,
             isCloudVersion: true,
             pageSizeSearchResult: Configurations.RequestPageDefaultSize,
             requestRetryTimes: Configurations.RequestTotalNumberOfTimesToRetry,
             listOfResponseCodeOnFailureToRetry: new System.Net.HttpStatusCode[] { System.Net.HttpStatusCode.ProxyAuthenticationRequired, System.Net.HttpStatusCode.GatewayTimeout },
             assertResponseStatusOk: Configurations.RequestAssertOk,
             timeToSleepBetweenRetryInMilliseconds: Configurations.RequestSleepTimeBetweenRetryInMilliseconds,
             requestTimeoutInSeconds: Configurations.RequestTimeoutInSeconds,
             retryOnRequestTimeout: false,
             proxyKeyName: Configurations.RequestProxyIsEnabled ? Configurations.RequestProxyName : string.Empty);
    }
}
