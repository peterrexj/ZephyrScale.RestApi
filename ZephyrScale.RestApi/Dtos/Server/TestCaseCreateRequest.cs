using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Server
{
    public class TestCaseCreateRequest
    {
        [JsonProperty("projectKey")]
        public string ProjectKey { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("precondition")]
        public string Precondition { get; set; }

        [JsonProperty("objective")]
        public string Objective { get; set; }

        [JsonProperty("folder")]
        public string Folder { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("labels")]
        public List<string> Labels { get; set; }

        [JsonProperty("issueLinks")]
        public List<string> IssueLinks { get; set; }

        [JsonProperty("customFields")]
        public dynamic CustomFields { get; set; }

        [JsonProperty("component")]
        public string Component { get; set; }

        [JsonProperty("estimatedTime")]
        public int EstimatedTime { get; set; }

        [JsonProperty("testScript")]
        public TestScript TestScript { get; set; }

        [JsonProperty("parameters")]
        public Parameters Parameters { get; set; }

    }
}
