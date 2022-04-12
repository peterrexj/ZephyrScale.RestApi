using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeyphrScale.RestApi.Dtos.Jira
{
    public class Properties
    {
        [JsonProperty("propertyKey")]
        public string PropertyKey { get; set; }
    }
}
