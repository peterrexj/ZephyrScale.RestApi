using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Cloud
{
    public class TestScript
    {
        [JsonProperty("self")]
        public string Self { get; set; }
    }
}
