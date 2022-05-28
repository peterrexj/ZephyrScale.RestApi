using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Cloud
{
    public class Pagination<T>
    {
        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("startAt")]
        public Int64? StartAt { get; set; }

        [JsonProperty("maxResults")]
        public Int64? MaxResults { get; set; }

        [JsonProperty("total")]
        public Int64? Total { get; set; }

        [JsonProperty("isLast")]
        public bool IsLast { get; set; }

        [JsonProperty("values")]
        public List<T> Values { get; set; }
    }
}
