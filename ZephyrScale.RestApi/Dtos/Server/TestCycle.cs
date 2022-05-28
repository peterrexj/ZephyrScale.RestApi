using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Server
{
    public class TestCycle
    {
        [JsonProperty("executionResultsSummary")]
        public ExecutionResultsSummary ExecutionResultsSummary { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("projectKey")]
        public string ProjectKey { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("iteration")]
        public string Iteration { get; set; }

        [JsonProperty("folder")]
        public string Folder { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("issueKey")]
        public string IssueKey { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("plannedStartDate")]
        public DateTime? PlannedStartDate { get; set; }

        [JsonProperty("plannedEndDate")]
        public DateTime? PlannedEndDate { get; set; }

        [JsonProperty("estimatedTime")]
        public long? EstimatedTime { get; set; }

        [JsonProperty("executionTime")]
        public long? ExecutionTime { get; set; }

        [JsonProperty("testCaseCount")]
        public long? TestCaseCount { get; set; }

        [JsonProperty("issueCount")]
        public long? IssueCount { get; set; }

        [JsonProperty("items")]
        public List<TestCycleItem> Items { get; set; }
    }
}
