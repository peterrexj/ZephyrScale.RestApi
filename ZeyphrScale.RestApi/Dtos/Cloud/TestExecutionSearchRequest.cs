using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Cloud
{
    public class TestExecutionSearchRequest : SearchRequestBase
    {
        [JsonProperty("testCase")]
        public string TestCase { get; set; }

        [JsonProperty("testCycle")]
        public string TestCycle { get; set; }

        [JsonProperty("actualEndDateAfter")]
        public DateTime? ActualEndDateAfter { get; set; }

        [JsonProperty("actualEndDateBefore")]
        public DateTime? ActualEndDateBefore { get; set; }

        [JsonProperty("includeStepLinks")]
        public bool? IncludeStepLinks { get; set; }
    }
}
