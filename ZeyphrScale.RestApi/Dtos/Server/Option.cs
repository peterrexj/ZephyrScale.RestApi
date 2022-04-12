using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Server
{
    public class Option
    {
        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
