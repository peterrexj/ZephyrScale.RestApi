using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Server
{
    public partial class TestScript
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("steps")]
        public List<Step> Steps { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }
    }
}
