using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Server
{
    public class TestCycleCreateRequest
    {
        [JsonProperty("projectKey")]
        public string ProjectKey { get; set; }

        [JsonProperty("testPlanKey")]
        public string TestPlanKey { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("iteration")]
        public string Iteration { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("folder")]
        public string Folder { get; set; }

        [JsonProperty("issueKey")]
        public string IssueKey { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("plannedStartDate")]
        public DateTime PlannedStartDate { get; set; }

        [JsonProperty("plannedEndDate")]
        public DateTime PlannedEndDate { get; set; }

        [JsonProperty("customFields")]
        public dynamic CustomFields { get; set; }

        [JsonProperty("items")]
        public List<TestCycleItemCreateRequest> Items { get; set; }
    }
}
