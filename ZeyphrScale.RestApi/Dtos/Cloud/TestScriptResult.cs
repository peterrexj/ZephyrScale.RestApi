using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Cloud
{
    public class TestScriptResult
    {
        [JsonProperty("statusName")]
        public string StatusName { get; set; }

        [JsonProperty("actualEndDate")]
        public DateTime ActualEndDate { get; set; }

        [JsonProperty("actualResult")]
        public string ActualResult { get; set; }
    }
}
