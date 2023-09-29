using Jira.Rest.Sdk;
using NUnit.Framework;
using Pj.Library;
using System;
using System.Collections.Generic;
using System.Text;
using ZephyrScale.RestApi.IntegrationTests.Configs;
using ZephyrScale.RestApi.Service.Cloud;

namespace ZephyrScale.RestApi.IntegrationTests
{
    public class TestCaseBase
    {
        protected ZephyrScaleCloudService _zephyrService;
        protected JiraService _jiraService;
        protected string _jiraProjectKey => Configs.Configurations.JiraProjectKey;

        public static void EnableProxyOnJiraZephyrService()
        {
            if (Configurations.RequestProxyIsEnabled)
            {
                PjUtility.EnvironmentConfig.AppSettingsConfigData.AddOrUpdate("Proxy_IsProxyRequired", Configurations.RequestProxyIsEnabled.ToString());
                PjUtility.EnvironmentConfig.AppSettingsConfigData.AddOrUpdate($"Proxy_{Configurations.RequestProxyName}_Host", Configurations.RequestProxyHost);
                PjUtility.EnvironmentConfig.AppSettingsConfigData.AddOrUpdate($"Proxy_{Configurations.RequestProxyName}_Port", Configurations.RequestProxyPort.ToString());
                PjUtility.EnvironmentConfig.AppSettingsConfigData.AddOrUpdate($"Proxy_{Configurations.RequestProxyName}_ByPassProxyOnLocal", Configurations.RequestProxyByPassProxyOnLocal.ToString());
                PjUtility.EnvironmentConfig.AppSettingsConfigData.AddOrUpdate($"Proxy_{Configurations.RequestProxyName}_UseDefaultCredentials", Configurations.RequestProxyUseDefaultCredentials.ToString());
                PjUtility.EnvironmentConfig.AppSettingsConfigData.AddOrUpdate($"Proxy_{Configurations.RequestProxyName}_UseOnlyInPipeline", Configurations.RequestProxyUseOnlyInPipeline.ToString());

                PjUtility.EnvironmentConfig.AppSettingsConfigData.AddOrUpdate($"Proxy_{Configurations.RequestProxyName}_UsernameEnvironmentKeyName", "Request.Proxy.Username");
                PjUtility.EnvironmentConfig.AppSettingsConfigData.AddOrUpdate($"Proxy_{Configurations.RequestProxyName}_PasswordEnvironmentKeyName", "Request.Proxy.Password");
            }
        }

        public TestCaseBase() { }



        [SetUp]
        public void TestSetup()
        {
            Environment.SetEnvironmentVariable("TestFrameworkLogEnable", "false");
            PjUtility.InitializeUtility();
            PjUtility.EnvironmentConfig.EnvironmentName = "Mock";
            PjUtility.EnvironmentConfig.LoadConfiguration();
            TestAny.Essentials.Core.TestAnyTestContextHelper.CreateTestContext();

            PjUtility.EnvironmentVariables.PathToEnvironmentVariableKeyNamesCollectionAssembly =
               IoHelper.CombinePath(Pj.Library.PjUtility.Runtime.ExecutingFolder, "ZephyrScale.RestApi.IntegrationTests.dll");
            PjUtility.EnvironmentVariables.PathToEnvironmentVariableKeyNamesCollectionFile = "ZephyrScale.RestApi.IntegrationTests.Configs.EnvironmentVariableNames.data";
            var envDataSetup = Pj.Library.PjUtility.EnvironmentVariables.LocalSettings.Count > 0;
            if (envDataSetup == false)
            {
                throw new Exception("The environment variable setup has some error, please check the localsettings.json file");
            }
            EnableProxyOnJiraZephyrService();

            _zephyrService = ServiceProvider.ZephyrScaleCloudService;
            _jiraService = ServiceProvider.JiraService;

            Assert.IsTrue(_zephyrService.CanConnect);
            Assert.IsTrue(_jiraService.CanConnect);
        }
    }
}
