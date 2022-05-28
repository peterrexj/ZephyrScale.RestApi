using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZephyrScale.RestApi.Dtos.Jira
{
    public class Properties
    {
        [JsonProperty("propertyKey")]
        public string PropertyKey { get; set; }
    }
}
