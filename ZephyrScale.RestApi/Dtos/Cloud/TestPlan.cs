using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZephyrScale.RestApi.Dtos.Cloud
{
    public class TestPlan
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        
        [JsonProperty("id")]
        public long? Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("project")]
        public Project Project { get; set; }
        
        [JsonProperty("owner")]
        public Owner Owner { get; set; }
        
        [JsonProperty("status")]
        public Status Status { get; set; }
        
        [JsonProperty("priority")]
        public Priority Priority { get; set; }
        
        [JsonProperty("folder")]
        public Folder Folder { get; set; }
        
        [JsonProperty("createdOn")]
        public DateTime? CreatedOn { get; set; }
        
        [JsonProperty("plannedStartDate")]
        public DateTime? PlannedStartDate { get; set; }
        
        [JsonProperty("plannedEndDate")]
        public DateTime? PlannedEndDate { get; set; }
        
        [JsonProperty("actualStartDate")]
        public DateTime? ActualStartDate { get; set; }
        
        [JsonProperty("actualEndDate")]
        public DateTime? ActualEndDate { get; set; }
        
        [JsonProperty("testCycles")]
        public List<TestCycle> TestCycles { get; set; }
        
        [JsonProperty("webLinks")]
        public List<WebLink> WebLinks { get; set; }
        
        [JsonProperty("customFields")]
        public dynamic CustomFields { get; set; }
        
        [JsonProperty("links")]
        public Links Links { get; set; }
    }
}