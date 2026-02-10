using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Cloud
{
    public class TestExecution
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        [JsonProperty("testCase")]
        public TestCase TestCase { get; set; }

        [JsonProperty("environment")]
        public Environment Environment { get; set; }

        [JsonProperty("testExecutionStatus")]
        public TestExecutionStatus TestExecutionStatus { get; set; }

        [JsonProperty("actualEndDate")]
        public DateTime? ActualEndDate { get; set; }

        [JsonProperty("estimatedTime")]
        public int? EstimatedTime { get; set; }

        [JsonProperty("executionTime")]
        public int? ExecutionTime { get; set; }

        [JsonProperty("executedById")]
        public string ExecutedById { get; set; }

        [JsonProperty("assignedToId")]
        public string AssignedToId { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("automated")]
        public bool? Automated { get; set; }

        [JsonProperty("testCycle")]
        public TestCycle TestCycle { get; set; }

        [JsonProperty("customFields")]
        public dynamic CustomFields { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }

        [JsonProperty("jiraProjectVersion")]
        public Project JiraProjectVersion { get; set; }
    }
}
