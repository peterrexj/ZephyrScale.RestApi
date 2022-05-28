using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Cloud
{
    public class TestCaseCreateRequest
    {
        [JsonProperty("projectKey")]
        public string ProjectKey { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("objective")]
        public string Objective { get; set; }

        [JsonProperty("precondition")]
        public string Precondition { get; set; }

        [JsonProperty("estimatedTime")]
        public int? EstimatedTime { get; set; }

        [JsonProperty("componentId")]
        public int? ComponentId { get; set; }

        [JsonProperty("priorityName")]
        public string PriorityName { get; set; }

        [JsonProperty("statusName")]
        public string StatusName { get; set; }

        [JsonProperty("folderId")]
        public long? FolderId { get; set; }

        [JsonProperty("ownerId")]
        public string OwnerId { get; set; }

        [JsonProperty("labels")]
        public List<string> Labels { get; set; }

        [JsonProperty("customFields")]
        public dynamic CustomFields { get; set; }
    }
}
