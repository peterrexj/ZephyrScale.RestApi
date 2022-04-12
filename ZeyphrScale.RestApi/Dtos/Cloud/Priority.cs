using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Cloud
{
    public class Priority
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("self")]
        public string Self { get; set; }
    }
}
