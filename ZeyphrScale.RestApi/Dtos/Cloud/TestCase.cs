using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Cloud
{
    public class TestCase
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty("objective")]
        public string Objective { get; set; }

        [JsonProperty("precondition")]
        public string Precondition { get; set; }

        [JsonProperty("estimatedTime")]
        public int? EstimatedTime { get; set; }

        [JsonProperty("labels")]
        public List<string> Labels { get; set; }

        [JsonProperty("component")]
        public Component Component { get; set; }

        [JsonProperty("priority")]
        public Priority Priority { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("folder")]
        public Folder Folder { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        [JsonProperty("testScript")]
        public TestScript TestScript { get; set; }

        //[JsonProperty("customFields")]
        //public CustomFields CustomFields { get; set; }

        [JsonProperty("customFields")]
        public dynamic CustomFields { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }
    }
}
