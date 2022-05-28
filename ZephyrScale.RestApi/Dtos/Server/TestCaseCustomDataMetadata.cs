using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Server
{
    public class TestCaseCustomDataMetadata
    {
        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("projectId")]
        public int ProjectId { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }

        [JsonProperty("options")]
        public List<Option> Options { get; set; }
    }
}
