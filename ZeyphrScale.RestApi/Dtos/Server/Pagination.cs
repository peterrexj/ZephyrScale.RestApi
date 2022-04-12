using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Server
{
    public class Pagination<T>
    {
        [JsonProperty("startAt")]
        public Int64? StartAt { get; set; }

        [JsonProperty("maxResults")]
        public Int64? MaxResults { get; set; }

        [JsonProperty("total")]
        public Int64? Total { get; set; }

        [JsonProperty("values")]
        public List<T> Values { get; set; }
    }
}
