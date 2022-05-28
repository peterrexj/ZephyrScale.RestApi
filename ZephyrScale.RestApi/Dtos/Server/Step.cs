using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Server
{
    public class Step
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("testData")]
        public string TestData { get; set; }

        [JsonProperty("expectedResult")]
        public string ExpectedResult { get; set; }
    }
}
