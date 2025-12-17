using Jira.Rest.Sdk;
using NUnit.Framework;
using Pj.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZephyrScale.RestApi.Dtos.Cloud;
using ZephyrScale.RestApi.IntegrationTests.Configs;
using ZephyrScale.RestApi.Service.Cloud;
using Environment = System.Environment;

namespace ZephyrScale.RestApi.IntegrationTests
{
    public class TestCaseBase
    {
        protected ZephyrScaleCloudService _zephyrService;
        protected JiraService _jiraService;
        protected string _jiraProjectKey => Configs.Configurations.JiraProjectKey;

        private static void EnableProxyOnJiraZephyrService()
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
        
        
        protected TestCaseCreateRequest CreateValidTestCaseRequest(string? name = null)
        {
            return new TestCaseCreateRequest
            {
                ProjectKey = _jiraProjectKey,
                Name = name ?? $"Integration Test Case {DateTimeEx.GetDateTimeReadable()}",
                Objective = $"Integration test objective for {name}",
                Precondition = "Test preconditions",
                // ... other properties ...
                CustomFields = new Dictionary<string, object>
                {
                    { "Method", "Automated" },
                    { "TestLevel", "ST" }
                }
            };
        }
        
        protected TestExecutionCreateRequest CreateValidTestExecutionRequest(string testCaseKey)
        {
            // Get available statuses and use the first one
            var statuses = _zephyrService.StatusesGetFull(_jiraProjectKey);
            var defaultStatus = statuses?.FirstOrDefault();

            return new TestExecutionCreateRequest
            {
                ProjectKey = _jiraProjectKey,
                TestCaseKey = testCaseKey,
                StatusName = defaultStatus?.Name ?? "Not Executed",
                Comment = $"Integration test execution created at {DateTimeEx.GetDateTimeReadable()}",
                ExecutionTime = 3600, // 1 hour in seconds
                ExecutedById = null, // Will use current user
                AssignedToId = null
            };
        }
    }
}
