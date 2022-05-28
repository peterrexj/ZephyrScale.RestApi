using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Cloud
{
    public class Component
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("self")]
        public string Self { get; set; }
    }
}
