using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Cloud
{
    public class SearchRequestBase
    {
        [JsonProperty("maxResults")]
        public Int64? maxResults { get; set; }

        [JsonProperty("startAt")]
        public Int64? startAt { get; set; }

        [JsonProperty("projectKey")]
        public string projectKey { get; set; }
    }
}
