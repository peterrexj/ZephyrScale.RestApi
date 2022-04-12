using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Cloud
{
    public class Links
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("issues")]
        public List<Issue> Issues { get; set; }

        [JsonProperty("webLinks")]
        public List<WebLink> WebLinks { get; set; }
    }
}
