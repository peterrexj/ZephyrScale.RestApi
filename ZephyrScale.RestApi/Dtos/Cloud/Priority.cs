using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Cloud
{
    public class Priority
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("index")]
        public int? Index { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("default")]
        public bool? Default { get; set; }
    }
}
