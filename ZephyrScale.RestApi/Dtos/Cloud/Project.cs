using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Cloud
{
    public class Project
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("jiraProjectId")]
        public long JiraProjectId { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
}
