using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Cloud
{
    public class TestExecutionStatus
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("self")]
        public string Self { get; set; }
    }
}
