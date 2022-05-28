using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Server
{
    public class ExecutionResultsSummary
    {
        [JsonProperty("Pass")]
        public int Pass { get; set; }

        [JsonProperty("Fail")]
        public int Fail { get; set; }
    }
}
