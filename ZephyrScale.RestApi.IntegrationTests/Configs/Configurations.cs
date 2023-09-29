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

        public static string JiraEndpoint => ReadConfigData("Jira.Endpoint");
        public static string ZephyrEndpoint => ReadConfigData("Zephyr.Endpoint");
        public static string JiraUsername => ReadConfigData("Jira.Username");
        public static string JiraPassword => CryptoHelper.Decrypt(ReadConfigData("Jira.Password"), throwException: false);
        public static string ZephyrUserPin => CryptoHelper.Decrypt(ReadConfigData("Zephyr.UserPin"), throwException: false);

        public static bool RequestAssertOk => ReadConfigData("Request.AssertOk").ToBool();
        public static int RequestTotalNumberOfTimesToRetry => ReadConfigData("Request.TotalNumberOfTimesToRetry").ToInteger();
        public static int RequestSleepTimeBetweenRetryInMilliseconds => ReadConfigData("Request.SleepTimeBetweenRetryInMilliseconds").ToInteger();
        public static int RequestPageDefaultSize => ReadConfigData("Request.PageDefaultSize").ToInteger();
        public static int RequestTimeoutInSeconds => ReadConfigData("Request.TimeoutInSeconds").ToInteger();

        public static string RequestProxyName => ReadConfigData("Request.Proxy.Name");
        public static bool RequestProxyIsEnabled => ReadConfigData("Request.Proxy.IsEnabled").ToBool();
        public static string RequestProxyHost => ReadConfigData("Request.Proxy.Host");
        public static int RequestProxyPort => ReadConfigData("Request.Proxy.Port").ToInteger();
        public static bool RequestProxyByPassProxyOnLocal => ReadConfigData("Request.Proxy.ByPassProxyOnLocal").ToBool();
        public static bool RequestProxyUseDefaultCredentials => ReadConfigData("Request.Proxy.UseDefaultCredentials").ToBool();
        public static bool RequestProxyUseOnlyInPipeline => ReadConfigData("Request.Proxy.UseOnlyInPipeline").ToBool();
        public static string RequestProxyUsername => ReadConfigData("Request.Proxy.Username");
        public static string RequestProxyPassword => ReadConfigData("Request.Proxy.Password");

        public static string JiraProjectKey => ReadConfigData("JiraProjectKey");

    }


}
