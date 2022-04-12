using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Cloud
{
    public class StatusSearchRequest : SearchRequestBase
    {
        [JsonProperty("statusType")]
        public string statusType { get; set; }
    }
}
