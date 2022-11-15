using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Cloud
{
    public class TestExecutionCreateRequest
    {
        [JsonProperty("projectKey")]
        public string ProjectKey { get; set; }

        [JsonProperty("testCaseKey")]
        public string TestCaseKey { get; set; }

        [JsonProperty("testCycleKey")]
        public string TestCycleKey { get; set; }

        [JsonProperty("statusName")]
        public string StatusName { get; set; }

        [JsonProperty("testScriptResults")]
        public List<TestScriptResult> TestScriptResults { get; set; }

        [JsonProperty("environmentName")]
        public string EnvironmentName { get; set; }

        [JsonProperty("actualEndDate")]
        public string ActualEndDate { get; set; }

        [JsonProperty("executionTime")]
        public long? ExecutionTime { get; set; }

        [JsonProperty("executedById")]
        public string ExecutedById { get; set; }

        [JsonProperty("assignedToId")]
        public string AssignedToId { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("customFields")]
        public dynamic CustomFields { get; set; }
    }
}
