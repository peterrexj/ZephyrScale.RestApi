using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Cloud
{
    public class TestSearchRequest : SearchRequestBase
    {
        [JsonProperty("folderId")]
        public long? folderId { get; set; }
    }
}
