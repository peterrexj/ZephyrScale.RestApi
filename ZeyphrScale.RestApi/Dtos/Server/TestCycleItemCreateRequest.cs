using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Server
{
    public class TestCycleItemCreateRequest
    {
        [JsonProperty("testCaseKey")]
        public string TestCaseKey { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("userKey")]
        public string UserKey { get; set; }

        [JsonProperty("executionTime")]
        public int ExecutionTime { get; set; }

        [JsonProperty("executionDate")]
        public DateTime? ExecutionDate { get; set; }

        [JsonProperty("customFields")]
        public dynamic CustomFields { get; set; }

        [JsonProperty("scriptResults")]
        public List<ScriptResult> ScriptResults { get; set; }
    }
}
