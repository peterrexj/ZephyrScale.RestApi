using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Cloud
{
    public class TestExecutionSearchRequest : SearchRequestBase
    {
        [JsonProperty("testCase")]
        public string testCase { get; set; }

        [JsonProperty("testCycle")]
        public string testCycle { get; set; }

        [JsonProperty("actualEndDateAfter")]
        public DateTime? actualEndDateAfter { get; set; }

        [JsonProperty("actualEndDateBefore")]
        public DateTime? actualEndDateBefore { get; set; }

        [JsonProperty("includeStepLinks")]
        public bool? includeStepLinks { get; set; }
    }
}
