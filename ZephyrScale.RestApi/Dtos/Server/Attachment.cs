using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Server
{
    public class Attachment
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("filesize")]
        public int Filesize { get; set; }
    }
}
