using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZephyrScale.RestApi.Dtos.Cloud
{
    public class TestPlanCreateRequest
    {
        [JsonProperty("projectKey")]
        public string ProjectKey { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("ownerId")]
        public long? OwnerId { get; set; }
        
        [JsonProperty("statusId")]
        public long? StatusId { get; set; }
        
        [JsonProperty("priorityId")]
        public long? PriorityId { get; set; }
        
        [JsonProperty("folderId")]
        public long? FolderId { get; set; }
        
        [JsonProperty("plannedStartDate")]
        public DateTime? PlannedStartDate { get; set; }
        
        [JsonProperty("plannedEndDate")]
        public DateTime? PlannedEndDate { get; set; }
        
        [JsonProperty("testCycleKeys")]
        public List<string> TestCycleKeys { get; set; }
        
        [JsonProperty("webLinks")]
        public List<WebLink> WebLinks { get; set; }
        
        [JsonProperty("customFields")]
        public dynamic CustomFields { get; set; }
    }
}