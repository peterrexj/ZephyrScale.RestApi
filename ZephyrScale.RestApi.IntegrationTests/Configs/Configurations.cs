using Pj.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.IntegrationTests.Configs
{
    internal static class Configurations
    {
        static string ReadConfigData(string key)
        {
            if (PjUtility.EnvironmentVariables.GetValue(key).HasValue())
            {
                return PjUtility.EnvironmentVariables.GetValue(key);
            }
            if (PjUtility.EnvironmentConfig.AppSettingsConfigData.ContainsKey(key))
            {
                return PjUtility.EnvironmentConfig.AppSettingsConfigData[key];
            }
            else
            {
                throw new Exception($"The specified key [{key}] is not available in the configuration file");
            }
        }

        public static string JiraEndpoint => ReadConfigData("Jira_Endpoint");
        public static string ZephyrEndpoint => ReadConfigData("Zephyr_Endpoint");
        public static string JiraUsername => ReadConfigData("Jira_Username");
        public static string JiraPassword => CryptoHelper.Decrypt(ReadConfigData("Jira_Password"), throwException: false);
        public static string ZephyrUserPin => CryptoHelper.Decrypt(ReadConfigData("Zephyr_UserPin"), throwException: false);

        public static bool RequestAssertOk => ReadConfigData("Request_AssertOk").ToBool();
        public static int RequestTotalNumberOfTimesToRetry => ReadConfigData("Request_TotalNumberOfTimesToRetry").ToInteger();
        public static int RequestSleepTimeBetweenRetryInMilliseconds => ReadConfigData("Request_SleepTimeBetweenRetryInMilliseconds").ToInteger();
        public static int RequestPageDefaultSize => ReadConfigData("Request_PageDefaultSize").ToInteger();
        public static int RequestTimeoutInSeconds => ReadConfigData("Request_TimeoutInSeconds").ToInteger();

        public static string RequestProxyName => ReadConfigData("Request_Proxy_Name");
        public static bool RequestProxyIsEnabled => ReadConfigData("Request_Proxy_IsEnabled").ToBool();
        public static string RequestProxyHost => ReadConfigData("Request_Proxy_Host");
        public static int RequestProxyPort => ReadConfigData("Request_Proxy_Port").ToInteger();
        public static bool RequestProxyByPassProxyOnLocal => ReadConfigData("Request_Proxy_ByPassProxyOnLocal").ToBool();
        public static bool RequestProxyUseDefaultCredentials => ReadConfigData("Request_Proxy_UseDefaultCredentials").ToBool();
        public static bool RequestProxyUseOnlyInPipeline => ReadConfigData("Request_Proxy_UseOnlyInPipeline").ToBool();
        public static string RequestProxyUsername => ReadConfigData("Request_Proxy_Username");
        public static string RequestProxyPassword => ReadConfigData("Request_Proxy_Password");

        public static string JiraProjectKey => ReadConfigData("JiraProjectKey");

    }


}
