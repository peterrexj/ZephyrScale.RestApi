using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Cloud
{
    public class Issue
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("issueId")]
        public long? IssueId { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
