using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Cloud
{
    public class TestCycleCreateRequest
    {
        [JsonProperty("projectKey")]
        public string ProjectKey { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("plannedStartDate")]
        public DateTime? PlannedStartDate { get; set; }

        [JsonProperty("plannedEndDate")]
        public DateTime? PlannedEndDate { get; set; }

        [JsonProperty("jiraProjectVersion")]
        public long? JiraProjectVersion { get; set; }

        [JsonProperty("statusName")]
        public string StatusName { get; set; }

        [JsonProperty("folderId")]
        public long? FolderId { get; set; }

        [JsonProperty("ownerId")]
        public string OwnerId { get; set; }

        [JsonProperty("customFields")]
        public dynamic CustomFields { get; set; }
    }
}
