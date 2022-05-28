using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Server
{
    public class TestExecutionResultCreate
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("testCaseKey")]
        public string TestCaseKey { get; set; }

        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("assignedTo")]
        public string AssignedTo { get; set; }

        [JsonProperty("executedBy")]
        public string ExecutedBy { get; set; }

        [JsonProperty("executionTime")]
        public long ExecutionTime { get; set; }

        [JsonProperty("actualStartDate")]
        public DateTime ActualStartDate { get; set; }

        [JsonProperty("actualEndDate")]
        public DateTime ActualEndDate { get; set; }

        [JsonProperty("customFields")]
        public dynamic CustomFields { get; set; }

        [JsonProperty("issueLinks")]
        public List<string> IssueLinks { get; set; }

        [JsonProperty("scriptResults")]
        public List<ScriptResult> ScriptResults { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }
}
