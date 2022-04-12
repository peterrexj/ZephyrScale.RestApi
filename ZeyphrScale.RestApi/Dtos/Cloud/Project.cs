using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Cloud
{
    public class Project
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("jiraProjectId")]
        public int JiraProjectId { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
}
